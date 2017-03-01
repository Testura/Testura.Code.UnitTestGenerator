namespace Testura.Code.UnitTestGenerator.UI.Services
{
    public interface IFileDialogService
    {
        /// <summary>
        /// Show a pick a file dialog
        /// </summary>
        /// <param name="fileExtensionsFilter">We only show files with these extensions</param>
        /// <returns>Path to file, null if canceled</returns>
        string ShowPickFileDialog(string fileExtensionsFilter);

        /// <summary>
        /// Show pick a directory dialog
        /// </summary>
        /// <returns>Path to the directory, null if canceled</returns>
        string ShowPickDirectoryDialog();
    }
}
