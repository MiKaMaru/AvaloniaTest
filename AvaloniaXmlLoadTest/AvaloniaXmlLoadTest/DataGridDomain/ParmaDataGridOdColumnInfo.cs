using Avalonia.Controls;
using AvaloniaXmlLoadTest.DataGridDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaXmlLoadTest.DataGridDomain
{
    /// <summary>
    /// Типы фильтров в колонках гридов в АРМ Од
    /// </summary>
    public enum ArmodFilterTypes
    {
        DateTimeFilter,
        TextFilter,
        ComboBox
    }
    /// <summary>
    /// Описание колонки таблицы АРМ «Обработка данных».
    /// </summary>
    public class ParmaDataGridOdColumnInfo : IColumnInfo
    {
        /// <summary>
        /// Ключ реквизита.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Название колонки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя колонки в БД.
        /// </summary>
        public string NameInDB { get; set; }

        /// <summary>
        /// Тип данных в колонке.
        /// </summary>
        public Type Type { get; set; } = typeof(string);

        /// <summary>
        /// Ширина колонки.
        /// </summary>
        public DataGridLength Width { get; set; } = DataGridLength.Auto;

        /// <summary>
        /// Стиль колонки.
        /// </summary>
        public IEnumerable<string> Classes { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Видимость.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Тип применяемого фильтра.
        /// </summary>
        public ArmodFilterTypes FilterType { get; set; }

        /// <summary>
        /// Свойство отвечает за принудительный показ колонки в настройках таблицы
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Фабрика создания колонки для вьюхи грида
        /// </summary>
        public IDataGridColumnCreator SortFactory { get; set; }

        public ParmaDataGridOdColumnInfo()
        {
        }

        public ParmaDataGridOdColumnInfo(int key, string name) :
            this(key, name, typeof(string), DataGridLength.Auto)
        {
        }

        public ParmaDataGridOdColumnInfo(int key, string name, int width) :
            this(key, name, typeof(string), new DataGridLength(width))
        {
        }

        public ParmaDataGridOdColumnInfo(int key, string name, Type type, int width) :
            this(key, name, type, new DataGridLength(width))
        {
        }

        public ParmaDataGridOdColumnInfo(int key, string name, Type type, DataGridLength width) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Type = type ?? throw new ArgumentNullException(nameof(type));
            Key = key;
            Name = name;
            Width = width;
        }
    }
}
