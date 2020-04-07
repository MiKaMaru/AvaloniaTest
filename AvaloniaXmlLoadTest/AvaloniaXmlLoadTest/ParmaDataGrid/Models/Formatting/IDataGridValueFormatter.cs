namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models.Formatting
{
    /// <summary>
    /// Интерфейс форматирования значений <see cref="ParmaDataGrid"/>
    /// </summary>
    public interface IDataGridValueFormatter
    {
        /// <summary>
        /// Формат, к которому значение нужно привести
        /// </summary>
        string OutputFormat { get; }
        /// <summary>
        /// Форматирование значения
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Результат</returns>
        string FormatValue(string value);
        /// <summary>
        /// Парсинг и боксинг значения
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Упаковонное значение</returns>
        object ParseValue(string value);
    }
}
