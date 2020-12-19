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
            SelectedFileTrees = new List<IFileTree>();
        }

        public IReadOnlyCollection<IFileTree> SelectedFileTrees { get; }

        public void ClearSelection()
        {
            foreach (IFileTreeViewModel selectableFileTree in SelectedFileTrees.OfType<IFileTreeViewModel>())
            {
                selectableFileTree.DeselectForListView();
            }

            ((List<IFileTree>) SelectedFileTrees).Clear();
        }

        public void SelectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            foreach (IFileTree fileTree in fileTrees)
            {
                if (fileTree is IFileTreeViewModel selectableFileTree)
                {
                    selectableFileTree.SelectForListView();
                }

                ((List<IFileTree>)SelectedFileTrees).Add(fileTree);
            }
        }

        public void SelectFileTree(IFileTree fileTree)
        {
            if (fileTree is IFileTreeViewModel selectableFileTree)
            {
                selectableFileTree.SelectForListView();
            }

            ((List<IFileTree>) SelectedFileTrees).Add(fileTree);
        }

        public bool DeselectFileTree(IFileTree fileTree)
        {
            bool isSuccess = ((List<IFileTree>)SelectedFileTrees).Remove(fileTree);

            if (isSuccess && fileTree is IFileTreeViewModel selectableFileTree)
            {
                selectableFileTree.SelectForListView();
            }

            return isSuccess;
        }

        public bool DeselectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            bool allAreRemoved = true;

            // Remove the file trees from our reference of selected files
            foreach (IFileTree fileTree in fileTrees)
            {
                bool isSuccess = ((List<IFileTree>) SelectedFileTrees).Remove(fileTree);

                if (isSuccess)
                {
                    // If the item was successfully removed, set the IsSelected property to false
                    if (fileTree is IFileTreeViewModel selectableFileTree)
                    {
                        selectableFileTree.DeselectForListView();
                    }
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