using System;
using System.Reactive;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
    /// <summary>
    /// Интерфейс поведения колонки с сортировкой
    /// </summary>
    public interface IParmaDataGridSortColumn
    {
        /// <summary>
        /// Имя колонки
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        SortType SortType { get; set; }

        /// <summary>
        /// Идентификатор колонки
        /// </summary>
        object ColumnId { get; set; }

        /// <summary>
        /// Подписка на событие сортировки
        /// </summary>
        IObservable<Unit> ChangeSort { get; }

        /// <summary>
        /// Вьюха заголовка колонки
        /// </summary>
        object Header { get; set; }
    }
}