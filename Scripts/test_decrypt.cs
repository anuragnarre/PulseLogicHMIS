using System;
using System.Security.Cryptography;
using System.Text;

class Program {
    static void Main() {
        string[] encs = new string[] { "28A7hi0jvH0=", "/mg6Zc6XX/gWr695xeuq0g==" };
        string[] salts = new string[] { "pulselogicsalt", "PulseLogicsalt" };
        foreach (var salt in salts) {
            Console.WriteLine("Testing Salt: " + salt);
            foreach (var enc in encs) {
                byte[] data = Convert.FromBase64String(enc);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(salt));
                    using (TripleDESCryptoServiceProvider tripdes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripdes.CreateDecryptor();
                        try {
                            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                            Console.WriteLine(enc + " -> " + UTF8Encoding.UTF8.GetString(results));
                        } catch (Exception) {
                            Console.WriteLine(enc + " -> Error");
                        }
                    }
                }
            }
        }
    }
}
