using Microsoft.Win32;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Windows.Forms;

namespace ChecksumCalculator
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<FileItemViewModel> FileItems { get; set; } = new ObservableCollection<FileItemViewModel>();

        #endregion

        #region Commands

        public DelegateCommand AddFileCommand { get; }
        public DelegateCommand AddFolderCommand { get; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            AddFileCommand = new DelegateCommand(_ => AddFile());
            AddFolderCommand = new DelegateCommand(_ => AddFolder());
        }

        #endregion

        #region Private Methods

        private async void AddFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a file",
                Filter = "All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    if (Path.GetExtension(fileName).Equals(".checksum", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var fileItem = new FileItemViewModel { FilePath = fileName };

                    string checksumFilePath = fileName + ".checksum";

                    ChecksumModel.VerifyChecksum(checksumFilePath, fileItem);

                    FileItems.Add(fileItem);
                }
            }
        }

        private async void AddFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select a folder"
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
                await AddFilesFromFolder(selectedFolder);
            }
        }

        private async Task AddFilesFromFolder(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (var filePath in files)
                {
                    if (Path.GetExtension(filePath).Equals(".checksum", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var fileItem = new FileItemViewModel { FilePath = filePath };

                    string checksumFilePath = filePath + ".checksum";

                    ChecksumModel.VerifyChecksum(checksumFilePath, fileItem);

                    FileItems.Add(fileItem);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при доступе к файлам или другим проблемам
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion
    }
}
