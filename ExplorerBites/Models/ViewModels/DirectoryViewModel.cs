using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Commands;

namespace ExplorerBites.Models.ViewModels
{
    public class DirectoryViewModel : IDirectoryViewModel, IDirectory, INotifyPropertyChanged
    {
        public DirectoryViewModel(IDirectory directory, IDirectoryViewModel parent)
        {
            Directory = directory;
            Parent = parent;
            LoadDirectoriesCommand = new RelayCommand(LoadDirectories);
            LoadContentsCommand = new RelayCommand(LoadContents);

            ObservableContents = new ObservableCollection<IFileTree>();
            ObservableDirectories = new ObservableCollection<IDirectory>();
            ObservableFiles = new ObservableCollection<IFile>();

            // Allow the directory to be expanded via the UI without loading the directory contents (increased performance for folders containing a lot of sub-directories)
            if (Directory.HasChildren)
            {
                ObservableDirectories.Add(null);
            }
        }

        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }

        public IFileTree Parent { get; }
        public bool IsDirectory => Directory.IsDirectory;
        public string Name => Directory.Name;
        public string Path => Directory.Path;

        public ObservableCollection<IFileTree> ObservableContents { get; }
        public ObservableCollection<IDirectory> ObservableDirectories { get; }
        public ObservableCollection<IFile> ObservableFiles { get; }

        private IDirectory Directory { get; }

        public bool Rename(string name)
        {
            return Directory.Rename(name);
        }

        public bool Move(string path)
        {
            return Directory.Move(path);
        }

        public bool Move(IDirectory directory)
        {
            return Directory.Move(directory);
        }

        public bool IsValid => Directory.IsValid;

        public List<IFileTree> LoadedContents => Directory.LoadedContents;
        public List<IDirectory> LoadedDirectories => Directory.LoadedDirectories;
        public List<IFile> LoadedFiles => Directory.LoadedFiles;

        public void LoadContents()
        {
            LoadDirectories();
            LoadFiles();
        }

        /// <summary>
        /// The value for IsExpanded is stored here. The value should be set with IsExpanded so any events are properly fired
        /// </summary>
        private bool IsExpandedCache { get; set; }

        public bool IsExpanded
        {
            get => IsExpandedCache;
            set
            {
                IsExpandedCache = value;
                OnPropertyChanged(nameof(IsExpanded));

                if (IsExpandedCache && Parent is IDirectoryViewModel parent)
                {
                    parent.IsExpanded = true;
                }
            }
        }

        public void LoadDirectories()
        {
            Directory.LoadDirectories();

            ObservableDirectories.Clear();
            foreach (IDirectory subDirectory in LoadedDirectories)
            {
                DirectoryViewModel subDirectoryViewModel = new DirectoryViewModel(subDirectory, this);

                //if (subDirectoryViewModel.IsValid)
                //{
                //    subDirectory.LoadDirectories();

                //    foreach (IDirectory subDirectoryChild in subDirectory.LoadedDirectories)
                //    {
                //        subDirectoryViewModel.ObservableDirectories.Add(subDirectoryChild);
                //    }
                //}

                ObservableDirectories.Add(subDirectoryViewModel);
            }

            ResyncLoadedContents();
        }

        public void LoadFiles()
        {
            Directory.LoadFiles();

            ObservableFiles.Clear();
            foreach (IFile file in LoadedFiles)
            {
                ObservableFiles.Add(file);
            }

            ResyncLoadedContents();
        }

        public bool HasChildren => Directory.HasChildren;

        public new string ToString()
        {
            return Name;
        }

        private void ResyncLoadedContents()
        {
            ObservableContents.Clear();

            foreach (IDirectory directory in ObservableDirectories)
            {
                ObservableContents.Add(directory);
            }

            foreach (IFile file in ObservableFiles)
            {
                ObservableContents.Add(file);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}