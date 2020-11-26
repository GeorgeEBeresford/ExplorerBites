using System;
using System.IO;

namespace ExplorerBites.Models
{
    public class File : IFile
    {
        public File(string path, IDirectory parent)
        {
            FileInfo = new FileInfo(path);
            Parent = parent;
        }

        private FileInfo FileInfo { get; }

        public IFileTree Parent { get; }
        public bool IsDirectory => false;
        public string Name => FileInfo.Name;
        public string Path => FileInfo.FullName;
        public string Extension => FileInfo.Extension;

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

        public byte[] GetContents()
        {
            using (FileStream fileStream = FileInfo.OpenRead())
            {
                byte[] contentBuffer = new byte[FileInfo.Length];
                int status = fileStream.Read(contentBuffer, 0, (int) FileInfo.Length);
                return contentBuffer;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}