using System;

namespace ExplorerBites.Models
{
    public interface IFileTree
    {
        /// <summary>
        ///     The parent of this tree node
        /// </summary>
        IFileTree Parent { get; }

        /// <summary>
        ///     Whether the tree node is a directory
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// A description of the kind of file tree we're looking at based off either the extension or whether it's
        /// a directory
        /// </summary>
        string FileTreeType { get; }

        /// <summary>
        ///     The name of the tree node
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The absolute path to the tree node
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     The new name of the tree node
        /// </summary>
        /// <param name="name"></param>
        bool Rename(string name);

        /// <summary>
        ///     Moves the tree node to a different path
        /// </summary>
        /// <param name="path">
        ///     The path of the directory which the tree node should be moved to
        /// </param>
        bool Move(string path);

        /// <summary>
        ///     Moves the tree node to a different directory
        /// </summary>
        /// <param name="directory">
        ///     The directory which the tree node should be moved to
        /// </param>
        /// <returns></returns>
        bool Move(IDirectory directory);

        /// <summary>
        /// Whether the file tree exists and is a valid node (e.g. hasn't been corrupted)
        /// </summary>
        bool IsValid { get; }
        
        /// <summary>
        /// The last date the file was written to, expressed as a local time
        /// </summary>
        DateTime LastModifiedOn { get; }

        /// <summary>
        /// A description of the number of bytes in the file tree node
        /// </summary>
        string SizeDescription { get; }

        /// <summary>
        /// The number of Kibibytes in the file tree node
        /// </summary>
        string KiBDescription { get; }
    }
}