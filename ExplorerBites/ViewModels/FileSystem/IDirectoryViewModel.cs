using System.Collections.Generic;
using System.Windows.Input;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public interface IDirectoryViewModel : IFileTreeViewModel
    {
        ICommand LoadDirectoriesCommand { get; }

        ICommand LoadContentsCommand { get; }

        IDirectoryViewModel Parent { get; }

        /// <summary>
        ///     Whether the current item is selected in the directory tree view
        /// </summary>
        bool IsSelectedForTreeView { get; }

        /// <summary>
        ///     Whether the current item is expanded in the directory tree view
        /// </summary>
        bool IsExpandedForTreeView { get; }

        /// <summary>
        ///     The current directory which we are using for our view model
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        ///     Sets the current item to be selected in the directory tree view and notifies any subscribers of the change to
        ///     <see cref="IsSelectedForTreeView" />
        /// </summary>
        void SelectForTreeView();

        /// <summary>
        ///     Sets the current item to not be selected and notifies any subscribers of the change to
        ///     <see cref="IsSelectedForTreeView" />
        /// </summary>
        void DeselectForTreeView();

        /// <summary>
        ///     Sets the current item to be expanded in the directory tree view and notifies any subscribers of the change to
        ///     <see cref="IsExpandedForTreeView" />
        /// </summary>
        void ExpandForTreeView();


        /// <summary>
        ///     Sets the current item to not be expanded in the directory tree view and notifies any subscribers of the change to
        ///     <see cref="IsExpandedForTreeView" />
        /// </summary>
        void ContractForTreeView();

        List<IDirectoryViewModel> LoadedDirectories { get; }
        List<IFileViewModel> LoadedFiles { get; }
        List<IFileTreeViewModel> LoadedContents { get; }

        void LoadContents();
        void LoadFiles();
        void LoadDirectories();
    }
}