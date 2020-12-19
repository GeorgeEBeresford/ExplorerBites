using System;
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
    public class RootViewModel : IDirectoryViewModel
    {
        public RootViewModel()
        {
            Directory = new Root();
            LoadedDirectories = new List<IDirectoryViewModel>();
            LoadedContents = new List<IFileTreeViewModel>();

            LoadContentsCommand = new RelayCommand(LoadContents);
            LoadDirectoriesCommand = new RelayCommand(LoadDirectories);

            IsExpandedForTreeView = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public IFileTree FileTree => Directory;
        public IDirectory Directory { get; }
        public IDirectoryViewModel Parent => null;

        public List<IFileViewModel> LoadedFiles => null;
        public List<IDirectoryViewModel> LoadedDirectories { get; }
        public List<IFileTreeViewModel> LoadedContents { get; }

        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }


        public bool IsExpandedForTreeView { get; private set; }

        public bool IsSelectedForTreeView { get; private set; }

        public bool IsSelectedForListView { get; private set; }

        public void ExpandForTreeView()
        {
            IsExpandedForTreeView = true;
            OnPropertyChanged(nameof(IsExpandedForTreeView));
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
        }

        public void LoadFiles()
        {
            throw new InvalidOperationException("Cannot load files as the root is a collection of drives");
        }

        public void LoadDirectories()
        {
            IEnumerable<DirectoryViewModel> initialDrives = Directory.GetDirectories()
                .Select(drive => new DirectoryViewModel(drive, this));

            LoadedDirectories.Clear();
            LoadedContents.Clear();

            foreach (DirectoryViewModel drive in initialDrives)
            {
                LoadedDirectories.Add(drive);
                LoadedContents.Add(drive);
            }

            OnPropertyChanged(nameof(LoadedDirectories));
            OnPropertyChanged(nameof(LoadedContents));
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
    }
}