using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.DataGridDomain.Interfaces
{
    /// <summary>
    /// ViewModel для редактирования настроек колонки.
    /// </summary>
    public interface IDataGridColumnViewModel : IColumnViewModel
    {
        /// <summary>
		/// Колонка.
		/// </summary>
        DataGridColumn DataGridColumn { get; set; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Колонка видима.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
		/// Колонка активна.
		/// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Стиль колонки.
        /// </summary>
        IEnumerable<string> Classes { get; set; }

        /// <summary>
        /// Применить параметры.
        /// </summary>
        void Apply();
    }
}
