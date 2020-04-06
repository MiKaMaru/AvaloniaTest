using AvaloniaXmlLoadTest.Interfaces;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaXmlLoadTest.Utils
{
    /// <summary>
    /// Иерархический ItemViewModel
    /// </summary>
    public class HierarchicalItemViewModel : ItemViewModel, IHierarchicalItem
    {
        private IObservableCollection<IHierarchicalItem> _children;
        private string _parentKey;
        private bool _isExpanded;
        private bool _isVisible = true;

        public string ParentKey
        {
            get => _parentKey;
            set => this.RaiseAndSetIfChanged(ref _parentKey, value);
        }

        public bool IsSelectionItem { get; set; }

        public HierarchicalItemViewModel(string key, string value, string parentKey, bool isSelectionItem = false) : base(key, value)
        {
            ParentKey = parentKey;
            Children = new ObservableCollectionExtended<IHierarchicalItem>();
            IsSelectionItem = isSelectionItem;
        }

        public IObservableCollection<IHierarchicalItem> Children
        {
            get => _children;
            set => this.RaiseAndSetIfChanged(ref _children, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => this.RaiseAndSetIfChanged(ref _isVisible, value);
        }

        public object Clone()
        {
            return new HierarchicalItemViewModel(Key, Value, ParentKey, IsSelectionItem) { Children = new ObservableCollectionExtended<IHierarchicalItem>(Children) };
        }
    }
}
