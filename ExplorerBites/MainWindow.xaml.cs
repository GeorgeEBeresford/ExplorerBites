using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExplorerBites.Annotations;
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
            InitializeComponent();
            SelectedFileTrees = new List<IFileTree>();
        }

        public RootViewModel RootViewModel => new RootViewModel();
        public DirectoryViewModel SelectedDirectory { get; private set; }
        public ICollection<IFileTree> SelectedFileTrees { get; }

        private void SelectDirectory(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DirectoryViewModel selectedDirectory = e.NewValue as DirectoryViewModel;

            if (selectedDirectory != null)
            {
                SelectDirectory(selectedDirectory);
            }
        }

        private void SelectDirectory(DirectoryViewModel directory)
        {
            SelectedDirectory = directory;
            SelectedDirectory.LoadContents();
            OnPropertyChanged(nameof(SelectedDirectory));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenItem(object sender, MouseButtonEventArgs e)
        {
            DirectoryViewModel firstSelectedDirectory = SelectedFileTrees
                .OfType<DirectoryViewModel>()
                .FirstOrDefault();

            if (firstSelectedDirectory != null)
            {
                SelectDirectory(firstSelectedDirectory);
            }
        }

        private void SelectFileTreeNode(object sender, SelectionChangedEventArgs e)
        {
            foreach (IFileTree fileTree in e.AddedItems)
            {
                SelectedFileTrees.Add(fileTree);
            }

            foreach (IFileTree fileTree in e.RemovedItems)
            {
                SelectedFileTrees.Remove(fileTree);
            }
        }
    }
}