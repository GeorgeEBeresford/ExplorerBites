using System;
using System.IO;
using ExplorerBites.Models.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Directory = ExplorerBites.Models.FileSystem.Directory;
using File = ExplorerBites.Models.FileSystem.File;

namespace ExplorerBites.UnitTests.Tests.Models.FileSystem
{
    [TestCategory("Models.FileSystem")]
    [TestClass]
    public class FileTest : Test
    {
        [TestMethod]
        public void ParentIsMatching()
        {
            Assert.AreEqual(DirectoryWithFiles.Path, TestFile.Parent.Path, "Parent did not match the parent that we passed in the constructor");
        }

        [TestMethod]
        public void ReturnsCorrectFileTreeType()
        {
            Assert.AreEqual("TXT File", TestFile.FileTreeType, "File tree type did not match the expected string");
        }

        [TestMethod]
        public void ReturnsCorrectName()
        {
            Assert.AreEqual("testing-file.txt", TestFile.Name, "Name did not match the expected string");
        }

        [TestMethod]
        public void ReturnsCorrectPath()
        {
            FileInfo fileInfo = new FileInfo(TestFilePath);

            Assert.AreEqual(fileInfo.FullName, TestFile.Path, "Path did not match the expected string");
        }

        [TestMethod]
        public void ReturnsCorrectExtension()
        {
            Assert.AreEqual("txt", TestFile.Extension, "Extension did not match the expected string");
        }

        [TestMethod]
        public void ChecksWhetherFileIsValid()
        {
            // Must be invalid
            IDirectory directory = new Directory(@"C:\");
            IFile file = new File(Guid.NewGuid().ToString(), directory);
            Assert.IsFalse(file.IsValid, "File should not be valid if it does not exist");

            // Must be valid
            Assert.IsTrue(TestFile.IsValid, "File should be valid if it exists and we have sufficient permissions");
        }

        [TestMethod]
        public void ChecksWhetherContentsMatch()
        {
            byte[] testingImageContents = GetTestFileContents();

            byte[] fileContents = TestFile.GetContents();
            Assert.AreEqual(testingImageContents.Length, fileContents.Length, "File contents did not match the expected length");

            for (int byteIndex = 0; byteIndex < fileContents.Length; byteIndex++)
            {
                Assert.AreEqual(testingImageContents[byteIndex], fileContents[byteIndex], $"Byte at index {byteIndex} did not match the expected value");
            }
        }

        /// <summary>
        ///     Returns an image for testing whether file contents are returned
        /// </summary>
        /// <returns></returns>
        private static byte[] GetTestFileContents()
        {
            FileInfo fileInfo = new FileInfo(TestFilePath);
            byte[] buffer = new byte[fileInfo.Length];

            using (Stream stream = fileInfo.OpenRead())
            {
                stream.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }
    }
}