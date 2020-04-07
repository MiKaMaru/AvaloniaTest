using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
	/// <summary>
	/// Базовый класс текстовой колонки с переносом текста.
	/// </summary>
	public class ParmaDataGridTextColumn : DataGridTextColumn
	{
        private object _сolumnId;

        public static readonly DirectProperty<ParmaDataGridTextColumn, object> ColumnIdProperty =
            AvaloniaProperty.RegisterDirect<ParmaDataGridTextColumn, object>(
                nameof(ColumnId),
                (o) => o.ColumnId,
                (o, v) => o.ColumnId = v,
                null);

        public object ColumnId
        {
            get => _сolumnId;
            set => SetAndRaise(ColumnIdProperty, ref _сolumnId, value);
        }

        protected override IControl GenerateElement(DataGridCell cell, object dataItem)
		{
			IControl control = base.GenerateElement(cell, dataItem);
			if (control is TextBlock textBlock)
			{
				textBlock.TextWrapping = TextWrapping.Wrap;
			}
			return control;
		}
	}
}
