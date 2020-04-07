using Avalonia.Controls;
using Avalonia.Data;
using AvaloniaXmlLoadTest.DataGridDomain;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using AvaloniaXmlLoadTest.ParmaDataGrid.Views;
using System.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ColumnCreatorFactory.Creators
{
    public class UnSortableTextColumnCreator : ParmaDataGridSortColumnCreator
    {
        public override DataGridColumn CreateColumn(ParmaDataGridOdColumnInfo info)
        {
            var (minWidth, maxWidth) = GetWidth(info.Width);

            return new ParmaDataGridTextColumn()
            {
                Header = new HeaderView() { DataContext = new HeaderViewModel() { Name = info.Name } },
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
