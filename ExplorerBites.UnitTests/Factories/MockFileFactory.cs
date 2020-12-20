using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.UnitTests.Factories
{
    public class MockFileFactory
    {
        public MockFileFactory(MockDirectoryFactory directoryFactory)
        {
            DirectoryFactory = directoryFactory;
        }

        public MockDirectoryFactory DirectoryFactory { get; }

        public File GetTextFile()
        {
            File textFile = new File("testing-file.txt", DirectoryFactory.GetWithFiles());

            return textFile;
        }
    }
}