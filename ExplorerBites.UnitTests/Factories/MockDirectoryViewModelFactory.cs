using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplorerBites.UnitTests.Tests;
using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.UnitTests.Factories
{
    public class MockDirectoryViewModelFactory
    {
        public DirectoryViewModel GetEmpty()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Test.EmptyDirectoryPath);
            return GetWithPath(directoryInfo.FullName);
        }

        public DirectoryViewModel GetWithFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Test.DirectoryWithFilesPath);
            return GetWithPath(directoryInfo.FullName);
        }

        public DirectoryViewModel GetWithDirectories()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Test.DirectoryWithDirectoriesPath);
            return GetWithPath(directoryInfo.FullName);
        }

        private DirectoryViewModel GetWithPath(string path)
        {
            string[] pathParts = path.Split('\\');

            // Add the backslash back onto the drive name
            pathParts[0] = $@"{pathParts[0]}\";

            IDirectoryViewModel currentViewModel = new RootViewModel();

            foreach (string pathPart in pathParts)
            {
                currentViewModel.LoadDirectories();
                currentViewModel = currentViewModel.LoadedDirectories.First(viewModel => viewModel.Directory.Name == pathPart);
            }

            return (DirectoryViewModel) currentViewModel;
        }
    }
}
