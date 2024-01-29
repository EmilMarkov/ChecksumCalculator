using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

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

        public async static void VerifyChecksum(string checksumFilePath, FileItemViewModel fileItem)
        {
            if (File.Exists(checksumFilePath))
            {
                string existingChecksum = await Task.Run(() => File.ReadAllText(checksumFilePath).Trim());
                string calculatedChecksum = await Task.Run(() => ChecksumModel.CalculateChecksum(fileItem.FilePath));

                if (string.Equals(existingChecksum, calculatedChecksum, StringComparison.OrdinalIgnoreCase))
                {
                    fileItem.Checksum = existingChecksum;
                    fileItem.Result = "Checksum is valid";
                }
                else
                {
                    fileItem.Result = "Checksum is invalid";
                }
            }
            else
            {
                string calculatedChecksum = await Task.Run(() => ChecksumModel.CalculateChecksum(fileItem.FilePath));
                await Task.Run(() => ChecksumModel.SaveChecksumToFile(fileItem.FilePath, calculatedChecksum));
                fileItem.Checksum = calculatedChecksum;
                fileItem.Result = "Checksum calculated";
            }
        }

        public static void UpdateChecksum(string filePath)
        {
            string checksum = CalculateChecksum(filePath);
            SaveChecksumToFile(filePath, checksum);
        }
    }
}
