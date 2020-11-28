using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExplorerBites.Annotations;

namespace ExplorerBites.Models.ViewModels
{
    public class RootViewModel : IDirectoryViewModel, INotifyPropertyChanged
    {
        public RootViewModel()
        {
            ObservableContents = new ObservableCollection<IFileTree>();
            ObservableDirectories = new ObservableCollection<IDirectory>();
            IsExpanded = true;
        }

        public IFileTree Parent => null;
        public bool IsDirectory => false;
        public string FileTreeType => "Root";
        public string Name => null;
        public string Path => null;

        public bool Rename(string name)
        {
            throw new NotImplementedException("Cannot rename an object with no path");
        }

        public bool Move(string path)
        {
            throw new NotImplementedException("Cannot move an object with no path");
        }

        public bool Move(IDirectory directory)
        {
            throw new NotImplementedException("Cannot move an object with no path");
        }

        public bool IsValid => true;

        public DateTime LastModifiedOn => ObservableDirectories
            .OrderBy(drive => drive.LastModifiedOn)
            .Select(drive => drive.LastModifiedOn.ToLocalTime())
            .FirstOrDefault();

        public string SizeDescription => "";
        public string KiBDescription => "";

        public void LoadContents()
        {
            LoadDirectories();
        }

        public bool IsExpanded { get; set; }

        public void LoadDirectories()
        {
            IEnumerable<DirectoryViewModel> initialDrives =
                Directory.GetDrives().Select(drive => new DirectoryViewModel(drive, this));

            ObservableDirectories.Clear();
            ObservableContents.Clear();

            foreach (DirectoryViewModel drive in initialDrives)
            {
                ObservableDirectories.Add(drive);
                ObservableContents.Add(drive);

                // The drive should have the directories ready to preview in the heirarchy
                //drive.LoadDirectories();
            }

            OnPropertyChanged(nameof(ObservableDirectories));
            OnPropertyChanged(nameof(ObservableContents));
        }

        public void LoadFiles()
        {
            throw new NotImplementedException("Cannot load files from the root view model as it only contains drives");
        }

        public ObservableCollection<IFile> ObservableFiles =>
            throw new NotImplementedException("Cannot load files from the root view model as it only contains drives");
        public ObservableCollection<IDirectory> ObservableDirectories { get; }
        public ObservableCollection<IFileTree> ObservableContents { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
