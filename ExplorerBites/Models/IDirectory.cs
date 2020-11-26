using System.Collections.Generic;

namespace ExplorerBites.Models
{
    public interface IDirectory : IFileTree
    {
        List<IFileTree> LoadedContents { get; }
        List<IDirectory> LoadedDirectories { get; }
        List<IFile> LoadedFiles { get; }

        void LoadContents();
        void LoadDirectories();
        void LoadFiles();

        bool HasChildren { get; }
    }
}