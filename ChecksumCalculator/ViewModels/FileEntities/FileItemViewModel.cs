using System.IO;

namespace ChecksumCalculator
{
    public class FileItemViewModel : BaseViewModel
    {
        #region Properties

        public string FilePath { get; set; }

        public string FileName => Path.GetFileName(FilePath);

        public string Checksum { get; set; }

        public string Result { get; set; }

        #endregion

        #region Commands

        public DelegateCommand UpdateChecksumCommand { get; }

        #endregion

        #region Constructor

        public FileItemViewModel()
        {
            UpdateChecksumCommand = new DelegateCommand(_ => UpdateChecksum());
        }

        #endregion

        #region Command Methods

        private void UpdateChecksum()
        {
            string newChecksum = ChecksumModel.CalculateChecksum(FilePath);
            ChecksumModel.SaveChecksumToFile(FilePath, newChecksum);
            Checksum = newChecksum;
            Result = "Checksum updated";
        }

        #endregion
    }
}
