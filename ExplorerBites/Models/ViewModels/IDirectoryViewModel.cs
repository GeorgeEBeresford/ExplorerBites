using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ExplorerBites.Models.ViewModels
{
    public interface IDirectoryViewModel : IFileTree
    {
        ObservableCollection<IFile> ObservableFiles { get; }
        ObservableCollection<IDirectory> ObservableDirectories { get; }
        ObservableCollection<IFileTree> ObservableContents { get; }

        void LoadFiles();
        void LoadDirectories();
        void LoadContents();
        bool IsExpanded { get; set; }
    }
}
