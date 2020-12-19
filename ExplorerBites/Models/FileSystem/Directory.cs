using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplorerBites.Models.FileSystem
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
            IsValid = DirectoryInfo.Exists;
        }

        public IDirectory Parent => new Directory(DirectoryInfo.Parent?.FullName);
        public bool IsDirectory => true;
        public string FileTreeType => "File directory";
        public string Name => DirectoryInfo.Name;
        public string Path => DirectoryInfo.FullName;
        public DateTime LastModifiedOn => DirectoryInfo.LastWriteTimeUtc.ToLocalTime();
        public string SizeDescription => "";
        public string KiBDescription => "";

        private DirectoryInfo DirectoryInfo { get; }

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

        public bool IsValid { get; private set; }

        public List<IFileTree> GetContents()
        {
            List<IFileTree> contents = new List<IFileTree>();

            contents.AddRange(GetDirectories());
            contents.AddRange(GetFiles());

            return contents;
        }

        public List<IDirectory> GetDirectories()
        {
            if (!IsValid)
            {
                return new List<IDirectory>();
            }

            try
            {
                List<IDirectory> directories = DirectoryInfo
                    .GetDirectories()
                    .Select(file => (IDirectory)new Directory(file.FullName))
                    .ToList();

                return directories;
            }
            catch (UnauthorizedAccessException)
            {
                IsValid = false;
                return new List<IDirectory>();
            }
            catch (DirectoryNotFoundException)
            {
                IsValid = false;
                return new List<IDirectory>();
            }
        }

        public List<IFile> GetFiles()
        {
            if (!IsValid)
            {
                return new List<IFile>();
            }

            try
            {
                List<IFile> files = DirectoryInfo
                    .GetFiles()
                    .Select(file => (IFile) new File(file.FullName, this))
                    .ToList();

                return files;
            }
            catch (UnauthorizedAccessException)
            {
                IsValid = false;
                return new List<IFile>();
            }
            catch (DirectoryNotFoundException)
            {
                IsValid = false;
                return new List<IFile>();
            }
        }

        public bool HasChildren
        {
            get
            {
                if (!IsValid)
                {
                    return false;
                }

                bool hasChildren;

                try
                {
                    hasChildren = DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                        .Any(subDirectory => subDirectory.Exists);
                }
                catch (UnauthorizedAccessException)
                {
                    IsValid = false;
                    return false;
                }
                catch (DirectoryNotFoundException)
                {
                    IsValid = false;
                    return false;
                }

                return hasChildren;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }}