using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.Utils
{
    /// <summary>
    /// Конвертер, который конвертирует пути до изображений в изображения.
    /// </summary>
    public class PathToBitmapConverter : IValueConverter, IBindingTypeConverter
    {
        /// <summary>
        /// Регулярное выражение, для получения пути.
        /// </summary>
        private readonly Regex _extractNormalPathRegex = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");

        /// <summary>
        /// Сепаратор разделяющий путь в xaml. 
        /// </summary>
        private readonly char _separator = '|';

        /// <summary>
        /// Конвертирует путь в изображение.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            string path = (string)value;
            var pathValues = path.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            var pathList = pathValues.ToList();
            pathList.Insert(0, GetApplicationRoot());
            var fullPath = Path.Combine(pathList.ToArray());
            return new Bitmap(fullPath);
        }

        /// <summary>
        /// Получение пути до папки где лежит exe.
        /// </summary>
        /// <returns>Путь до папки , где лежит exe.</returns>
        public string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                .Assembly.GetExecutingAssembly().CodeBase);
            var appRoot = _extractNormalPathRegex.Match(exePath).Value;
            return appRoot;
        }

        /// <summary>
        /// Получение viewModel.
        /// </summary>
        /// <typeparam name="T">Тип вью модели.</typeparam>
        /// <returns>Созданный объект вью модели.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            throw new NotImplementedException();
        }

        public bool TryConvert(object @from, Type toType, object conversionHint, out object result)
        {
            if (@from == null)
            {
                result = null;
                return false;
            }
            var path = (string)@from;
            var pathValues = path.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            var pathList = pathValues.ToList();
            pathList.Insert(0, GetApplicationRoot());
            var fullPath = Path.Combine(pathList.ToArray());
            result = new Bitmap(fullPath);
            return true;

        }
    }
}
