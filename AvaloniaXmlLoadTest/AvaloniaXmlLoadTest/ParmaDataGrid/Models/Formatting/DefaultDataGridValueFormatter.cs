using System;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models.Formatting
{
    /// <summary>
    /// Дефолтный форматтер для строковых значений колонок.
    /// Возвращает исходное значение.
    /// </summary>
    public class DefaultDataGridValueFormatter : IDataGridValueFormatter
    {
        public static Type Type => typeof(string);

        public string OutputFormat => string.Empty;

        public string FormatValue(string value)
        {
            return value;
        }

        public object ParseValue(string value)
        {
            return value;
        }
    }
}
