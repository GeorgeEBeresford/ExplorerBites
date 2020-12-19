using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExplorerBites.Annotations;
using ExplorerBites.Models.FileSystem;

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

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     The currently selected directory
        /// </summary>
        public IDirectory SelectedDirectory { get; private set; }

        public void LoadParentDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory == null) return;

            if (currentDirectory.Parent is IDirectory parentViewModel) SelectDirectory(parentViewModel);

            History.Push(currentDirectory);
            HistoryRollback.Clear();
        }

        public void SelectDirectory(IDirectory directory)
        {
            SelectedDirectory = directory;
            SelectedDirectory.LoadContents();

            // If we aren't currently at the root level and the last viewed item isn't the last item in our history, add the directory to our history
            if (directory.Parent is IDirectory parentDirectory &&
                (!History.Any() || !History.Peek().Equals(parentDirectory))) History.Push(parentDirectory);

            HistoryRollback.Clear();
        }

        public void LoadPreviousDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory != null && History.Any())
            {
                IDirectory historicalDirectory = History.Pop();

                if (historicalDirectory != null) SelectDirectory(historicalDirectory);

                HistoryRollback.Push(currentDirectory);
            }
        }

        public void UndoPreviousDirectory()
        {
            IDirectory currentDirectory = SelectedDirectory;

            if (currentDirectory != null && HistoryRollback.Any())
            {
                IDirectory historicalDirectory = HistoryRollback.Pop();

                if (historicalDirectory != null) SelectDirectory(historicalDirectory);

                History.Push(currentDirectory);
            }
        }
    }
}