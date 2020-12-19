using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExplorerBites.Models;
using ExplorerBites.Models.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Directory = ExplorerBites.Models.FileSystem.Directory;

namespace ExplorerBites.UnitTests.DirectoryBrowser
{
    [TestClass]
    public class DirectoryTest
    {
        [TestMethod]
        public void AreContentsLoaded()
        {
            IDirectory directory = new Directory("C:/");

            IEnumerable<IFileTree> contents = directory.GetContents();
            Assert.IsTrue(contents.Any(), "No contents were loaded");
        }

        [TestMethod]
        public void AreDirectoriesLoaded()
        {
            IDirectory directory = new Directory("C:/");

            IEnumerable<IDirectory> directories = directory.GetDirectories();
            Assert.IsTrue(directories.Any(), "No directories were loaded");
        }

        [TestMethod]
        public void AreFilesLoaded()
        {
            IDirectory directory = new Directory("C:/");

            IEnumerable<IFile> files = directory.GetFiles();
            Assert.IsTrue(files.Any(), "No files were loaded");
        }

        [TestMethod]
        public void DoesCheckWhetherDirectoryExists()
        {
            IDirectory directory = new Directory($"C:\\{Guid.NewGuid()}");
            Assert.IsFalse(directory.IsValid, "Directory should be invalid as it does not exist");
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
            IDirectory directory = new Directory("C:/Windows");

            Assert.AreEqual(@"C:\", directory.Parent.Name);
        }

        [TestMethod]
        public void HasCorrectPath()
        {
            IDirectory directory = new Directory("C:/Windows");

            Assert.AreEqual(@"C:\Windows", directory.Path);
        }
    }
}