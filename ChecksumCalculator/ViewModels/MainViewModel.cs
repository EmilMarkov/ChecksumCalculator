using Microsoft.Win32;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChecksumCalculator
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<FileItemViewModel> FileItems { get; set; } = new ObservableCollection<FileItemViewModel>();

        #endregion

        #region Commands

        public DelegateCommand AddFileCommand { get; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            AddFileCommand = new DelegateCommand(_ => AddFile());
        }

        #endregion

        #region Private Methods

        private async void AddFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    var fileItem = new FileItemViewModel { FilePath = fileName };

                    string checksumFilePath = fileName + ".checksum";

                    if (File.Exists(checksumFilePath))
                    {
                        string existingChecksum = await Task.Run(() => File.ReadAllText(checksumFilePath).Trim());
                        string calculatedChecksum = await Task.Run(() => ChecksumModel.CalculateChecksum(fileItem.FilePath));

                        if (string.Equals(existingChecksum, calculatedChecksum, StringComparison.OrdinalIgnoreCase))
                        {
                            fileItem.Checksum = existingChecksum;
                            fileItem.Result = "Checksum is valid";
                        }
                        else
                        {
                            fileItem.Result = "Checksum is invalid";
                        }
                    }
                    else
                    {
                        string calculatedChecksum = await Task.Run(() => ChecksumModel.CalculateChecksum(fileItem.FilePath));
                        await Task.Run(() => ChecksumModel.SaveChecksumToFile(fileItem.FilePath, calculatedChecksum));
                        fileItem.Checksum = calculatedChecksum;
                        fileItem.Result = "Checksum calculated";
                    }

                    FileItems.Add(fileItem);
                }
            }
        }

        #endregion
    }
}
