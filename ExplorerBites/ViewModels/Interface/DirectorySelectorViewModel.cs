using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Commands;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.Models.Interface;

namespace ExplorerBites.ViewModels.Interface
{
    public class DirectorySelectorViewModel : IDirectorySelectorViewModel
    {
        private readonly IDirectorySelector DirectorySelector = new DirectorySelector();

        public DirectorySelectorViewModel()
        {
            PreviousDirectoryCommand = new RelayCommand(LoadPreviousDirectory);
            ParentDirectoryCommand = new RelayCommand(LoadParentDirectory);
            UndoPreviousDirectoryCommand = new RelayCommand(UndoPreviousDirectory);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IDirectory SelectedDirectory => DirectorySelector.SelectedDirectory;
        public ICommand PreviousDirectoryCommand { get; }
        public ICommand ParentDirectoryCommand { get; }
        public ICommand UndoPreviousDirectoryCommand { get; }

        public void LoadParentDirectory()
        {
            DirectorySelector.LoadParentDirectory();
        }

        public void LoadPreviousDirectory()
        {
            DirectorySelector.LoadPreviousDirectory();
        }

        public void UndoPreviousDirectory()
        {
            DirectorySelector.UndoPreviousDirectory();
        }

        public void SelectDirectory(IDirectory directory)
        {
            DirectorySelector.SelectDirectory(directory);

            OnPropertyChanged(nameof(SelectedDirectory));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}