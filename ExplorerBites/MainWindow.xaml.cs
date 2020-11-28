using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Commands;
using ExplorerBites.Models;
using ExplorerBites.Models.ViewModels;

namespace ExplorerBites
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            History = new Stack<IDirectoryViewModel>();
            HistoryRollback = new Stack<IDirectoryViewModel>();
            SelectedFileTrees = new List<IFileTree>();

            PreviousDirectoryCommand = new RelayCommand(LoadPreviousDirectory);
            ParentDirectoryCommand = new RelayCommand(LoadParentDirectory);
            UndoPreviousDirectoryCommand = new RelayCommand(UndoPreviousDirectory);

            InitializeComponent();
            InitialiseFolderHeirarchy();

            NavigationShortcuts.DataContext = this;
            UI.WindowState = WindowState.Maximized;
        }

        private void InitialiseFolderHeirarchy()
        {
            RootViewModel = new RootViewModel();
            OnPropertyChanged(nameof(RootViewModel));

            RootViewModel.LoadDirectories();
            SelectDirectory(RootViewModel);
        }

        /// <summary>
        ///  A stack of directories which the user has hit the "back" button while on. If the page is changed without executing the PreviousDirectoryCommand, this will be cleared.
        /// </summary>
        private Stack<IDirectoryViewModel> HistoryRollback { get; }

        /// <summary>
        /// A stack of directories which the user has selected another page while on.
        /// </summary>
        private Stack<IDirectoryViewModel> History { get; }
        public RootViewModel RootViewModel { get; private set; }
        public IDirectoryViewModel SelectedDirectory { get; private set; }
        public ICollection<IFileTree> SelectedFileTrees { get; }

        public ICommand PreviousDirectoryCommand { get; }
        public ICommand ParentDirectoryCommand { get; }
        public ICommand UndoPreviousDirectoryCommand { get; }

        private void LoadParentDirectory()
        {
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory != null)
            {
                if (currentDirectory.Parent is IDirectoryViewModel parentViewModel)
                {
                    SelectDirectory(parentViewModel);
                }

                History.Push(currentDirectory);
                HistoryRollback.Clear();
            }
        }

        private void LoadPreviousDirectory()
        {
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory != null && History.Any())
            {
                IDirectoryViewModel historicalDirectory = History.Pop();

                if (historicalDirectory != null)
                {
                    SelectDirectory(historicalDirectory);
                }

                HistoryRollback.Push(currentDirectory);
            }
        }

        private void UndoPreviousDirectory()
        {
            IDirectoryViewModel currentDirectory = SelectedDirectory;

            if (currentDirectory != null && HistoryRollback.Any())
            {
                IDirectoryViewModel historicalDirectory = HistoryRollback.Pop();

                if (historicalDirectory != null)
                {
                    SelectDirectory(historicalDirectory);
                }

                History.Push(currentDirectory);
            }
        }

        private void SelectDirectory(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is IDirectoryViewModel selectedDirectory)
            {
                SelectDirectory(selectedDirectory);
            }
        }

        private void SelectDirectory(IDirectoryViewModel directory)
        {
            SelectedDirectory = directory;
            SelectedDirectory.LoadContents();
            OnPropertyChanged(nameof(SelectedDirectory));
            OnPropertyChanged(nameof(TotalItemsCount));

            // If we aren't currently at the root level and the last viewed item isn't the last item in our history, add the directory to our history
            if (directory.Parent is IDirectoryViewModel parentViewModel && (!History.Any() || !History.Peek().Equals(parentViewModel)))
            {
                History.Push(parentViewModel);
            }

            HistoryRollback.Clear();

            // New folder context, new set of selected file trees
            SelectedFileTrees.Clear();
            OnPropertyChanged(nameof(TotalItemsSelected));

            Title = directory.Path ?? Assembly.GetExecutingAssembly().GetName().Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenItem(object sender, MouseButtonEventArgs e)
        {
            IDirectoryViewModel firstSelectedDirectory = SelectedFileTrees
                .OfType<IDirectoryViewModel>()
                .FirstOrDefault();

            if (firstSelectedDirectory != null)
            {
                SelectDirectory(firstSelectedDirectory);
                firstSelectedDirectory.IsExpanded = true;
            }

            IEnumerable<IFile> selectedFiles = SelectedFileTrees
                .OfType<IFile>();

            foreach (IFile file in selectedFiles)
            {
                file.TryOpen();
            }
        }

        private void SelectFileTreeNodes(object sender, SelectionChangedEventArgs e)
        {
            foreach (IFileTree fileTree in e.AddedItems)
            {
                SelectedFileTrees.Add(fileTree);
            }

            foreach (IFileTree fileTree in e.RemovedItems)
            {
                SelectedFileTrees.Remove(fileTree);
            }

            OnPropertyChanged(nameof(TotalItemsSelected));
        }

        public string TotalItemsCount => $"#Items: {SelectedDirectory?.ObservableContents.Count ?? 0}";

        public string TotalItemsSelected => SelectedFileTrees.Any()
            ? $"#Items Selected: {SelectedFileTrees.Count}"
            : "";

        private void DeselectFileTrees(object sender, MouseButtonEventArgs e)
        {
            FileTreeNodeView.SelectedItem = null;
        }
    }
}