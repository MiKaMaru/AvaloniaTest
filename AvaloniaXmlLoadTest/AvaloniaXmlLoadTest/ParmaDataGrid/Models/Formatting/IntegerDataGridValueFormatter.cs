using System;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models.Formatting
{
    /// <summary>
    /// Форматтер для значений с типом Int32
    /// </summary>
    public class IntegerDataGridValueFormatter : IDataGridValueFormatter
    {
        public static Type Type => typeof(int);

        public string OutputFormat => string.Empty;

        public string FormatValue(string value)
        {
            return value;
        }

        public object ParseValue(string value)
        {
            return ParseValueInternal(value);
        }

        private int? ParseValueInternal(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return null;
        }
    }
}
