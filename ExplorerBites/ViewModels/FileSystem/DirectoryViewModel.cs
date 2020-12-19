using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            // Allow the directory to be expanded via the UI without loading the directory contents (increased performance for folders containing a lot of sub-directories)
            if (Directory.HasChildren)
            {
                LoadedDirectories.Add(null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }
        public IDirectory Parent { get; }
        public bool IsDirectory => Directory.IsDirectory;
        public string FileTreeType => Directory.FileTreeType;
        public string Name => Directory.Name;
        public string Path => Directory.Path;

        public bool IsValid => Directory.IsValid;
        public DateTime LastModifiedOn => Directory.LastModifiedOn;
        public List<IFileTree> LoadedContents => Directory.LoadedContents;
        public List<IDirectory> LoadedDirectories => Directory.LoadedDirectories;
        public List<IFile> LoadedFiles => Directory.LoadedFiles;

        public string SizeDescription => Directory.SizeDescription;
        public string KiBDescription => Directory.KiBDescription;

        public bool HasChildren => Directory.HasChildren;

        public bool IsExpandedForTreeView { get; private set; }

        public bool IsSelectedForTreeView { get; private set; }

        public bool IsSelectedForListView { get; private set; }

        private IDirectory Directory { get; }

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

        public void LoadContents()
        {
            LoadDirectories();
            LoadFiles();
        }

        public void LoadDirectories()
        {
            Directory.LoadDirectories();

            // The children should be view models so the interface has access to the extended functionality
            for (int directoryIndex = 0; directoryIndex < Directory.LoadedDirectories.Count; directoryIndex++)
            {
                IDirectory loadedDirectory = Directory.LoadedDirectories[directoryIndex];

                Directory.LoadedDirectories[directoryIndex] = new DirectoryViewModel(loadedDirectory, this);
            }

            OnPropertyChanged(nameof(LoadedDirectories));
            ResyncLoadedContents();
        }

        public void LoadFiles()
        {
            Directory.LoadFiles();

            // The children should be view models so the interface has access to the extended functionality
            for (int fileIndex = 0; fileIndex < Directory.LoadedFiles.Count; fileIndex++)
            {
                IFile loadedFile = Directory.LoadedFiles[fileIndex];

                Directory.LoadedFiles[fileIndex] = new FileViewModel(loadedFile);
            }

            OnPropertyChanged(nameof(LoadedFiles));
            ResyncLoadedContents();
        }

        public new string ToString()
        {
            return Name;
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