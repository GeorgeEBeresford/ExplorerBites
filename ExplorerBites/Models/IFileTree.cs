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
    }
}