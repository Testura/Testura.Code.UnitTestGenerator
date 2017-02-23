using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Testura.Code.UnitTestGenerator.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="path">Create the file here</param>
        public void CreateFile(string path)
        {
            File.Create(path);
        }

        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="path">Create the directory here</param>
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Copy files from one directory to another
        /// </summary>
        /// <param name="fromDirectoryPath">Directory to copy from</param>
        /// <param name="toDirectoryPath">Directory to copy to</param>
        /// <param name="fileExtensionfilter">Copy only files that match any extension in the filter</param>
        /// <returns>The running task</returns>
        public Task CopyFilesFromDirectoryAsync(string fromDirectoryPath, string toDirectoryPath, string[] fileExtensionfilter = null)
        {
            return Task.Run(() =>
            {
                var files = Directory.GetFiles(fromDirectoryPath);
                foreach (var file in files)
                {
                    if (fileExtensionfilter != null)
                    {
                        if (!fileExtensionfilter.Any(f => file.EndsWith(f)))
                        {
                            continue;
                        }
                    }

                    File.Copy(file, Path.Combine(toDirectoryPath, Path.GetFileName(file)), true);
                }
            });
        }

        /// <summary>
        /// Copy a file to a directory
        /// </summary>
        /// <param name="filePath">File to copy</param>
        /// <param name="toDirectoryPath">Directory to copy to</param>
        /// <returns>The runing task</returns>
        public Task CopyFileToDirectoryAsync(string filePath, string toDirectoryPath)
        {
            return Task.Run(() =>
            {
                File.Copy(filePath, Path.Combine(toDirectoryPath, Path.GetFileName(filePath)), true);
            });
        }

        /// <summary>
        /// Return the name of the files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <returns>Path of the files in the directory</returns>
        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// Deletes the specific file
        /// </summary>
        /// <param name="path">Path to the file to delete</param>
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Determines whetether the specified file exists.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>True if file exists, otherwise false</returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <returns>True if the directory exists, false otherwise</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Opens a text file, read all lines of the file, and then close the file.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Lines in the file</returns>
        public string[] ReadAllLinesFromFile(string path)
        {
            // We won't use File.ReadAllLinesFromFile as it can't read from files that are already open.
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    List<string> file = new List<string>();
                    while (!streamReader.EndOfStream)
                    {
                        file.Add(streamReader.ReadLine());
                    }

                    return file.ToArray();
                }
            }
        }

        /// <summary>
        /// Open a text file, write all lines to file, and then close the file.
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <param name="textLines">Lines to write to the file</param>
        public void WriteTextToFile(string path, string[] textLines)
        {
            using (var file = new StreamWriter(path))
            {
                foreach (string line in textLines)
                {
                    file.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// From a list of paths get all paths that match a list of filters.
        /// If the path is a directory we loop through it and check it's content.
        /// </summary>
        /// <param name="paths">Paths to look at</param>
        /// <param name="filters">A list of file extensions. The path need to match any of the extensions in the filter</param>
        /// <returns>All paths that match any of the extensions in the filter array</returns>
        public IList<string> GetPathToFiles(IList<string> paths, string[] filters)
        {
            var matchingPaths = new List<string>();
            foreach (var path in paths)
            {
                var attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    var files = GetFiles(path);
                    foreach (var file in files)
                    {
                        if (filters.Any(filter => path.EndsWith(filter)))
                        {
                            if (!matchingPaths.Contains(file))
                            {
                                matchingPaths.Add(file);
                            }
                        }
                    }
                }
                else
                {
                    if (filters.Any(filter => path.EndsWith(filter)))
                    {
                        if (!matchingPaths.Contains(path))
                        {
                            matchingPaths.Add(path);
                        }
                    }
                }
            }

            return matchingPaths;
        }

        /// <summary>
        /// Delete all files in a directory
        /// </summary>
        /// <param name="path">Path to directory</param>
        public void DeleteFiles(string path)
        {
            var files = GetFiles(path);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
