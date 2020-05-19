using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using AvaloniaXmlLoadTest.Interfaces;
using AvaloniaXmlLoadTest.Utils;
using DynamicData.Binding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Avalonia.Controls.ToolTip;

namespace AvaloniaXmlLoadTest.TestControls
{
    public class TestTreeBox : AbstractFilterControl
    {
        #region DependancyProperties

        public static readonly DirectProperty<TestTreeBox, string> SelectedViewItemProperty;

        public static readonly DirectProperty<TestTreeBox, IObservableCollection<IHierarchicalItem>> ItemsProperty;

        public static readonly DirectProperty<TestTreeBox, object> SelectedItemProperty;

        public static readonly DirectProperty<TestTreeBox, bool> IsOpenProperty;

        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty;

        public static readonly StyledProperty<string> FilterPathProperty;

        public static readonly StyledProperty<double> MaxHeightDropDownProperty;

        public static readonly StyledProperty<int> TabIndexProperty;

        public static readonly StyledProperty<int> MaxSearchCountProperty;

        public static readonly StyledProperty<bool> DisabledProperty;

        #region ToolTip property
        public static readonly DirectProperty<TestTreeBox, string> ToolTipProperty;
        private string _toolTip;

        /// <summary>
        /// Всплывающая подсказка
        /// </summary>
        public string ToolTip
        {
            get => _toolTip;
            set => SetAndRaise(ToolTipProperty, ref _toolTip, value);
        }

        #endregion

        #endregion

        #region Static Constructor

        static TestTreeBox()
        {
            ItemsProperty = AvaloniaProperty.RegisterDirect<TestTreeBox, IObservableCollection<IHierarchicalItem>>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);
            ItemsProperty.Changed.AddClassHandler<TestTreeBox>(x => x.ItemsChanged);

            SelectedViewItemProperty = AvaloniaProperty.RegisterDirect<TestTreeBox, string>(
                nameof(SelectedViewItem),
                o => o.SelectedViewItem,
                (o, v) => o.SelectedViewItem = v);

            SelectedItemProperty = AvaloniaProperty.RegisterDirect<TestTreeBox, object>(
                nameof(SelectedItem),
                o => o.SelectedItem,
                (o, v) => o.SelectedItem = v,
                defaultBindingMode: BindingMode.TwoWay);
            SelectedItemProperty.Changed.AddClassHandler<TestTreeBox>(x => x.SelectedItemChanged);

            IsOpenProperty = AvaloniaProperty.RegisterDirect<TestTreeBox, bool>(
                nameof(IsOpen),
                o => o.IsOpen);
            IsOpenProperty.Changed.AddClassHandler<TestTreeBox>(x => x.IsOpenChanged);

            ItemTemplateProperty = AvaloniaProperty.Register<TestTreeBox, IDataTemplate>(nameof(ItemTemplate));

            FilterPathProperty = AvaloniaProperty.Register<TestTreeBox, string>(
                nameof(FilterPath),
                defaultValue: string.Empty);

            MaxHeightDropDownProperty = AvaloniaProperty.Register<TestTreeBox, double>(
                nameof(MaxHeightDropDown),
                defaultValue: 300);

            TabIndexProperty = AvaloniaProperty.Register<TestTreeBox, int>(
                nameof(TabIndex));
            TabIndexProperty.Changed.AddClassHandler<TestTreeBox>(x => x.TabIndexChanged);

            DisabledProperty = AvaloniaProperty.Register<TestTreeBox, bool>(
                nameof(Disabled));
            DisabledProperty.Changed.AddClassHandler<TestTreeBox>(x => x.DisabledChanged);

            FocusableProperty.OverrideDefaultValue<TestTreeBox>(true);


            MaxSearchCountProperty = AvaloniaProperty.Register<TestTreeBox, int>(nameof(MaxSearchCount), defaultValue: 100);

            ToolTipProperty = AvaloniaProperty.RegisterDirect<TestTreeBox, string>(
                nameof(ToolTip),
                o => o.ToolTip,
                (o, v) => o.ToolTip = v);
        }

        #endregion

        #region Private Fields

        private List<IHierarchicalItem> _allChildsItems;
        private object _selectedItem;
        private string _selectedViewItem = string.Empty;
        private IObservableCollection<IHierarchicalItem> _items = new ObservableCollectionExtended<IHierarchicalItem>();
        private bool _isOpen;
        private int _tabIndex;
        private bool _disabled;
        private IDisposable _subscriptionsOnOpen;

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TreeView1.Tapped += (sender, args) =>
            {
                //TODO костыли костылики
                if (((Control)args.Source).Parent is TreeViewItem treeViewItem)
                {
                    if (!((IHierarchicalItem)treeViewItem.DataContext).IsSelectionItem)
                    {
                        treeViewItem.IsExpanded = true;
                        args.Handled = true;
                        return;
                    }
                }
                else if ((Control)(((Control)args.Source).Parent).Parent is TreeViewItem treeViewItem2)
                {
                    if (!((IHierarchicalItem)treeViewItem2.DataContext).IsSelectionItem)
                    {
                        treeViewItem2.IsExpanded = true;
                        args.Handled = true;
                        return;
                    }
                }

                IsOpen = false;
                Filter = string.Empty;
            };

            TreeView1.SelectionChanged += (sender, args) =>
            {
                if (args.AddedItems.Count != 0)
                {
                    if (!((IHierarchicalItem)(args.AddedItems[0])).IsSelectionItem)
                    {
                        args.Handled = true;
                        return;
                    }

                    SelectedItem = args.AddedItems[0];
                }
            };

            TreeView1.KeyUp += (sender, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    IsOpen = false;
                    Filter = string.Empty;
                }
            };

            TextBox1.KeyUp += (sender, args) =>
            {
                args.Handled = true;
                if (args.Key >= Key.D0 && args.Key <= Key.Z || args.Key >= Key.NumPad0 && args.Key <= Key.NumPad9 || args.Key == Key.Delete || args.Key == Key.Back)
                {
                    var textBox = (TextBox)sender;
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        ClearCommand();
                    }
                    else
                    {
                        Filter = textBox.Text;
                        SelectedItem = null;
                        SelectedViewItem = Filter;
                        textBox.CaretIndex = Filter.Length;
                    }
                }
            };

            TextBox1.LayoutUpdated += (sender, args) =>
            {
                var tb = ((TextBox)sender);
                var difference = Height - tb.Bounds.Height;
                tb.Padding = new Thickness(tb.Padding.Left, tb.Padding.Top + difference / 2);
            };
        }

        /// <inheritdoc/>
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            if (!e.Handled)
            {
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (Popup1 != null)
            {
                Popup1.Opened -= PopupOpened;
                Popup1.Closed -= PopupClosed;
            }

            //Popup1 = this.FindControl<Popup>(nameof(Popup1));
            Popup1.PlacementTarget = TextBox1;
            Popup1.Opened += PopupOpened;
            Popup1.Closed += PopupClosed;

            base.OnTemplateApplied(e);
        }

        private void PopupOpened(object sender, EventArgs e)
        {
            _subscriptionsOnOpen?.Dispose();
            _subscriptionsOnOpen = null;
            var toplevel = this.GetVisualRoot() as TopLevel;
            if (toplevel != null)
            {
                _subscriptionsOnOpen = toplevel.AddHandler(PointerWheelChangedEvent, (s, ev) =>
                {
                    //eat wheel scroll event outside dropdown popup while it's open
                    if (IsOpen && ((ev.Source as IVisual).GetVisualRoot() == toplevel))
                    {
                        ev.Handled = true;
                    }
                }, RoutingStrategies.Tunnel);
            }
        }
        private void PopupClosed(object sender, EventArgs e)
        {
            _subscriptionsOnOpen?.Dispose();
            _subscriptionsOnOpen = null;

            if (CanFocus(this))
            {
                Focus();
            }
        }

        private bool CanFocus(IControl control) => control.Focusable && control.IsEffectivelyVisible && control.IsVisible;

        public List<IHierarchicalItem> GetAllChildsItem(IObservableCollection<IHierarchicalItem> allItem)
        {
            var allChildsItem = new List<IHierarchicalItem>();
            foreach (var item in allItem)
            {
                if (item.IsSelectionItem)
                {
                    allChildsItem.Add(item);
                    if (item.Children != null && item.Children.Count > 0)
                    {
                        allChildsItem.AddRange(GetAllChildsItem(item.Children));
                    }
                }
                else
                {
                    allChildsItem.AddRange(GetAllChildsItem(item.Children));
                }
            }

            return allChildsItem;
        }

        private bool ContainElementKey(IHierarchicalItem hierarchicalItem, string key)
        {
            return hierarchicalItem.Key == key || hierarchicalItem.Children.Any(hierarchicalItemChild => ContainElementKey(hierarchicalItemChild, key));
        }

        private void HideItem(IObservableCollection<IHierarchicalItem> allItem, IList<IHierarchicalItem> filteredItems)
        {
            foreach (var item in allItem)
            {
                if (!filteredItems.Any(x => ContainElementKey(item, x.Key)))
                {
                    item.IsVisible = false;
                }
                else
                {
                    item.IsVisible = true;
                    item.IsExpanded = true;
                    HideItem(item.Children, filteredItems);
                }
            }
        }

        private void ShowAllItem(IObservableCollection<IHierarchicalItem> allItem)
        {
            foreach (var item in allItem)
            {
                item.IsVisible = true;
                item.IsExpanded = false;
                ShowAllItem(item.Children);
            }
        }

        private void Expand()
        {
            for (var i = 0; i < TreeView1.Items.Cast<object>().Count(); i++)
            {
                if (TreeView1.ItemContainerGenerator.ContainerFromIndex(i) is TreeViewItem childControl)
                    childControl.IsExpanded = false;
            }
        }


        #endregion

        #region Constructor

        public TestTreeBox()
        {
            FontWeight = FontWeight.Normal;
            Background = Brushes.White;
            FontFamily = new FontFamily("Calibri");
            FontSize = 14;
            FontStyle = FontStyle.Normal;

            InitializeComponent();
            ToolTipProperty.Changed.AddClassHandler<TestTreeBox>(x => x.ToolTipChanged);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Все элементы
        /// </summary>
        public IObservableCollection<IHierarchicalItem> Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }

        /// <summary>
        /// Таб индекс
        /// </summary>
        public int TabIndex
        {
            get => _tabIndex;
            set => SetAndRaise(TabIndexProperty, ref _tabIndex, value);
        }

        /// <summary>
        /// Активен ли элемент
        /// </summary>
        public bool Disabled
        {
            get => _disabled;
            set => SetAndRaise(DisabledProperty, ref _disabled, value);
        }

        /// <summary>
        /// Выбранный элемент
        /// </summary>
        public object SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
        }

        /// <summary>
        /// Выбранный элемент в поле ввода
        /// </summary>
		public string SelectedViewItem
        {
            get => _selectedViewItem;
            set => SetAndRaise(SelectedViewItemProperty, ref _selectedViewItem, value);
        }

        /// <summary>
        /// Открыт ли popup
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

        /// <summary>
        /// Шаблон элемента
        /// </summary>
		public IDataTemplate ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Свойство объекта по которому будет происходить фильтрация
        /// </summary>
		public string FilterPath
        {
            get => GetValue(FilterPathProperty);
            set => SetValue(FilterPathProperty, value);
        }

        /// <summary>
        /// Максимальная высота раскрывающегося списка
        /// </summary>
		public double MaxHeightDropDown
        {
            get => GetValue(MaxHeightDropDownProperty);
            set => SetValue(MaxHeightDropDownProperty, value);
        }

        /// <summary>
        /// Максимальное количество найденных элементов
        /// </summary>
		public int MaxSearchCount
        {
            get => GetValue(MaxSearchCountProperty);
            set => SetValue(MaxSearchCountProperty, value);
        }

        #endregion

        #region Controls

        public TreeView TreeView1 => this.FindControl<TreeView>(nameof(TreeView1));
        public TextBox TextBox1 => this.FindControl<TextBox>(nameof(TextBox1));
        public ToggleButton button1 => this.FindControl<ToggleButton>(nameof(button1));
        public Popup Popup1 => this.FindControl<Popup>(nameof(Popup1));// { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Команд очистки поля ввода и закрытия всех узлов
        /// </summary>
        public void ClearCommand()
        {
            SetTip(TextBox1, null);
            SelectedViewItem = string.Empty;
            Filter = string.Empty;
            SelectedItem = null;
            TextBox1.Focus();
            Expand();
        }

        #endregion

        #region PropertyChanges

        protected override void Filtering(string value)
        {
            base.Filtering(value);

            if (Items == null || value == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(value))
            {
                IsOpen = true;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var filteredItems = _allChildsItems.Where(x => x.Value.ToLower().Contains(value.ToLower())).Take(MaxSearchCount).ToList();
            HideItem(Items, filteredItems);
            stopwatch.Stop();
            Debug.WriteLine("Время фильтрации:" + stopwatch.Elapsed.TotalMilliseconds);
        }

        protected override void NoFiltering(string value)
        {
            base.NoFiltering(value);

            if (Items == null || value == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(value))
            {
                IsOpen = true;
            }

            ShowAllItem(Items);
        }

        protected override int GetCountItems()
        {
            return GetTotalCount(Items);
        }

        private void DisabledChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            bool val = (bool)obj.NewValue;
            TextBox1.IsReadOnly = val;
            TextBox1.IsEnabled = !val;
            TreeView1.IsEnabled = !val;
            button1.IsEnabled = !val;
        }

        private void TabIndexChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            //TODO: найти где у авалонии tabIndex
            //TextBox1.CaretIndex
        }

        private void IsOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                TextBox1.Focus();
            }
        }

        private void SelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            SelectedViewItem = ((IHierarchicalItem)e.NewValue).Value;
            TreeView1.SelectedItem = e.NewValue;
        }

        private void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            SelectedViewItem = string.Empty;
            if (e.NewValue != null)
                _allChildsItems = GetAllChildsItem(Items);
            TreeView1.IsVisible = e.NewValue != null &&
                                ((e.NewValue as IEnumerable) ?? throw new InvalidOperationException()).Cast<object>()
                                .Count() != 0;
        }

        private int GetTotalCount(IEnumerable<IHierarchicalItem> item)
        {
            return item?.Sum(x => 1 + GetTotalCount(x.Children)) ?? 0;
        }

        private void ToolTipChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            SetTip(TextBox1, ToolTip);
        }
        #endregion
    }
}
