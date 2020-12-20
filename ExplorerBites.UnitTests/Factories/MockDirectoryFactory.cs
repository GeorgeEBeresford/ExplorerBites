using System.IO;
using ExplorerBites.UnitTests.Tests;
using Directory = ExplorerBites.Models.FileSystem.Directory;

namespace ExplorerBites.UnitTests.Factories
{
    public class MockDirectoryFactory
    {
        /// <summary>
        ///     Creates a mock directory for use in unit tests
        /// </summary>
        /// <returns></returns>
        public Directory GetEmpty()
        {
            Directory directory = new Directory(Test.EmptyDirectoryPath);

            return directory;
        }

        public Directory GetWithFiles()
        {
            Directory textFiles = new Directory(Test.DirectoryWithFilesPath);

            return textFiles;
        }

        public Directory GetWithDirectories()
        {
            Directory textFiles = new Directory(Test.DirectoryWithDirectoriesPath);

            return textFiles;
        }
    }
}