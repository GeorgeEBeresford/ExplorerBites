namespace ExplorerBites.Models
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
    }
}