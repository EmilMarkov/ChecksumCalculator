using System;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumCalculator
{
    public class ChecksumModel
    {
        public static string CalculateChecksum(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        public static void SaveChecksumToFile(string filePath, string checksum)
        {
            File.WriteAllText(filePath + ".checksum", checksum);
        }

        public static void UpdateChecksum(string filePath)
        {
            string checksum = CalculateChecksum(filePath);
            SaveChecksumToFile(filePath, checksum);
        }
    }
}
