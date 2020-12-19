using System.ComponentModel;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public interface IFileTreeViewModel : IFileTree, INotifyPropertyChanged
    {
        /// <summary>
        ///     Whether the current item is selected in the file tree list view
        /// </summary>
        bool IsSelectedForListView { get; }

        /// <summary>
        ///     Sets the current item to be selected in the file tree list view and notifies any subscribers of the change to
        ///     <see cref="IsSelectedForListView" />
        /// </summary>
        void SelectForListView();

        /// <summary>
        ///     Sets the current item to not be selected in the file tree list view and notifies any subscribers of the change to
        ///     <see cref="IsSelectedForListView" />
        /// </summary>
        void DeselectForListView();
    }
}