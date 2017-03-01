using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Testura.Code.UnitTestGenerator.UI.Services
{
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// Show a pick a file dialog
        /// </summary>
        /// <param name="fileExtensionsFilter">We only show files with these extensions</param>
        /// <returns>Path to file, null if canceled</returns>
        public string ShowPickFileDialog(string fileExtensionsFilter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter += fileExtensionsFilter;
            if (dialog.ShowDialog() ?? true)
            {
                return dialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Show pick a directory dialog
        /// </summary>
        /// <returns>Path to the directory, null if canceled</returns>
        public string ShowPickDirectoryDialog()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            return null;
        }
    }
}
