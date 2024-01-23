using Microsoft.Win32;
using System.Collections.ObjectModel;
using System;
using System.IO;

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

        private void AddFile()
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
                        string existingChecksum = File.ReadAllText(checksumFilePath).Trim();
                        string calculatedChecksum = ChecksumModel.CalculateChecksum(fileItem.FilePath);

                        if (string.Equals(existingChecksum, calculatedChecksum, StringComparison.OrdinalIgnoreCase))
                        {
                            fileItem.Checksum = existingChecksum;
                            fileItem.Result = "Checksum is valid";
                        }
                        else
                        {
                            ChecksumModel.UpdateChecksum(fileItem.FilePath);
                            fileItem.Checksum = calculatedChecksum;
                            fileItem.Result = "Checksum is invalid. Right checksum was write";
                        }
                    }
                    else
                    {
                        string calculatedChecksum = ChecksumModel.CalculateChecksum(fileItem.FilePath);
                        ChecksumModel.SaveChecksumToFile(fileItem.FilePath, calculatedChecksum);
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
