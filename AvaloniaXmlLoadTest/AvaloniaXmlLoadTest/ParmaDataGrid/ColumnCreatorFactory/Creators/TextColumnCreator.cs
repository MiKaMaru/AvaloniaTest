using Avalonia.Controls;
using Avalonia.Data;
using AvaloniaXmlLoadTest.DataGridDomain;
using System.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ColumnCreatorFactory.Creators
{
    /// <summary>
    /// Реализация для создания текстовых колонок сортировки данных в <see cref="ParmaDataGridOd"/>
    /// </summary>
    public class TextColumnCreator : ParmaDataGridSortColumnCreator
    {
        public override DataGridColumn CreateColumn(ParmaDataGridOdColumnInfo info)
        {
            var (minWidth, maxWidth) = GetWidth(info.Width);

            return new ParmaDataGridSortTextColumn
            {
                Name = info.Name,
                Binding = new Binding($"Values[{info.Key}]", BindingMode.OneWay),
                MinWidth = minWidth,
                MaxWidth = maxWidth,
                CanUserReorder = true,
                CanUserSort = false,
                CanUserResize = true,
                ColumnId = info.Key,
                CellStyleClasses = new Classes(info.Classes ?? Enumerable.Empty<string>())
            };
        }
    }
}