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
        private readonly Stack<IDirectoryViewModel> HistoryRollback = new Stack<IDirectoryViewModel>();

        /// <summary>
        ///     A stack of directories which the user has selected another page while on.
        /// </summary>
        private readonly Stack<IDirectoryViewModel> History = new Stack<IDirectoryViewModel>();

        /// <summary>
        ///     The currently selected directory
        /// </summary>
        public IDirectoryViewModel SelectedDirectory { get; private set; }

        public void LoadParentDirectory()
        {
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory == null)
            {
                return;
            }

            if (currentDirectory.Parent is IDirectoryViewModel parentViewModel)
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

        public void SelectDirectory(IDirectoryViewModel newlySelectedDirectory)
        {
            IDirectoryViewModel previouslySelectedDirectory = SelectedDirectory;

            bool isSelectedDirectoryIdentical = previouslySelectedDirectory != null && previouslySelectedDirectory.Directory.Path == newlySelectedDirectory?.Directory.Path;

            if (isSelectedDirectoryIdentical)
            {
                return;
            }

            SelectDirectoryWithoutHistoryChange(newlySelectedDirectory);

            // If we already have something selected, deselect it and add it to our history
            if (previouslySelectedDirectory is IDirectoryViewModel selectableDirectory)
            {
                selectableDirectory.DeselectForTreeView();
                History.Push(previouslySelectedDirectory);
            }

            HistoryRollback.Clear();
        }

        public void LoadPreviousDirectory()
        {
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory == null || History.Any() == false)
            {
                return;
            }

            IDirectoryViewModel historicalDirectory = History.Pop();

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
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory != null && HistoryRollback.Any())
            {
                IDirectoryViewModel historicalDirectory = HistoryRollback.Pop();

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

        private void SelectDirectoryWithoutHistoryChange(IDirectoryViewModel directory)
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