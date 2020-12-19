using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExplorerBites.Annotations;
using ExplorerBites.Models.FileSystem;

namespace ExplorerBites.ViewModels.FileSystem
{
    public class FileViewModel : IFileViewModel
    {
        public FileViewModel(IFile file)
        {
            File = file;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IDirectory Parent => File.Parent;
        public bool IsDirectory => File.IsDirectory;
        public string FileTreeType => File.FileTreeType;
        public string Name => File.Name;
        public string Path => File.Path;
        public bool IsValid => File.IsValid;
        public DateTime LastModifiedOn => File.LastModifiedOn;
        public string SizeDescription => File.SizeDescription;
        public string KiBDescription => File.KiBDescription;

        public long Length => File.Length;
        public bool IsDirectorySelected { get; set; }

        private IFile File { get; }

        public void SelectDirectory()
        {
            IsDirectorySelected = true;
            OnPropertyChanged(nameof(IsDirectorySelected));
        }

        public void DeselectDirectory()
        {
            IsDirectorySelected = false;
            OnPropertyChanged(nameof(IsDirectorySelected));
        }

        public bool Rename(string name)
        {
            return File.Rename(name);
        }

        public bool Move(string path)
        {
            return File.Move(path);
        }

        public bool Move(IDirectory directory)
        {
            return File.Move(directory);
        }

        public byte[] GetContents()
        {
            return File.GetContents();
        }

        public bool TryOpen()
        {
            return File.TryOpen();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}