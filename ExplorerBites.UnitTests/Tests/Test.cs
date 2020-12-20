using System.IO;
using ExplorerBites.UnitTests.Factories;
using ExplorerBites.ViewModels.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Directory = ExplorerBites.Models.FileSystem.Directory;
using File = ExplorerBites.Models.FileSystem.File;

namespace ExplorerBites.UnitTests.Tests
{
    public abstract class Test
    {
        public const string EmptyDirectoryPath = "EmptyDirectory";
        public const string DirectoryWithDirectoriesPath = "DirectoryWithDirectories";
        public const string DirectoryWithFilesPath = @"DirectoryWithDirectories\DirectoryWithFiles";
        public const string TestFilePath = @"DirectoryWithDirectories\DirectoryWithFiles\testing-file.txt";

        protected Directory DirectoryWithDirectories { get; private set; }
        protected Directory DirectoryWithFiles { get; private set; }
        protected Directory EmptyDirectory { get; private set; }
        protected File TestFile { get; private set; }

        protected DirectoryViewModel DirectoryViewModelWithDirectories { get; private set; }
        protected DirectoryViewModel DirectoryViewModelWithFiles { get; private set; }
        protected DirectoryViewModel EmptyDirectoryViewModel { get; private set; }
        protected FileViewModel TestFileViewModel { get; private set; }

        [TestInitialize]
        public void Initialise()
        {
            CreateFileStructure();

            MockDirectoryFactory directoryFactory = new MockDirectoryFactory();
            MockDirectoryViewModelFactory directoryViewModelFactory = new MockDirectoryViewModelFactory();
            MockFileFactory fileFactory = new MockFileFactory(directoryFactory);
            MockFileViewModelFactory fileViewModelFactory = new MockFileViewModelFactory(fileFactory);

            EmptyDirectory = directoryFactory.GetEmpty();
            EmptyDirectoryViewModel = directoryViewModelFactory.GetEmpty();
            DirectoryWithFiles = directoryFactory.GetWithFiles();
            DirectoryViewModelWithFiles = directoryViewModelFactory.GetWithFiles();
            DirectoryWithDirectories = directoryFactory.GetWithDirectories();
            DirectoryViewModelWithDirectories = directoryViewModelFactory.GetWithDirectories();
            TestFile = fileFactory.GetTextFile();
            TestFileViewModel = fileViewModelFactory.GetTextFile();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DeleteFileStructure();
        }

        private static void CreateFileStructure()
        {
            DirectoryInfo emptyDirectory = new DirectoryInfo(EmptyDirectoryPath);

            // If we escaped our unit tests rather than letting them finish, the Cleanup() method may not have been called
            if (!emptyDirectory.Exists)
            {
                emptyDirectory.Create();
            }

            DirectoryInfo directoryWithDirectories = new DirectoryInfo(DirectoryWithDirectoriesPath);

            if (!directoryWithDirectories.Exists)
            {
                directoryWithDirectories.Create();
            }

            DirectoryInfo directoryWithFiles = new DirectoryInfo(DirectoryWithFilesPath);

            if (!directoryWithFiles.Exists)
            {
                directoryWithFiles.Create();
            }

            FileInfo fileInfo = new FileInfo(TestFilePath);

            if (!fileInfo.Exists)
            {
                using (StreamWriter streamWriter = fileInfo.CreateText())
                {
                    streamWriter.WriteLine("Test document. This should be removed when the unit tests are finished");
                }
            }
        }

        private static void DeleteFileStructure()
        {
            DirectoryInfo directoryWithDirectories = new DirectoryInfo(DirectoryWithDirectoriesPath);
            directoryWithDirectories.Delete(true);

            DirectoryInfo emptyDirectory = new DirectoryInfo(EmptyDirectoryPath);
            emptyDirectory.Delete();
        }
    }
}