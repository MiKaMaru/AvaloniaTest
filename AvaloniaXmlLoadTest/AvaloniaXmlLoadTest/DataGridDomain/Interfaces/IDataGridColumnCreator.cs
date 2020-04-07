using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.DataGridDomain.Interfaces
{
    /// <summary>
    /// Интерфейс фабрики создания колонки для сортировок данных
    /// </summary>
    public interface IDataGridColumnCreator
    {
        DataGridColumn CreateColumn(ParmaDataGridOdColumnInfo info);
    }
}
