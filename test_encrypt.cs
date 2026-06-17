using System;
using System.Security.Cryptography;
using System.Text;

class Program {
    static void Main() {
        string[] passwords = new string[] { "admin", "password", "admin123", "admin@123", "pass@123", "PulseLogic123", "123456" };
        string Salt = "PulseLogicsalt";
        foreach (string pass in passwords) {
            byte[] data = UTF8Encoding.UTF8.GetBytes(pass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Salt));
                using (TripleDESCryptoServiceProvider tripdes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripdes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string enc = Convert.ToBase64String(results, 0, results.Length);
                    Console.WriteLine(pass + " -> " + enc);
                    if (enc == "28A7hi0jvH0=" || enc == "/mg6Zc6XX/gWr695xeuq0g==") {
                        Console.WriteLine("MATCH FOUND!");
                    }
                }
            }
        }
    }
}