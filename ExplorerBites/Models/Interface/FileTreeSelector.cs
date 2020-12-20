using System.Collections.Generic;
using System.Linq;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.Models.Interface
{
    public class FileTreeSelector : IFileTreeSelector
    {
        public FileTreeSelector()
        {
            SelectedFileTrees = new List<IFileTreeViewModel>();
        }

        public IReadOnlyCollection<IFileTreeViewModel> SelectedFileTrees { get; }

        public void ClearSelection()
        {
            foreach (IFileTreeViewModel selectableFileTree in SelectedFileTrees)
            {
                selectableFileTree.DeselectForListView();
            }

            ((List<IFileTreeViewModel>)SelectedFileTrees).Clear();
        }

        public void SelectFileTrees(IEnumerable<IFileTreeViewModel> fileTrees)
        {
            foreach (IFileTreeViewModel fileTree in fileTrees)
            {
                fileTree.SelectForListView();

                ((List<IFileTreeViewModel>)SelectedFileTrees).Add(fileTree);
            }
        }

        public void SelectFileTree(IFileTreeViewModel fileTree)
        {
            if (fileTree is IFileTreeViewModel selectableFileTree)
            {
                selectableFileTree.SelectForListView();
            }

            ((List<IFileTreeViewModel>)SelectedFileTrees).Add(fileTree);
        }

        public bool DeselectFileTree(IFileTreeViewModel fileTree)
        {
            bool isSuccess = ((List<IFileTreeViewModel>)SelectedFileTrees).Remove(fileTree);

            if (isSuccess)
            {
                fileTree.DeselectForListView();
            }

            return isSuccess;
        }

        public bool DeselectFileTrees(IEnumerable<IFileTreeViewModel> fileTrees)
        {
            bool allAreRemoved = true;

            // Remove the file trees from our reference of selected files
            foreach (IFileTreeViewModel fileTree in fileTrees)
            {
                bool isSuccess = ((List<IFileTreeViewModel>)SelectedFileTrees).Remove(fileTree);

                if (isSuccess)
                {
                    // If the item was successfully removed, set the IsSelected property to false
                    fileTree.DeselectForListView();
                }
                else
                {
                    allAreRemoved = false;
                }
            }

            return allAreRemoved;
        }
    }
}