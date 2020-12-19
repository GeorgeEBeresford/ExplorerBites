using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExplorerBites.Annotations;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public class FileViewModel : IFileViewModel
    {
        public FileViewModel(IFile file)
        {
            File = file;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public IFile File { get; }

        public IFileTree FileTree => File;

        public bool IsSelectedForListView { get; private set; }

        public void SelectForListView()
        {
            IsSelectedForListView = true;
            OnPropertyChanged(nameof(IsSelectedForListView));
        }

        public void DeselectForListView()
        {
            IsSelectedForListView = false;
            OnPropertyChanged(nameof(IsSelectedForListView));
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}