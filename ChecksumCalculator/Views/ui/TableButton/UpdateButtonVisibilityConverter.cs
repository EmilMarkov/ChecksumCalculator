using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChecksumCalculator
{
    public class UpdateButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = value as string;

            return (result == "Checksum is invalid") ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
