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

        public string SizeDescription
        {
            get
            {
                long length = File.Length;
                int power = 0;

                // I'm sure there's a better way to find KiB, MiB, GiB, etc... I'm tired.
                while (length > 1024)
                {
                    length /= 1024;
                    power += 1;
                }

                switch (power)
                {
                    case 1:
                        return $"{length} KiB";
                    case 2:
                        return $"{length} MiB";
                    case 3:
                        return $"{length} GiB";
                    case 4:
                        // Maximum file size is 256 Terabytes. We don't need to go any higher than this
                        return $"{length} TiB";
                    default:
                        return $"{length} bytes";
                }
            }
        }

        public string KiBDescription => $"{File.Length / 1024:N0} KB";

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