using System.Collections.Generic;
using System.Threading.Tasks;

namespace Testura.Code.UnitTests.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="path">Create the file here</param>
        void CreateFile(string path);

        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="path">Create the directory here</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Copy files from one directory to another
        /// </summary>
        /// <param name="fromDirectoryPath">Directory to copy from</param>
        /// <param name="toDirectoryPath">Directory to copy to</param>
        /// <param name="fileExtensionfilter">Copy only files that match any extension in the filter</param>
        /// <returns>The running task</returns>
        Task CopyFilesFromDirectoryAsync(string fromDirectoryPath, string toDirectoryPath, string[] fileExtensionfilter = null);

        /// <summary>
        /// Copy a file to a directory
        /// </summary>
        /// <param name="filePath">File to copy</param>
        /// <param name="toDirectoryPath">Directory to copy to</param>
        /// <returns>The runing task</returns>
        Task CopyFileToDirectoryAsync(string filePath, string toDirectoryPath);

        /// <summary>
        /// Return the name of the files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <returns>Path of the files in the directory</returns>
        string[] GetFiles(string path);

        /// <summary>
        /// Deletes the specific file
        /// </summary>
        /// <param name="path">Path to the file to delete</param>
        void DeleteFile(string path);

        /// <summary>
        /// Determines whetether the specified file exists.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>True if file exists, otherwise false</returns>
        bool FileExists(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <returns>True if the directory exists, false otherwise</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Read all lines from a file
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Lines in the file</returns>
        string[] ReadAllLinesFromFile(string path);

        /// <summary>
        /// Open a text file, write all lines to file, and then close the file.
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <param name="textLines">Lines to write to the file</param>
        void WriteTextToFile(string path, string[] textLines);

        /// <summary>
        /// From a list of paths get all paths that match a specific data type.
        /// If the path is a directory we loop through it and check it's content.
        /// </summary>
        /// <param name="paths">Paths to look at</param>
        /// <param name="filters">A list of file extensions. The path need to match any of the extensions in the filter</param>
        /// <returns>All paths that match any of the extensions in the filter array</returns>
        IList<string> GetPathToFiles(IList<string> paths, string[] filters);

        /// <summary>
        /// Delete all files in a directory
        /// </summary>
        /// <param name="path">Path to directory</param>
        void DeleteFiles(string path);
    }
}
