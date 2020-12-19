﻿using System;
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
            IsExpanded = true;

            LoadedDirectories = new List<IDirectory>();
            LoadedContents = new List<IFileTree>();

            LoadContentsCommand = new RelayCommand(LoadContents);
            LoadDirectoriesCommand = new RelayCommand(LoadDirectories);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public IFileTree Parent => null;
        public bool IsDirectory => false;
        public string FileTreeType => "Root";
        public string Name => null;
        public string Path => null;
        public bool IsValid => true;

        public DateTime LastModifiedOn => LoadedDirectories
            .OrderBy(drive => drive.LastModifiedOn)
            .Select(drive => drive.LastModifiedOn.ToLocalTime())
            .FirstOrDefault();

        public string SizeDescription => "";
        public string KiBDescription => "";
        public List<IFileTree> LoadedContents { get; }
        public List<IDirectory> LoadedDirectories { get; }
        public List<IFile> LoadedFiles => throw new InvalidOperationException("Cannot load files from the root view model as it only contains drives");
        public bool HasChildren => LoadedContents.Any();

        public ICommand LoadDirectoriesCommand { get; }
        public ICommand LoadContentsCommand { get; }
        public bool IsExpanded { get; set; }

        public bool Rename(string name)
        {
            throw new InvalidOperationException("Cannot rename an object with no path");
        }

        public bool Move(string path)
        {
            throw new InvalidOperationException("Cannot move an object with no path");
        }

        public bool Move(IDirectory directory)
        {
            throw new InvalidOperationException("Cannot move an object with no path");
        }

        public void LoadContents()
        {
            LoadDirectories();
        }

        public void LoadDirectories()
        {
            IEnumerable<DirectoryViewModel> initialDrives =
                Directory.GetDrives().Select(drive => new DirectoryViewModel(drive, this));

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

        public void LoadFiles()
        {
            throw new InvalidOperationException("Cannot load files from the root view model as it only contains drives");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}