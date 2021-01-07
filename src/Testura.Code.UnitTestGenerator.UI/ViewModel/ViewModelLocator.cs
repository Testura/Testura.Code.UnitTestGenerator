using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Testura.Code.UnitTestGenerator.UI.Services;

namespace Testura.Code.UnitTestGenerator.UI.ViewModel
{
	public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default.);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}