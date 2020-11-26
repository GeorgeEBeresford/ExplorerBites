using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ExplorerBites.Commands;

namespace ExplorerBites.Models.ViewModels
{
    public class DirectoryViewModel : IDirectory, INotifyPropertyChanged
    {
        public DirectoryViewModel(IDirectory directory)
        {
            Directory = directory;
            LoadDirectoriesCommand = new RelayCommand(LoadDirectories);
            LoadContentsCommand = new RelayCommand(LoadContents);

            ObservableContents = new ObservableCollection<IFileTree>();
            ObservableDirectories = new ObservableCollection<IDirectory>();
            ObservableFiles = new ObservableCollection<IFile>();
        }

        public DirectoryViewModel GetUnloaded()
        {
            // Create a fresh copy of the current directory view model
            Directory.LoadedDirectories.Clear();
            Directory.LoadedContents.Clear();
            Directory.LoadedFiles.Clear();

            return new DirectoryViewModel(Directory);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }

        public IFileTree Parent => Directory.Parent;
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

        public List<IFileTree> LoadedContents => Directory.LoadedContents;
        public List<IDirectory> LoadedDirectories => Directory.LoadedDirectories;
        public List<IFile> LoadedFiles => Directory.LoadedFiles;

        public void LoadContents()
        {
            LoadDirectories();
            LoadFiles();
        }

        public void LoadDirectories()
        {
            Directory.LoadDirectories();

            ObservableDirectories.Clear();
            foreach (IDirectory directory in LoadedDirectories)
            {
                directory.LoadDirectories();

                DirectoryViewModel directoryViewModel = new DirectoryViewModel(directory);
                foreach (IDirectory subDirectory in directory.LoadedDirectories)
                {
                    directoryViewModel.ObservableDirectories.Add(subDirectory);
                }

                ObservableDirectories.Add(directoryViewModel);
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
    }
}