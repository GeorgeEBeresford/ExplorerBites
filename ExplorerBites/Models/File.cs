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
            IsValid = FileInfo.Exists;
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

        public bool IsValid { get; private set; }

        public byte[] GetContents()
        {
            if (!IsValid)
            {
                return new byte[0];
            }

            FileStream fileStream;

            try
            {
                fileStream = FileInfo.OpenRead();
            }
            catch (UnauthorizedAccessException)
            {
                IsValid = false;
                return new byte[0];
            }
            catch (FileNotFoundException)
            {
                IsValid = false;
                return new byte[0];
            }

            using (fileStream)
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