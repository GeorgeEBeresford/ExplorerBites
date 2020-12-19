using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Commands;
using ExplorerBites.Models.Interface;
using ExplorerBites.ViewModels.FileSystem;

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

        public IDirectoryViewModel SelectedDirectory => DirectorySelector.SelectedDirectory;
        public ICommand PreviousDirectoryCommand { get; }
        public ICommand ParentDirectoryCommand { get; }
        public ICommand UndoPreviousDirectoryCommand { get; }

        public void LoadParentDirectory()
        {
            DirectorySelector.LoadParentDirectory();

            OnPropertyChanged(nameof(SelectedDirectory));
        }

        public void LoadPreviousDirectory()
        {
            DirectorySelector.LoadPreviousDirectory();

            OnPropertyChanged(nameof(SelectedDirectory));
        }

        public void UndoPreviousDirectory()
        {
            DirectorySelector.UndoPreviousDirectory();

            OnPropertyChanged(nameof(SelectedDirectory));
        }

        public void SelectDirectory(IDirectoryViewModel directory)
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