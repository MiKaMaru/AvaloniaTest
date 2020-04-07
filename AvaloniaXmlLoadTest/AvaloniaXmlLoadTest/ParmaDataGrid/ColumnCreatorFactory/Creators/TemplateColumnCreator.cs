using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaXmlLoadTest.DataGridDomain;
using System;
using System.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ColumnCreatorFactory.Creators
{
    /// <summary>
    /// Реализация для создания кастомных колонок сортировки данных в <see cref="ParmaDataGridOd"/>
    /// </summary>
    public class TemplateColumnCreator : ParmaDataGridSortColumnCreator
    {
        private readonly FuncDataTemplate _cellDataTemplate;

        public TemplateColumnCreator(FuncDataTemplate cellDataTemplate)
        {
            _cellDataTemplate = cellDataTemplate;
        }

        public override DataGridColumn CreateColumn(ParmaDataGridOdColumnInfo info)
        {
            if (_cellDataTemplate == null)
                throw new ArgumentException("Должен быть задан шаблон ячейки для формирования колонки", nameof(_cellDataTemplate));

            var (minWidth, maxWidth) = GetWidth(info.Width);

            return new ParmaDataGridSortTemplateColumn(_cellDataTemplate)
            {
                Name = info.Name,
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