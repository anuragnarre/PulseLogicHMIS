using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using PulseLogicHMR.Security;

namespace DbRestorer
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            string repoRoot = FindRepoRoot();

            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    Console.WriteLine("Connected to LocalDB.");

                    string adminSqlPath = Path.Combine(repoRoot, "Database", "1. Admin-Db", "1. PulseLogicAdmin_CompleteDB.sql");
                    if (File.Exists(adminSqlPath))
                    {
                        Console.WriteLine("Executing Admin SQL Script...");
                        ExecuteSqlScript(conn, adminSqlPath);
                        Console.WriteLine("Admin DB script execution completed.");

                        Console.WriteLine("Updating admin password...");
                        string newPassword = GenerateRandomPassword(16);
                        string encryptedPassword = RBAC.EncryptPassword(newPassword);

                        string updateSql = @"
                            USE [DEV_PulseLogicHMR_INT];
                            UPDATE [dbo].[RBAC_User]
                            SET [Password] = @Password,
                                [NeedsPasswordUpdate] = 1,
                                [ModifiedOn] = GETDATE()
                            WHERE [UserName] = 'admin';";

                        using (var cmd = new SqlCommand(updateSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine($"Admin password updated. Your temporary password is: {newPassword}");
                        Console.WriteLine("Please login and change it immediately.");
                    }
                    else
                    {
                        Console.WriteLine("Admin SQL script not found at " + adminSqlPath);
                    }

                    string bakPath = Path.Combine(repoRoot, "Database", "2. EMR-Db", "PulseLogicInternationalDB", "extracted", "Dev_PulseLogicHMR_INT1.bak");
                    if (File.Exists(bakPath))
                    {
                        Console.WriteLine("Enabling FILESTREAM...");
                        using (var cmd = new SqlCommand("EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'filestream_access_level', 2; RECONFIGURE;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Restoring EMR DB from .bak file...");
                        RestoreEmrDatabase(conn, bakPath);
                        Console.WriteLine("EMR DB restoration completed.");
                    }
                    else
                    {
                        Console.WriteLine("EMR .bak file not found at " + bakPath);
                        Console.WriteLine("Obtain Dev_PulseLogicHMR_INT1.bak and place it at the path above.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal Error: " + ex.Message);
            }
        }

        static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            StringBuilder res = new StringBuilder();
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                RandomNumberGenerator.Fill(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }
            return res.ToString();
        }

        static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Database")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        }

        static void ExecuteSqlScript(SqlConnection conn, string scriptPath)
        {
            string script = File.ReadAllText(scriptPath);
            string[] commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string commandString in commands)
            {
                if (string.IsNullOrWhiteSpace(commandString)) continue;
                using (var cmd = new SqlCommand(commandString, conn))
                {
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception ex) { Console.WriteLine($"Error executing batch: {ex.Message}"); }
                }
            }
        }

        static void RestoreEmrDatabase(SqlConnection conn, string bakPath)
        {
            string getFileListSql = $"RESTORE FILELISTONLY FROM DISK = '{bakPath}'";
            string dataLogicalName = "";
            string logLogicalName = "";
            using (var cmd = new SqlCommand(getFileListSql, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string type = reader["Type"].ToString();
                        if (type == "D") dataLogicalName = reader["LogicalName"].ToString();
                        if (type == "L") logLogicalName = reader["LogicalName"].ToString();
                    }
                }
            }

            string defaultDataPath = "";
            string defaultLogPath = "";
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('InstanceDefaultDataPath')", conn))
            {
                defaultDataPath = cmd.ExecuteScalar()?.ToString() + "DEV_PulseLogicHMR_INT.mdf";
            }
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('InstanceDefaultLogPath')", conn))
            {
                defaultLogPath = cmd.ExecuteScalar()?.ToString() + "DEV_PulseLogicHMR_INT_log.ldf";
            }

            string restoreSql = $@"
                RESTORE DATABASE DEV_PulseLogicHMR_INT 
                FROM DISK = '{bakPath}'
                WITH REPLACE,
                MOVE '{dataLogicalName}' TO '{defaultDataPath}',
                MOVE '{logLogicalName}' TO '{defaultLogPath}'";

            using (var cmd = new SqlCommand(restoreSql, conn))
            {
                cmd.CommandTimeout = 600;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
