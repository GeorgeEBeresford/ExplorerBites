using System;
using System.IO;
using System.Linq;
using WindowsWrapper.FileSystem;

namespace ExplorerBites.Models.FileSystem
{
    public class File : IFile
    {
        public File(string fileName, IDirectory parent)
        {
            FileInfo = new FileInfo(System.IO.Path.Combine(parent.Path, fileName));
            WrappedFile = new WindowsFile(FileInfo);
            Parent = parent;
            IsValid = FileInfo.Exists;
        }

        private FileInfo FileInfo { get; }

        public IDirectory Parent { get; }
        public string FileTreeType => $"{Extension.ToUpper()} File";
        public string Name => FileInfo.Name;
        public string Path => FileInfo.FullName;
        public string Extension => FileInfo.Name.Split('.').LastOrDefault();

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
                int status = fileStream.Read(contentBuffer, 0, (int)FileInfo.Length);
                return contentBuffer;
            }
        }

        public bool TryOpen()
        {
            bool isSuccess = WrappedFile.TryExecute();
            return isSuccess;
        }

        public DateTime LastModifiedOn => FileInfo.LastWriteTimeUtc.ToLocalTime();
        public long Length => FileInfo.Length;

        public override string ToString()
        {
            return Name;
        }

        private WindowsWrapper.FileSystem.IFile WrappedFile { get; }
    }
}