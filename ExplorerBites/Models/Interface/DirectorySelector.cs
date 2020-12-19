using System.Collections.Generic;
using System.Linq;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.Models.Interface
{
    public class DirectorySelector : IDirectorySelector
    {
        /// <summary>
        ///     A stack of directories which the user has hit the "back" button while on. If the page is changed without executing
        ///     the PreviousDirectoryCommand, this will be cleared.
        /// </summary>
        private readonly Stack<IDirectory> HistoryRollback = new Stack<IDirectory>();

        /// <summary>
        ///     A stack of directories which the user has selected another page while on.
        /// </summary>
        private readonly Stack<IDirectory> History = new Stack<IDirectory>();

        /// <summary>
        ///     The currently selected directory
        /// </summary>
        public IDirectory SelectedDirectory { get; private set; }

        public void LoadParentDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory == null)
            {
                return;
            }

            if (currentDirectory.Parent is IDirectory parentViewModel)
            {
                SelectDirectoryWithoutHistoryChange(parentViewModel);
            }

            if (currentDirectory is IDirectoryViewModel selectableDirectory)
            {
                selectableDirectory.DeselectForTreeView();
            }

            History.Push(currentDirectory);
            HistoryRollback.Clear();
        }

        public void SelectDirectory(IDirectory directory)
        {
            IDirectory currentlySelectedDirectory = SelectedDirectory;

            bool isSelectedDirectoryIdentical = currentlySelectedDirectory != null && currentlySelectedDirectory.Path == directory?.Path;

            if (isSelectedDirectoryIdentical)
            {
                return;
            }

            SelectDirectoryWithoutHistoryChange(directory);

            // If we already have something selected, deselect it and add it to our history
            if (currentlySelectedDirectory is IDirectoryViewModel selectableDirectory)
            {
                selectableDirectory.DeselectForTreeView();
                History.Push(currentlySelectedDirectory);
            }

            HistoryRollback.Clear();
        }

        public void LoadPreviousDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory == null || History.Any() == false)
            {
                return;
            }

            IDirectory historicalDirectory = History.Pop();

            if (historicalDirectory != null)
            {
                SelectDirectoryWithoutHistoryChange(historicalDirectory);
            }

            // If we already have something selected, deselect it and add it to our history
            if (currentDirectory is IDirectoryViewModel selectableDirectory)
            {
                selectableDirectory.DeselectForTreeView();
                HistoryRollback.Push(currentDirectory);
            }
        }

        public void UndoPreviousDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory != null && HistoryRollback.Any())
            {
                IDirectory historicalDirectory = HistoryRollback.Pop();

                if (historicalDirectory != null)
                {
                    SelectDirectoryWithoutHistoryChange(historicalDirectory);
                }

                if (currentDirectory is IDirectoryViewModel selectableDirectory)
                {
                    selectableDirectory.DeselectForTreeView();
                }

                History.Push(currentDirectory);
            }
        }

        private void SelectDirectoryWithoutHistoryChange(IDirectory directory)
        {
            SelectedDirectory = directory;
            SelectedDirectory.LoadContents();

            if (directory is IDirectoryViewModel selectableDirectory)
            {
                selectableDirectory.SelectForTreeView();
                selectableDirectory.ExpandForTreeView();
            }
        }
    }
}