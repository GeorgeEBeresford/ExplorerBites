using System;
using System.IO;
using WindowsWrapper.FileSystem;

namespace ExplorerBites.Models.FileSystem
{
    public class File : IFile
    {
        public File(string path, IDirectory parent)
        {
            FileInfo = new FileInfo(path);
            WrappedFile = new WindowsFile(FileInfo);
            Parent = parent;
            IsValid = FileInfo.Exists;
        }

        private FileInfo FileInfo { get; }

        public IFileTree Parent { get; }
        public bool IsDirectory => false;
        public string FileTreeType => $"{Extension.ToUpper()} File";
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
        public string SizeDescription
        {
            get
            {
                long length = Length;
                int power = 0;

                // I'm sure there's a better way to find KiB, MiB, GiB, etc... I'm tired.
                while (length > 1024)
                {
                    length /= 1024;
                    power += 1;
                }

                switch (power)
                {
                    case 1:
                        return $"{length} KiB";
                    case 2:
                        return $"{length} MiB";
                    case 3:
                        return $"{length} GiB";
                    case 4:
                        // Maximum file size is 256 Terabytes. We don't need to go any higher than this
                        return $"{length} TiB";
                    default:
                        return $"{Length} bytes";

                }
            }
        }

        public string KiBDescription => $"{(Length / 1024):N0} KB";

        public override string ToString()
        {
            return Name;
        }

        private WindowsWrapper.FileSystem.IFile WrappedFile { get; }
    }
}