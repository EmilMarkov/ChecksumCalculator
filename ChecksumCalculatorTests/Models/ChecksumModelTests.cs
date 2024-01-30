using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChecksumCalculator.Tests
{
    [TestClass]
    public class ChecksumModelTests
    {
        private const int NumberOfTestFiles = 10;
        private const string TestFilesDirectory = "TestFiles";

        [TestInitialize]
        public void TestInitialize()
        {
            Directory.CreateDirectory(TestFilesDirectory);

            for (int i = 1; i <= NumberOfTestFiles; i++)
            {
                string filePath = Path.Combine(TestFilesDirectory, $"testfile{i}.txt");
                string randomString = GenerateRandomString();
                File.WriteAllText(filePath, randomString);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var filePath in Directory.GetFiles(TestFilesDirectory))
            {
                File.Delete(filePath);
            }
            Directory.Delete(TestFilesDirectory);
        }

        private string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] randomString = new char[10];
            for (int i = 0; i < 10; i++)
            {
                randomString[i] = chars[random.Next(chars.Length)];
            }
            return new string(randomString);
        }
        
        [TestMethod]
        public void TestChecksumCalculating()
        {
            for (int i = 1; i <= NumberOfTestFiles; i++)
            {
                string filePath = Path.Combine(TestFilesDirectory, $"testfile{i}.txt");

                string checksum_model = ChecksumModel.CalculateChecksum(filePath);
                string checksum_library = EasyEncryption.SHA.ComputeSHA256Hash(File.ReadAllText(filePath));

                Assert.AreEqual(checksum_library, checksum_model, $"Invalid checksum for file {filePath}");
            }
        }

        [TestMethod]
        public async Task TestChecksumVerifying()
        {
            string filePath = Path.Combine(TestFilesDirectory, "testfile3.txt");
            string checksumFilePath = filePath + ".checksum";
            string randomString = GenerateRandomString();

            File.WriteAllText(filePath, randomString);
            ChecksumModel.SaveChecksumToFile(filePath, ChecksumModel.CalculateChecksum(filePath));

            var fileItem = new FileItemViewModel { FilePath = filePath };

            await ChecksumModel.VerifyChecksum(checksumFilePath, fileItem);

            Assert.AreEqual("Checksum is valid", fileItem.Result);
        }

    }
}