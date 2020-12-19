using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExplorerBites.Annotations;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.ViewModels.FileSystem;
using ExplorerBites.ViewModels.Interface;

namespace ExplorerBites
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        //private Point? DragStartPosition;

        public MainWindow()
        {
            FileTreeSelector = new FileTreeSelectorViewModel();
            DirectorySelector = new DirectorySelectorViewModel();
            AddEventHandlers();

            InitializeComponent();
            InitialiseFolderHeirarchy();

            NavigationShortcuts.DataContext = this;
            UI.WindowState = WindowState.Maximized;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Allows file trees within the selected directory to also be selected
        /// </summary>
        public IFileTreeSelectorViewModel FileTreeSelector { get; }

        /// <summary>
        ///     Allows a directory to be selected as the current focus for any interactions
        /// </summary>
        public IDirectorySelectorViewModel DirectorySelector { get; }

        /// <summary>
        ///     The total number of file trees in the selected directory
        /// </summary>
        public string TotalItemsCount { get; private set; }

        /// <summary>
        ///     The total number of selected file trees in the selected directory
        /// </summary>
        public string TotalItemsSelected { get; private set; }

        /// <summary>
        ///     The top level directory. This contains any drives.
        /// </summary>
        public RootViewModel RootViewModel { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitialiseFolderHeirarchy()
        {
            RootViewModel = new RootViewModel();
            OnPropertyChanged(nameof(RootViewModel));

            RootViewModel.LoadDirectories();
            DirectorySelector.SelectDirectory(RootViewModel);
        }

        private void AddEventHandlers()
        {
            DirectorySelector.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(DirectorySelector.SelectedDirectory))
                {
                    SynchroniseSelectedDirectory(DirectorySelector.SelectedDirectory);
                }
            };

            FileTreeSelector.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(FileTreeSelector.SelectedFileTrees))
                {
                    SynchroniseSelectedFileTrees(FileTreeSelector.SelectedFileTrees);
                }
            };
        }

        /// <summary>
        ///     Updates any fields and properties which are dependent on the selected file trees
        /// </summary>
        /// <param name="fileTrees"></param>
        private void SynchroniseSelectedFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            // Greedy-load the enumerable so we aren't enumerating twice
            fileTrees = fileTrees.ToList();

            foreach (IFileTreeViewModel selectableFileTree in fileTrees.OfType<IFileTreeViewModel>())
            {
                selectableFileTree.SelectForListView();
            }

            // Update any statistic previews
            TotalItemsSelected = fileTrees.Any() ? $"#Items Selected: {fileTrees.Count()}" : "";
            OnPropertyChanged(nameof(TotalItemsSelected));
        }

        /// <summary>
        ///     Updates any fields and properties which are dependent on the selected directory
        /// </summary>
        private void SynchroniseSelectedDirectory(IDirectoryViewModel selectedDirectoryViewModel)
        {
            // Update any statistic previews
            TotalItemsCount = $"#Items: {selectedDirectoryViewModel.LoadedContents.Count}";
            OnPropertyChanged(nameof(TotalItemsCount));

            Title = selectedDirectoryViewModel.Directory.Path ?? Assembly.GetExecutingAssembly().GetName().Name;
        }

        private void OnSelectedDirectoryChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is IDirectoryViewModel selectedDirectoryViewModel)
            {
                DirectorySelector.SelectDirectory(selectedDirectoryViewModel);
            }
        }

        private void OnFileTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // If we're clicking on a file, try to open it as a file
            if (FileTreeNodeView.SelectedItem is IFile selectedFile)
            {
                selectedFile.TryOpen();
            }

            // If we're clicking on a directory, open it as a directory
            else if (FileTreeNodeView.SelectedItem is IDirectoryViewModel selectedDirectory)
            {
                DirectorySelector.SelectDirectory(selectedDirectory);
            }
        }

        private void OnFileTreeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<IFileTree> addedItems = e.AddedItems.OfType<IFileTree>();
            IEnumerable<IFileTree> removedItems = e.RemovedItems.OfType<IFileTree>();

            FileTreeSelector.SelectFileTrees(addedItems);
            FileTreeSelector.DeselectFileTrees(removedItems);
        }

        private void OnFileTreeLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileTreeSelector.ClearSelection();
        }

        //private void SelectViaDragPosition(object sender, MouseEventArgs e)
        //{
        //    if (!(sender is ItemsControl relativeItemsControl))
        //    {
        //        return;
        //    }

        //    Point currentDragPosition = e.GetPosition(relativeItemsControl);

        //    foreach (object child in relativeItemsControl.Items)
        //    {
        //        if (relativeItemsControl.ItemContainerGenerator.ContainerFromItem(child) is Control childControl)
        //        {
        //            Point relativePosition = e.GetPosition(childControl);

        //            // If the cursor is below or on top of the element then we must have included it in the selection
        //            bool isElementIncluded =
        //                currentDragPosition.Y > DragStartPosition.Value.Y && relativePosition.Y >= currentDragPosition.Y ||
        //                currentDragPosition.Y < DragStartPosition.Value.Y && relativePosition.Y <= currentDragPosition.Y ||
        //                currentDragPosition.X > DragStartPosition.Value.X && relativePosition.X >= currentDragPosition.X ||
        //                currentDragPosition.X < DragStartPosition.Value.Y && relativePosition.X <= currentDragPosition.X;

        //            if (isElementIncluded)
        //            {
        //                FileTreeSelector.SelectFileTree((IFileTree) child);
        //            }
        //        }
        //    }
        //}

        //private void StopSelectingViaDrag(object sender, MouseButtonEventArgs e)
        //{
        //    if (!(sender is ItemsControl relativeItemsControl))
        //    {
        //        return;
        //    }

        //    DragStartPosition = null;

        //    relativeItemsControl.PreviewMouseMove -= SelectViaDragPosition;
        //    relativeItemsControl.PreviewMouseUp -= StopSelectingViaDrag;
        //}

        //private void StartSelectingViaDrag(object sender, MouseButtonEventArgs e)
        //{
        //    if (!(sender is ItemsControl relativeItemsControl))
        //    {
        //        return;
        //    }

        //    FileTreeSelector.ClearSelection();

        //    DragStartPosition = e.GetPosition(relativeItemsControl);

        //    relativeItemsControl.PreviewMouseMove += SelectViaDragPosition;
        //    relativeItemsControl.PreviewMouseUp += StopSelectingViaDrag;
        //}
    }
}