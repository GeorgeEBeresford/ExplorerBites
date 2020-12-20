using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.UnitTests.Factories
{
    public class MockFileViewModelFactory
    {
        public MockFileViewModelFactory(MockFileFactory fileFactory)
        {
            FileFactory = fileFactory;
        }

        private MockFileFactory FileFactory { get; }

        public FileViewModel GetTextFile()
        {
            return new FileViewModel(FileFactory.GetTextFile());
        }
    }
}