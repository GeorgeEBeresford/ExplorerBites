using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.Models.Interface
{
    public interface IDirectorySelector
    {
        /// <summary>
        ///     The currently selected directory
        /// </summary>
        IDirectory SelectedDirectory { get; }

        /// <summary>
        ///     Finds the parent of the current directory and selects it as the current directory
        /// </summary>
        void LoadParentDirectory();

        /// <summary>
        ///     Goes backards 1 step through our directory history
        /// </summary>
        void LoadPreviousDirectory();

        /// <summary>
        ///     Goes forward 1 step through our directory history
        /// </summary>
        void UndoPreviousDirectory();

        /// <summary>
        ///     Selects a directory as the current context
        /// </summary>
        /// <param name="directory">The newly selected directory</param>
        void SelectDirectory(IDirectory directory);
    }
}