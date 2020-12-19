using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplorerBites.Models.FileSystem
{
    /// <summary>
    ///     Represents a collection of drives that make up the root of the file system
    /// </summary>
    public class Root : IDirectory
    {
        private readonly IReadOnlyCollection<DriveInfo> Drives = DriveInfo.GetDrives();

        public IDirectory Parent => null;
        public bool IsDirectory => true;
        public string FileTreeType => "Root";
        public string Name => "Root";
        public string Path => "Root";

        public bool IsValid => true;
        public DateTime LastModifiedOn => DateTime.MinValue;
        public string SizeDescription => "0B";
        public string KiBDescription => "0B";

        public bool HasChildren => Drives.Any();

        public bool Rename(string name)
        {
            throw new InvalidOperationException("Cannot rename the root");
        }

        public bool Move(string path)
        {
            throw new InvalidOperationException("Cannot move the root");
        }

        public bool Move(IDirectory directory)
        {
            throw new InvalidOperationException("Cannot move the root");
        }

        public List<IFileTree> GetContents()
        {
            return GetDirectories()
                .OfType<IFileTree>()
                .ToList();
        }

        public List<IDirectory> GetDirectories()
        {
            return Drives
                .Select(drive => (IDirectory) new Directory(drive.Name))
                .ToList();
        }

        public List<IFile> GetFiles()
        {
            throw new InvalidOperationException("Cannot get files from the root");
        }
    }
}