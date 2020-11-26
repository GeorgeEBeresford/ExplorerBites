using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExplorerBites.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Directory = ExplorerBites.Models.Directory;

namespace ExplorerBites.UnitTests.DirectoryBrowser
{
    [TestClass]
    public class DirectoryTest
    {
        [TestMethod]
        public void AreContentsLoaded()
        {
            IDirectory directory = new Directory("C:/");

            directory.LoadContents();
            Assert.IsTrue(directory.LoadedContents.Any(), "No contents were loaded");
        }

        [TestMethod]
        public void AreDirectoriesLoaded()
        {
            IDirectory directory = new Directory("C:/");

            directory.LoadDirectories();
            Assert.IsTrue(directory.LoadedDirectories.Any(), "No directories were loaded");
        }

        [TestMethod]
        public void AreDrivesReturned()
        {
            IEnumerable<Directory> drives = Directory.GetDrives();

            Assert.AreEqual(DriveInfo.GetDrives().Length, drives.Count());
        }

        [TestMethod]
        public void AreFilesLoaded()
        {
            IDirectory directory = new Directory("C:/");

            directory.LoadFiles();
            Assert.IsTrue(directory.LoadedFiles.Any(), "No files were loaded");
        }

        [TestMethod]
        public void DoesEnsureDirectoryExists()
        {
            try
            {
                IDirectory directory = new Directory($"C:\\{Guid.NewGuid()}");
            }
            catch (DirectoryNotFoundException)
            {
                return;
            }

            Assert.Fail("Directory was created for path that does not exist");
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