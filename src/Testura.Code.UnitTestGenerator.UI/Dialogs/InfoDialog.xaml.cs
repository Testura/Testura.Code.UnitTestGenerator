using System.Windows;
using MahApps.Metro.Controls;

namespace Testura.Code.UnitTestGenerator.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : MetroWindow
    {
        public InfoDialog(string message)
        {
            InitializeComponent();
            InfoMessage.Content = message;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
