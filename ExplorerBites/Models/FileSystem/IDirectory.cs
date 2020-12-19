using System.Collections.Generic;

namespace ExplorerBites.Models.FileSystem
{
    public interface IDirectory : IFileTree
    {
        List<IFileTree> GetContents();
        List<IDirectory> GetDirectories();
        List<IFile> GetFiles();

        bool HasChildren { get; }
    }
}