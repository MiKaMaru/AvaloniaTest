using System;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models.Formatting
{
    /// <summary>
    /// Форматтер для типа DateTime
    /// </summary>
    public class DatetimeDataGridValueFormatter : IDataGridValueFormatter
    {
        private readonly string[] _inputDatetimeFormats = 
        {
            "dd.MM.yyyy", "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss.fff",
            "yyyy.MM.dd", "yyyy.MM.dd HH:mm", "yyyy.MM.dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss.fff",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz", "yyyy.dd.MM HH:mm:ss"
        };

        public static Type Type => typeof(DateTime);

        public string OutputFormat { get; }

        public DatetimeDataGridValueFormatter(string outputFormat)
        {
            OutputFormat = outputFormat;
        }

        public string FormatValue(string value)
        {
            var result = ParseValueInternal(value);
            return result.HasValue ? result.Value.ToString(OutputFormat) : value;
        }

        public object ParseValue(string value)
        {
            return ParseValueInternal(value);
        }

        private DateTime? ParseValueInternal(string value)
        {
            if (DateTime.TryParseExact(value?.Trim(), _inputDatetimeFormats, null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            return null;
        }
    }
}