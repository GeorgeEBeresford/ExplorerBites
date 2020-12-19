﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExplorerBites.Annotations;
using ExplorerBites.Models.FileSystem;
using ExplorerBites.Models.Interface;

namespace ExplorerBites.ViewModels.Interface
{
    public class FileTreeSelectorViewModel : IFileTreeSelectorViewModel
    {
        private readonly IFileTreeSelector FileTreeSelector = new FileTreeSelector();

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyCollection<IFileTree> SelectedFileTrees => FileTreeSelector.SelectedFileTrees;

        public void ClearSelection()
        {
            FileTreeSelector.ClearSelection();
            OnPropertyChanged(nameof(SelectedFileTrees));
        }

        /// <summary>
        ///     Selects one or more file trees and then calls the PropertyChanged for the <see cref="SelectedFileTrees" /> event
        /// </summary>
        /// <param name="fileTrees"></param>
        /// <returns></returns>
        public void SelectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            FileTreeSelector.SelectFileTrees(fileTrees);
            OnPropertyChanged(nameof(SelectedFileTrees));
        }

        /// <summary>
        ///     Selects one file tree and then calls the PropertyChanged for the <see cref="SelectedFileTrees" /> event
        /// </summary>
        /// <param name="fileTree"></param>
        /// <returns></returns>
        public void SelectFileTree(IFileTree fileTree)
        {
            FileTreeSelector.SelectFileTree(fileTree);
            OnPropertyChanged(nameof(SelectedFileTrees));
        }

        /// <summary>
        ///     Deselects one or more file trees and then calls the PropertyChanged for the <see cref="SelectedFileTrees" /> event
        /// </summary>
        /// <param name="fileTrees"></param>
        /// <returns></returns>
        public bool DeselectFileTrees(IEnumerable<IFileTree> fileTrees)
        {
            bool isSuccess = FileTreeSelector.DeselectFileTrees(fileTrees);
            OnPropertyChanged(nameof(SelectedFileTrees));

            return isSuccess;
        }

        /// <summary>
        ///     Deselects one file tree and then calls the PropertyChanged for the <see cref="SelectedFileTrees" /> event
        /// </summary>
        /// <param name="fileTree"></param>
        /// <returns></returns>
        public bool DeselectFileTree(IFileTree fileTree)
        {
            bool isSuccess = FileTreeSelector.DeselectFileTree(fileTree);
            OnPropertyChanged(nameof(SelectedFileTrees));

            return isSuccess;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}