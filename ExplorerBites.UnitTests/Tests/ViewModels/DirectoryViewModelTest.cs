using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExplorerBites.UnitTests.Tests.ViewModels
{
    [TestCategory("ViewModels.FileSystem")]
    [TestClass]
    public class DirectoryViewModelTest : Test
    {
        [TestMethod]
        public void AreDirectoriesLoaded()
        {
            // Empty directory
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedDirectories.Count, "Directory should not have any directories loaded");

            EmptyDirectoryViewModel.LoadDirectories();
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedDirectories.Count, "No directories should be found in the view model after we have loaded the sub-directories");

            // Directory with just files
            Assert.AreEqual(0, DirectoryViewModelWithFiles.LoadedDirectories.Count, "Directory should not have any directories loaded");

            DirectoryViewModelWithFiles.LoadDirectories();
            Assert.AreEqual(0, DirectoryViewModelWithFiles.LoadedDirectories.Count, "No directories should be found in the view model after we have loaded the sub-directories");

            // Directory with just folders
            Assert.AreEqual(1, DirectoryViewModelWithDirectories.LoadedDirectories.Count, "Directory has not been asked to load its sub-directories. It should only have 1 directory and it should be blank");

            Assert.IsNull(DirectoryViewModelWithDirectories.LoadedDirectories.First(), "Loaded directory should be null, to allow the tree view to be expanded");

            DirectoryViewModelWithDirectories.LoadDirectories();
            Assert.AreNotEqual(0, DirectoryViewModelWithDirectories.LoadedDirectories.Count, "Directory should find sub-directories after they have been loaded");
        }

        [TestMethod]
        public void AreFilesLoaded()
        {
            // Empty directory
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedFiles.Count, "Empty directory should not have any files loaded");

            EmptyDirectoryViewModel.LoadFiles();

            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedFiles.Count, "No files should be found in the empty view model after we have loaded the files");

            // Directory with just folders
            Assert.AreEqual(0, DirectoryViewModelWithDirectories.LoadedFiles.Count, "Directory should not have any files loaded");

            DirectoryViewModelWithDirectories.LoadFiles();
            Assert.AreEqual(0, DirectoryViewModelWithDirectories.LoadedFiles.Count, "No files should be found in the view model after we have loaded the files");

            // Directory with just files
            Assert.AreEqual(0, DirectoryViewModelWithFiles.LoadedFiles.Count, "Directory has not been asked to load its files. It should have no files loaded yet");

            DirectoryViewModelWithFiles.LoadFiles();
            Assert.AreNotEqual(0, DirectoryViewModelWithFiles.LoadedFiles.Count, "Directory should find files after they have been loaded");

        }

        [TestMethod]
        public void AreContentsLoaded()
        {
            // Empty directory
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedContents.Count, "Empty directory should not have any files loaded");

            EmptyDirectoryViewModel.LoadContents();

            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedContents.Count, "No files should be found in the empty view model after we have loaded the files");

            // Directory with just files
            Assert.AreEqual(0, DirectoryViewModelWithFiles.LoadedContents.Count, "Directory should not have any contents loaded");

            DirectoryViewModelWithFiles.LoadContents();
            Assert.AreEqual(1, DirectoryViewModelWithFiles.LoadedContents.Count, "1 content should be found in the view model after we have loaded the contents");

            // Directory with just folders
            Assert.AreEqual(0, DirectoryViewModelWithDirectories.LoadedContents.Count, "Directory has not been asked to load its contents. It should have no contents yet");

            DirectoryViewModelWithDirectories.LoadContents();
            Assert.AreEqual(1, DirectoryViewModelWithDirectories.LoadedContents.Count, "Directory should find contents after they have been loaded");
        }

        [TestMethod]
        public void IsLoadDirectoriesCommandExecuted()
        {
            // Empty directory
            EmptyDirectoryViewModel.LoadDirectoriesCommand.Execute(null);
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedDirectories.Count, "No directories should be loaded after we have executed the LoadDirectories command");

            // Directory with just folders
            DirectoryViewModelWithDirectories.LoadDirectoriesCommand.Execute(null);
            Assert.AreEqual(1, DirectoryViewModelWithDirectories.LoadedDirectories.Count, "1 Directory should be loaded after we have executed the LoadDirectories command");

            // Directory with just files
            DirectoryViewModelWithFiles.LoadDirectoriesCommand.Execute(null);
            Assert.AreEqual(0, DirectoryViewModelWithFiles.LoadedDirectories.Count, "No directories should be loaded after we have executed the LoadDirectories command");
        }

        [TestMethod]
        public void IsLoadContentsCommandExecuted()
        {
            // Empty directory
            EmptyDirectoryViewModel.LoadContentsCommand.Execute(null);
            Assert.AreEqual(0, EmptyDirectoryViewModel.LoadedContents.Count, "No directories should be loaded after we have executed the LoadDirectories command");

            // Directory with just folders
            DirectoryViewModelWithDirectories.LoadContentsCommand.Execute(null);
            Assert.AreEqual(1, DirectoryViewModelWithDirectories.LoadedContents.Count, "1 Content should be loaded after we have executed the LoadContents command");

            // Directory with just files
            DirectoryViewModelWithFiles.LoadContentsCommand.Execute(null);
            Assert.AreEqual(1, DirectoryViewModelWithFiles.LoadedContents.Count, "1 Content should be loaded after we have executed the LoadContents command");
        }

        [TestMethod]
        public void IsDirectoryEqualToViewModel()
        {
            Assert.AreEqual(EmptyDirectory.Path, EmptyDirectoryViewModel.Directory.Path, "Directory is not equal to our view model");

            Assert.AreEqual(EmptyDirectory.Path, EmptyDirectoryViewModel.FileTree.Path, "File tree is not equal to our view model");
        }

        [TestMethod]
        public void ParentIsEqual()
        {
            Assert.AreEqual(DirectoryViewModelWithDirectories.Directory.Path, DirectoryViewModelWithFiles.Parent.Directory.Path, "Parent of view model is not the same as what we passed to the constructor");
        }

        [TestMethod]
        public void SizeDescriptionIsEmpty()
        {
            Assert.AreEqual("", DirectoryViewModelWithFiles.SizeDescription, "Size description for a directory should be an empty string");
        }

        [TestMethod]
        public void KiBDescriptionIsEmpty()
        {
            Assert.AreEqual("", DirectoryViewModelWithFiles.SizeDescription, "KibiByte description for a directory should be an empty string");
        }
    }
}
