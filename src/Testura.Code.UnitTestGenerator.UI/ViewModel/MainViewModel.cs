using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using Testura.Code.UnitTestGenerator.UI.Services;
using Testura.Code.UnitTestGenerator.Util;

namespace Testura.Code.UnitTestGenerator.UI.ViewModel
{
	//[ImplementPropertyChanged]
	[AddINotifyPropertyChangedInterface]
    public class MainViewModel : ViewModelBase
    {
        private IFileDialogService _fileDialogService;
        private IDialogService _dialogService;

        public MainViewModel(IFileDialogService fileDialogService, IDialogService dialogService)
        {
            UseMsTest = true;
            UseMoq = true;
            _fileDialogService = fileDialogService;
            _dialogService = dialogService;
            DllFileDialogCommand = new RelayCommand(OpenDllFileDialog);
            OutputDirectoryDialogCommand = new RelayCommand(OpenDirectoryDialog);
            GenerateCodeCommand = new RelayCommand(GenerateCode);
        }

        public bool UseMsTest { get; set; }

        public bool UseNUnit { get; set; }

        public bool UseMoq { get; set; }

        public string DllPath { get; set; }

        public string OutputDirectory { get; set; }

        public bool IsGenerating { get; set; }

        public RelayCommand DllFileDialogCommand { get; set; }

        public RelayCommand OutputDirectoryDialogCommand { get; set; }

        public RelayCommand GenerateCodeCommand { get; set; }

        public void OpenDllFileDialog()
        {
            var path = _fileDialogService.ShowPickFileDialog("Dll files (.dll)|*.dll|Exe files (.exe)|*.exe");
            if (!string.IsNullOrEmpty(path))
            {
                DllPath = path;
            }
        }

        public void OpenDirectoryDialog()
        {
            var path = _fileDialogService.ShowPickDirectoryDialog();
            if (!string.IsNullOrEmpty(path))
            {
                OutputDirectory = path;
            }
        }

        public async void GenerateCode()
        {
            var unitTestGenerator = new UnitTestGenerator();
            try
            {
                IsGenerating = true;
                await unitTestGenerator.GenerateUnitTestsAsync(DllPath,
                    UseMsTest ? TestFrameworks.MsTest : TestFrameworks.NUnit, MockFrameworks.Moq, OutputDirectory);
            }
            catch (Exception ex)
            {
                _dialogService.ShowInfoDialog($"Failed to generate unit tests: {ex.Message}");
                return;
            }
            finally
            {
                IsGenerating = false;
            }
            
            _dialogService.ShowInfoDialog("Generated unit tests successfully!");
        }
    }
}