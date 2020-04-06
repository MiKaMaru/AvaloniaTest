using AvaloniaXmlLoadTest.Interfaces;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.Interfaces
{
    public interface IHierarchicalItem : ICloneable, IItem
    {
        /// <summary>
        /// Потомки элемента
        /// </summary>
        IObservableCollection<IHierarchicalItem> Children { get; set; }

        /// <summary>
        /// Ключ родителя
        /// </summary>
        string ParentKey { get; set; }

        /// <summary>
        /// Может ли элемент быть выбран
        /// </summary>
        bool IsSelectionItem { get; set; }

        /// <summary>
        /// Раскрыт ли элемент в дереве
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Виден ли элемент в дереве
        /// </summary>
        bool IsVisible { get; set; }
    }
}
