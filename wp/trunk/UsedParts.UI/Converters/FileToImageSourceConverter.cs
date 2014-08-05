using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using UsedParts.Common;

namespace UsedParts.UI.Converters
{
    public class FileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = new BitmapImage();
            try
            {
                var path = (string)value;
                if (!String.IsNullOrEmpty(path))
                {
                    using (var file = LoadFile(path))
                    {
                        bitmap.SetSource(file);
                    }
                }
            }
            catch(FileStorageException fileStorageException)
            {
                Debug.WriteLine("Error while reading image file: {0}", fileStorageException);
            }
            return bitmap;
        }

        private static Stream LoadFile(string file)
        {
            var fileStorageManager = IoC.Get<IFileStorageManager>();
            return fileStorageManager.OpenFile(file);
        } 

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
