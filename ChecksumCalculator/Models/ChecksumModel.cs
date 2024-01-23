using System;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumCalculator
{
    public class ChecksumModel
    {
        public static string CalculateChecksum(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        public static void SaveChecksumToFile(string filePath, string checksum)
        {
            File.WriteAllText(filePath + ".checksum", checksum);
        }

        public static bool VerifyChecksum(string filePath, string checksum)
        {
            string calculatedChecksum = CalculateChecksum(filePath);
            return string.Equals(calculatedChecksum, checksum, StringComparison.OrdinalIgnoreCase);
        }

        public static void UpdateChecksum(string filePath)
        {
            string checksum = CalculateChecksum(filePath);
            SaveChecksumToFile(filePath, checksum);
        }
    }
}
