using Avalonia.Diagnostics.ViewModels;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// ViewModel для заголовка колонки с сортировкой.
    /// </summary>
    public class SortHeaderViewModel : ViewModelBase
    {
        private static Dictionary<SortType, SortType> _changeSortMap = new Dictionary<SortType, SortType>()
        {
            { SortType.None, SortType.Asc },
            { SortType.Asc, SortType.Desc },
            { SortType.Desc, SortType.None }
        };

        /// <summary>
        /// Имя колонки.
        /// </summary>
        [Reactive] public string Name { get; set; }

        /// <summary>
        /// Сортировка.
        /// </summary>
        [Reactive] public SortType SortType { get; set; }

        public SortHeaderViewModel()
        {
            Name = "<Здесь могло быть ваше название>";
            SortType = SortType.None;
        }

        public void ChangeSort()
        {
            if (_changeSortMap.TryGetValue(SortType, out SortType newSortType))
            {
                SortType = newSortType;
            }
        }
    }
}