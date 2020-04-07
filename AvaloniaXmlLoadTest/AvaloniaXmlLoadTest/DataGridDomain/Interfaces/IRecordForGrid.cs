using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.DataGridDomain.Interfaces
{
    /// <summary>
    /// Интерфейс для строки таблицы
    /// </summary>
    public interface IRecordForGrid
    {
        int Id { get; set; }
        Dictionary<int, string> Values { get; set; }
        bool IsChecked { get; set; }
    }
}
