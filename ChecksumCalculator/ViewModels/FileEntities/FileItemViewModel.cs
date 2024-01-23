using System.IO;

namespace ChecksumCalculator
{
    public class FileItemViewModel : BaseViewModel
    {
        public string FilePath { get; set; }

        public string FileName => Path.GetFileName(FilePath);

        public string Checksum { get; set; }

        public string Result { get; set; }
    }
}
