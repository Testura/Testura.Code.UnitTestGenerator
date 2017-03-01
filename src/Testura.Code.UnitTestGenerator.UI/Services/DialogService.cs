
using Testura.Code.UnitTestGenerator.UI.Dialogs;

namespace Testura.Code.UnitTestGenerator.UI.Services
{
    public class DialogService : IDialogService
    {

        /// <summary>
        /// Show an info dialog
        /// </summary>
        /// <param name="message">Info message to show</param>
        public void ShowInfoDialog(string message)
        {
            var dialog = new InfoDialog(message);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Show the about dialog
        /// </summary>
        public void ShowAboutDialog()
        {

        }
    }
}
