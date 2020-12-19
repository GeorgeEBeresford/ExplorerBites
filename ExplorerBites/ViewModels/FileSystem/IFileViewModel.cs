using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public interface IFileViewModel : IFileTreeViewModel
    {
        /// <summary>
        /// The current file which we are using for our view model
        /// </summary>
        IFile File { get; }
    }
}