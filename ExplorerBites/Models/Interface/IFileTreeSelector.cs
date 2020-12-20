using System.Collections.Generic;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.Models.Interface
{
    public interface IFileTreeSelector
    {
        /// <summary>
        ///     A reference to any currently selected file tree nodes
        /// </summary>
        IReadOnlyCollection<IFileTreeViewModel> SelectedFileTrees { get; }

        /// <summary>
        ///     Clears the references to any selected file tree nodes
        /// </summary>
        void ClearSelection();

        /// <summary>
        ///     Adds one or more file trees to our references of selected file tree nodes
        /// </summary>
        /// <param name="fileTrees"></param>
        void SelectFileTrees(IEnumerable<IFileTreeViewModel> fileTrees);

        /// <summary>
        ///     Adds one file tree to our reference of selected file tree nodes
        /// </summary>
        /// <param name="fileTree"></param>
        void SelectFileTree(IFileTreeViewModel fileTree);

        /// <summary>
        ///     Removes one file tree from our reference of selected file tree nodes
        /// </summary>
        /// <param name="fileTree"></param>
        bool DeselectFileTree(IFileTreeViewModel fileTree);

        /// <summary>
        ///     Removes one or more file tree from our reference of selected file tree nodes
        /// </summary>
        /// <param name="fileTrees"></param>
        bool DeselectFileTrees(IEnumerable<IFileTreeViewModel> fileTrees);
    }
}