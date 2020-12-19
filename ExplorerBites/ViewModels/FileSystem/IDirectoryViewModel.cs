using System.ComponentModel;
using System.Windows.Input;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public interface IDirectoryViewModel : IDirectory, INotifyPropertyChanged
    {
        ICommand LoadDirectoriesCommand { get; }
        ICommand LoadContentsCommand { get; }
        bool IsExpanded { get; set; }
    }
}