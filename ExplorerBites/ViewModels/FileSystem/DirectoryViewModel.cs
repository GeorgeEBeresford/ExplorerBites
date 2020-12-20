using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Commands;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public class DirectoryViewModel : IDirectoryViewModel
    {
        public DirectoryViewModel(IDirectory directory, IDirectoryViewModel parent)
        {
            Directory = directory;
            Parent = parent;
            LoadDirectoriesCommand = new RelayCommand(LoadDirectories);
            LoadContentsCommand = new RelayCommand(LoadContents);

            LoadedDirectories = new List<IDirectoryViewModel>();
            LoadedContents = new List<IFileTreeViewModel>();
            LoadedFiles = new List<IFileViewModel>();

            // Allow the directory to be expanded via the UI without loading the directory contents (increased performance for folders containing a lot of sub-directories)
            if (Directory.HasChildren)
            {
                LoadedDirectories.Add(null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IFileTree FileTree => Directory;
        public IDirectory Directory { get; }
        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }
        public IDirectoryViewModel Parent { get; }

        public List<IDirectoryViewModel> LoadedDirectories { get; }
        public List<IFileViewModel> LoadedFiles { get; }
        public List<IFileTreeViewModel> LoadedContents { get; }

        public string SizeDescription => "";
        public string KiBDescription => "";

        public bool IsExpandedForTreeView { get; private set; }
        public bool IsSelectedForTreeView { get; private set; }
        public bool IsSelectedForListView { get; private set; }

        public void ExpandForTreeView()
        {
            IsExpandedForTreeView = true;
            OnPropertyChanged(nameof(IsExpandedForTreeView));

            // Make sure the parent is also expanded
            if (Parent is IDirectoryViewModel expandableParent)
            {
                expandableParent.ExpandForTreeView();
            }
        }

        public void ContractForTreeView()
        {
            IsExpandedForTreeView = false;
            OnPropertyChanged(nameof(IsExpandedForTreeView));
        }

        public void SelectForTreeView()
        {
            IsSelectedForTreeView = true;
            OnPropertyChanged(nameof(IsSelectedForTreeView));
        }

        public void DeselectForTreeView()
        {
            IsSelectedForTreeView = false;
            OnPropertyChanged(nameof(IsSelectedForTreeView));
        }

        public void LoadContents()
        {
            LoadDirectories();
            LoadFiles();
        }

        public void LoadDirectories()
        {
            LoadedDirectories.Clear();

            IEnumerable<IDirectoryViewModel> directories = Directory.GetDirectories()
                .Select(directory => new DirectoryViewModel(directory, this));

            LoadedDirectories.AddRange(directories);

            OnPropertyChanged(nameof(LoadedDirectories));
            ResyncLoadedContents();
        }

        public void LoadFiles()
        {
            LoadedFiles.Clear();

            IEnumerable<IFileViewModel> files = Directory.GetFiles()
                .Select(file => new FileViewModel(file));

            LoadedFiles.AddRange(files);

            OnPropertyChanged(nameof(LoadedFiles));
            ResyncLoadedContents();
        }

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

        private void ResyncLoadedContents()
        {
            LoadedContents.Clear();
            LoadedContents.AddRange(LoadedDirectories);
            LoadedContents.AddRange(LoadedFiles);

            OnPropertyChanged(nameof(LoadedContents));
        }
    }
}