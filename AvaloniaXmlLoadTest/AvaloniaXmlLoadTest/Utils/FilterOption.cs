using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.Utils
{
    /// <summary>
    /// Настройка фильтра. Соответствие количества элементов и нужного количества символов для запуска поиска.
    /// </summary>
    public class FilterOption
    {
        /// <summary>
        /// Нижняя граница количества элементов.
        /// </summary>
        public int ItemsCount { get; set; }

        /// <summary>
        /// Количество символов для запуска поиска.
        /// </summary>
        public int CharCount { get; set; }
    }
}
