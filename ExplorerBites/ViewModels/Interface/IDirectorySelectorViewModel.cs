﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using ExplorerBites.Models.Interface;
using ExplorerBites.ViewModels.FileSystem;

namespace ExplorerBites.ViewModels.Interface
{
    public interface IDirectorySelectorViewModel : IDirectorySelector, INotifyPropertyChanged
    {
        ICommand PreviousDirectoryCommand { get; }
        ICommand ParentDirectoryCommand { get; }
        ICommand UndoPreviousDirectoryCommand { get; }
    }
}