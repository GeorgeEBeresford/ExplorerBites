namespace ExplorerBites.Models.FileSystem
{
    /// <summary>
    ///     Represents a class that allows files to be manipulated in a non-static context
    /// </summary>
    public interface IFile : IFileTree
    {
        /// <summary>
        ///     A collection of bytes that make up the file
        /// </summary>
        /// <returns></returns>
        byte[] GetContents();

        /// <summary>
        /// Attempts to open the file using the default handler
        /// </summary>
        /// <returns></returns>
        bool TryOpen();

        /// <summary>
        /// The number of bytes in the file
        /// </summary>
        long Length { get; }
    }
}