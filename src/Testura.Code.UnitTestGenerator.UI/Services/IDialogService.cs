namespace Testura.Code.UnitTestGenerator.UI.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Show an info dialog
        /// </summary>
        /// <param name="message">Info message to show</param>
        void ShowInfoDialog(string message);

        /// <summary>
        /// Show the about dialog
        /// </summary>
        void ShowAboutDialog();
    }
}
