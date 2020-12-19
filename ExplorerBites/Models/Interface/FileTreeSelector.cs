using System.Collections.Generic;
using System.Linq;
using ExplorerBites.Models.FileSystem;

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
            ((List<IFileTree>) SelectedFileTrees).Clear();
        }

        public void SelectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            ((List<IFileTree>) SelectedFileTrees).AddRange(fileTrees);
        }

        public void SelectFileTree(IFileTree fileTree)
        {
            ((List<IFileTree>) SelectedFileTrees).Add(fileTree);
        }

        public bool DeselectFileTree(IFileTree fileTree)
        {
            return ((List<IFileTree>) SelectedFileTrees).Remove(fileTree);
        }

        public bool DeselectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            // Remove all of the specified file trees from our reference of selected file trees and return whether all of the removals were successful
            bool allAreRemoved = fileTrees.All(fileTree => ((List<IFileTree>) SelectedFileTrees).Remove(fileTree));

            return allAreRemoved;
        }
    }
}