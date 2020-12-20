using System.Collections.Generic;
using ExplorerBites.Models.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExplorerBites.UnitTests.Tests.Models.FileSystem
{
    [TestCategory("Models.FileSystem")]
    [TestClass]
    public class RootTest : Test
    {
        [TestMethod]
        public void AreDirectoriesLoaded()
        {
            IDirectory root = new Root();
            ICollection<IDirectory> drives = root.GetDirectories();

            Assert.AreNotEqual(0, drives.Count, "At least 1 drive should be returned");
        }

        [TestMethod]
        public void AreContentsLoaded()
        {
            IDirectory root = new Root();
            ICollection<IFileTree> contents = root.GetContents();

            Assert.AreNotEqual(0, contents, "At least 1 content should be returned");
        }

        [TestMethod]
        public void IsRootValid()
        {
            IDirectory root = new Root();
            Assert.IsTrue(root.IsValid, "Root should always be valid");
        }

        [TestMethod]
        public void DoesRootHaveChildren()
        {
            IDirectory root = new Root();
            Assert.IsTrue(root.HasChildren, "Root should always have drives as children");
        }
    }
}