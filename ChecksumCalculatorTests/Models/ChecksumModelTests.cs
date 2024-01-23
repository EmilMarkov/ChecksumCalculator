using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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

        [TestMethod]
        public void CalculateAndVerifyChecksumsForTestFiles()
        {
            for (int i = 1; i <= NumberOfTestFiles; i++)
            {
                string filePath = Path.Combine(TestFilesDirectory, $"testfile{i}.txt");
                string checksum = ChecksumModel.CalculateChecksum(filePath);
                bool verificationResult = ChecksumModel.VerifyChecksum(filePath, checksum);

                Assert.IsTrue(verificationResult, $"Checksum verification failed for file {filePath}");
            }
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
    }
}