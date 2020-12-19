using System.ComponentModel;
using ExplorerBites.Models.Interface;

namespace ExplorerBites.ViewModels.Interface
{
    public interface IFileTreeSelectorViewModel : IFileTreeSelector, INotifyPropertyChanged
    {
    }
}