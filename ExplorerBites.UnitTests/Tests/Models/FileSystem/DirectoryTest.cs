using System;
using System.Collections.Generic;
using System.Linq;
using ExplorerBites.Models.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExplorerBites.UnitTests.Tests.Models.FileSystem
{
    [TestCategory("Models.FileSystem")]
    [TestClass]
    public class DirectoryTest : Test
    {
        [TestMethod]
        public void AreContentsLoaded()
        {
            // Make sure nothing is loaded
            IEnumerable<IFileTree> contents = EmptyDirectory.GetContents();
            Assert.IsFalse(contents.Any(), "No contents should be loaded for an empty directory");

            // Make sure we can load files
            contents = DirectoryWithFiles.GetContents();
            Assert.AreEqual(1, contents.Count(), $"Only 1 content should be in the {DirectoryWithFiles.Name} folder and only 1 content should be detected");

            foreach (IFileTree content in contents)
            {
                Assert.IsTrue(content.IsValid, $"Every content in the {DirectoryWithFiles.Name} folder should be valid as they exist and you should have permission to access them");
            }

            // Make sure we can load directories
            contents = DirectoryWithDirectories.GetContents();
            Assert.AreEqual(1, contents.Count(), $"Only 1 content should be in the {DirectoryWithDirectories.Name} folder and only 1 content should be detected");

            foreach (IFileTree content in contents)
            {
                Assert.IsTrue(content.IsValid, $"Every content in the {DirectoryWithDirectories.Name} folder should be valid as they exist and you should have permission to access them");
            }
        }

        [TestMethod]
        public void AreDirectoriesLoaded()
        {
            // Make sure nothing is loaded
            IEnumerable<IDirectory> directories = EmptyDirectory.GetDirectories();
            Assert.IsFalse(directories.Any(), "No directories should be loaded for an empty directory");

            // Make sure we can load directories
            directories = DirectoryWithDirectories.GetDirectories();
            Assert.AreEqual(1, directories.Count(), $"1 directory should be in the {DirectoryWithDirectories} folder and 1 directory should be detected");

            foreach (IDirectory subDirectory in directories)
            {
                Assert.IsTrue(subDirectory.IsValid, "Every directory in the Resources folder should be valid as they exist and you should have permission to access them");
            }
        }

        [TestMethod]
        public void AreFilesLoaded()
        {
            IEnumerable<IFile> files = EmptyDirectory.GetFiles();
            Assert.IsFalse(files.Any(), "No directories should be loaded for an empty directory");

            files = DirectoryWithFiles.GetFiles();
            Assert.AreEqual(1, files.Count(), $"Only 1 file should be in the {DirectoryWithFiles} folder and only 1 file should be detected");

            foreach (IFile file in files)
            {
                Assert.IsTrue(file.IsValid, $"Every file in the {DirectoryWithFiles} folder should be valid as they exist and you should have permission to access them");
            }
        }

        [TestMethod]
        public void ChecksWhetherIsValid()
        {
            IDirectory directory = new Directory($"C:\\{Guid.NewGuid()}");
            Assert.IsFalse(directory.IsValid, "Directory should be invalid as it does not exist");

            Assert.IsTrue(EmptyDirectory.IsValid, "Directory should be valid as it exists and we should have sufficient permissions");
        }

        [TestMethod]
        public void ChecksWhetherHasChildren()
        {
            IDirectory directory = new Directory($"C:\\{Guid.NewGuid()}");
            Assert.IsFalse(directory.HasChildren, "Directory should have no children as it does not exist");

            Assert.IsFalse(EmptyDirectory.HasChildren, "Directory should have no children as it is empty");

            Assert.IsFalse(DirectoryWithFiles.HasChildren, "Directory should have no children as it only contains files");

            Assert.IsTrue(DirectoryWithDirectories.HasChildren, "Directory should have children as it contains directories");
        }

        [TestMethod]
        public void HasCorrectName()
        {
            IDirectory directory = new Directory("C:/");

            Assert.AreEqual(@"C:\", directory.Name);
        }

        [TestMethod]
        public void HasCorrectParent()
        {
            Assert.AreEqual(DirectoryWithDirectories.Path, DirectoryWithFiles.Parent.Path, "Directory parent is not equal to what we passed as the constructor");
        }

        [TestMethod]
        public void HasCorrectPath()
        {
            IDirectory directory = new Directory("C:/Windows");

            Assert.AreEqual(@"C:\Windows", directory.Path);
        }
    }
}