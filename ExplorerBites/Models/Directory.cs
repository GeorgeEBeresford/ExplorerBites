using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplorerBites.Models
{
    public class Directory : IDirectory
    {
        /// <summary>
        ///     Creates a new reference to a directory at a specified path
        /// </summary>
        /// <param name="path">
        ///     The absolute path to a directory
        /// </param>
        /// <exception cref="DirectoryNotFoundException">A directory could not be found at the specified path</exception>
        public Directory(string path)
        {
            DirectoryInfo = new DirectoryInfo(path);

            LoadedDirectories = new List<IDirectory>();
            LoadedFiles = new List<IFile>();
        }

        public IFileTree Parent => new Directory(DirectoryInfo.Parent?.FullName);
        public bool IsDirectory => true;
        public string Name => DirectoryInfo.Name;
        public string Path => DirectoryInfo.FullName;

        public List<IFileTree> LoadedContents => (LoadedDirectories ?? new List<IDirectory>(0))
            .Cast<IFileTree>()
            .Union(LoadedFiles ?? new List<IFile>(0))
            .ToList();

        public List<IDirectory> LoadedDirectories { get; }
        public List<IFile> LoadedFiles { get; }

        private DirectoryInfo DirectoryInfo { get; }

        public static Directory[] GetDrives()
        {
            IEnumerable<Directory> directories = DriveInfo
                .GetDrives()
                .Select(drive => new Directory(drive.Name));

            return directories.ToArray();
        }

        public bool Rename(string name)
        {
            throw new NotImplementedException();
        }

        public bool Move(string path)
        {
            throw new NotImplementedException();
        }

        public bool Move(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public void LoadContents()
        {
            LoadDirectories();
            LoadFiles();
        }

        public void LoadDirectories()
        {
            if (!DirectoryInfo.Exists)
            {
                return;
            }

            LoadedDirectories.Clear();

            try
            {
                IEnumerable<Directory> directories = DirectoryInfo
                    .GetDirectories()
                    .Select(directory => new Directory(directory.FullName));

                LoadedDirectories.AddRange(directories);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public void LoadFiles()
        {
            if (!DirectoryInfo.Exists)
            {
                return;
            }

            LoadedFiles.Clear();

            try
            {
                IEnumerable<File> files = DirectoryInfo
                    .GetFiles()
                    .Select(file => new File(file.FullName, this));

                LoadedFiles.AddRange(files);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}