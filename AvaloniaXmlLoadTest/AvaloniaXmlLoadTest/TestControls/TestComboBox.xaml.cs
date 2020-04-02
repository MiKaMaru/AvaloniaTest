using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Collections;
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
using DynamicData;
//using Parma.Gasps.XPlat.GUI.Common.Utils;
//using Parma.Gasps.XPlat.GUI.Interfaces;
using Serilog;
using static Avalonia.Controls.ToolTip;

namespace AvaloniaXmlLoadTest.TestControls
{
    public class TestComboBox : AbstractFilterControl
    {
        #region DependencyProperties

        public static readonly DirectProperty<TestComboBox, string> SelectedViewItemProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, string>(
                nameof(SelectedViewItem),
                o => o.SelectedViewItem,
                (o, v) => o.SelectedViewItem = v);

        public static readonly DirectProperty<TestComboBox, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);

        public static readonly DirectProperty<TestComboBox, IEnumerable> FilteredItemsProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, IEnumerable>(
                nameof(FilteredItems),
                o => o.FilteredItems,
                (o, v) => o.FilteredItems = v);

        public static readonly DirectProperty<TestComboBox, int> SelectedIndexProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, int>(
                nameof(SelectedIndex),
                o => o.SelectedIndex,
                (o, v) => o.SelectedIndex = v,
                unsetValue: -1);

        public static readonly DirectProperty<TestComboBox, object> SelectedItemProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, object>(
                nameof(SelectedItem),
                o => o.SelectedItem,
                (o, v) => o.SelectedItem = v,
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly DirectProperty<TestComboBox, bool> IsOpenProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, bool>(
                nameof(IsOpen),
                o => o.IsOpen,
                (o, v) => o.IsOpen = v);

        public static readonly DirectProperty<TestComboBox, bool> IsFormatSearchProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, bool>(
                nameof(IsFormatSearch),
                o => o.IsFormatSearch,
                (o, v) => o.IsFormatSearch = v);

        public static readonly DirectProperty<TestComboBox, string> WatermarkProperty;

        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
            AvaloniaProperty.Register<TestComboBox, IDataTemplate>(nameof(ItemTemplate));

        public static readonly StyledProperty<string> FilterPathProperty =
            AvaloniaProperty.Register<TestComboBox, string>(
                nameof(FilterPath),
                defaultValue: string.Empty);

        public static readonly StyledProperty<double> MaxHeightDropDownProperty =
            AvaloniaProperty.Register<TestComboBox, double>(
                nameof(MaxHeightDropDown),
                defaultValue: 300);

        public static readonly StyledProperty<int> TabIndexProperty =
            AvaloniaProperty.Register<TestComboBox, int>(
                nameof(TabIndex));

        public static readonly StyledProperty<bool> DisabledProperty =
            AvaloniaProperty.Register<TestComboBox, bool>(
                nameof(Disabled));

        public static readonly StyledProperty<bool> IsTextBoxFocusProperty =
            AvaloniaProperty.Register<TestComboBox, bool>(
                nameof(IsTextBoxFocus));

        public static readonly DirectProperty<TestComboBox, bool> ClearButtonNotVisibleProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, bool>(
                nameof(ClearButtonNotVisible),
                o => o.ClearButtonNotVisible,
                (o, v) => o.ClearButtonNotVisible = v);

        public static readonly StyledProperty<IBrush> ArrowColorProperty =
            AvaloniaProperty.Register<TestComboBox, IBrush>(
                nameof(ArrowColor),
                defaultValue: Brushes.Black);

        public static readonly StyledProperty<IBrush> ButtonColorProperty =
            AvaloniaProperty.Register<TestComboBox, IBrush>(
                nameof(ButtonColor),
                defaultValue: Brushes.White);

        #region ToolTip property
        public static readonly DirectProperty<TestComboBox, string> ToolTipProperty =
            AvaloniaProperty.RegisterDirect<TestComboBox, string>(
                nameof(ToolTip),
                o => o.ToolTip,
                (o, v) => o.ToolTip = v);

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

        static TestComboBox()
        {
            ItemsProperty.Changed.AddClassHandler<TestComboBox>(x => x.ItemsChanged);

            SelectedItemProperty.Changed.AddClassHandler<TestComboBox>(x => x.SelectedItemChanged);

            IsOpenProperty.Changed.AddClassHandler<TestComboBox>(x => x.IsOpenChanged);

            FilteredItemsProperty.Changed.AddClassHandler<TestComboBox>(x => x.FilteredItemsChanged);

            TabIndexProperty.Changed.AddClassHandler<TestComboBox>(x => x.TabIndexChanged);

            DisabledProperty.Changed.AddClassHandler<TestComboBox>(x => x.DisabledChanged);

            FocusableProperty.OverrideDefaultValue<TestComboBox>(true);

            ToolTipProperty.Changed.AddClassHandler<TestComboBox>(x => x.ToolTipChanged);
        }

        #endregion

        #region Private Fields

        private int _selectedIndex = -1;

        private object _selectedItem;

        private string _selectedViewItem = string.Empty;

        private IEnumerable _items = new AvaloniaList<object>();

        private IEnumerable _filteredItems = new AvaloniaList<object>();

        private bool _isOpen;

        private IDisposable _subscriptionsOnOpen;

        private int _tabIndex;
        private bool _disabled;
        private bool _isFormatSearch;
        private bool _clearButtonNotVisible;

        #endregion

        #region Private Methods

        private string GetValueByStringProperty(object source, string propertyName)
        {
            var pi = source.GetType().GetProperty(propertyName);
            return (string)pi.GetValue(source, null);
        }

        private void FocusToSelectedItem()
        {
            ListBox1.SelectedIndex = ListBox1.SelectedIndex != -1 ? ListBox1.SelectedIndex : 0;
            var lbi = (ListBoxItem)
                (ListBox1.ItemContainerGenerator.ContainerFromIndex(ListBox1.SelectedIndex));

            lbi?.Focus();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ListBox1.Tapped += (sender, args) =>
            {
                IsOpen = false;
                Filter = string.Empty;
            };

            ListBox1.SelectionChanged += (sender, args) =>
            {
                if (args.AddedItems.Count != 0)
                {
                    SelectedItem = args.AddedItems[0];
                    SelectedIndex = Items.Cast<object>().IndexOf(SelectedItem);
                }
            };

            ListBox1.KeyUp += (sender, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    IsOpen = false;
                    Filter = string.Empty;
                }
            };

            TextBox1.KeyUp += (sender, args) =>
            {
                if (args.Key == Key.LeftCtrl || args.Key == Key.LeftShift ||
                   (args.Modifiers == InputModifiers.Shift && (args.Key == Key.Left || args.Key == Key.Right)) ||
                   (args.Modifiers == InputModifiers.Shift && (args.Key == Key.Home || args.Key == Key.End)) ||
                   (args.Modifiers == InputModifiers.Control && args.Key == Key.A) ||
                   (args.Modifiers == InputModifiers.Control && args.Key == Key.C)) // копировать и выделять в TestComboBox теперь можно
                {
                    return;
                }

                args.Handled = true;
                if (args.Key >= Key.D0 && args.Key <= Key.Z || args.Key >= Key.NumPad0 && args.Key <= Key.NumPad9 || args.Key == Key.Delete || args.Key == Key.Back || CheckLastLetter(sender))
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
                        ListBox1.SelectedIndex = -1;
                        SelectedIndex = -1;
                        SelectedViewItem = Filter;
                        textBox.CaretIndex = Filter.Length;
                    }
                }
                else if (args.Key == Key.Up || args.Key == Key.Down)
                {
                    FocusToSelectedItem();
                }
            };

            TextBox1.LayoutUpdated += (sender, args) =>
            {
                var tb = ((TextBox)sender);
                var difference = Height - tb.Bounds.Height;
                tb.Padding = new Thickness(tb.Padding.Left, tb.Padding.Top + difference / 2);
            };

            TextBox1.PropertyChanged += TextBox1_PropertyChanged;

            KeyUp += (sender, args) =>
            {
                if (args.Key == Key.Up || args.Key == Key.Down)
                {
                    FocusToSelectedItem();
                }
            };
        }

        private void TextBox1_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "IsCustomFocused")
            {
                IsTextBoxFocus = (bool)e.NewValue;
            }
        }

        private bool CheckLastLetter(object sender)
        {
            var textBox = (TextBox)sender;

            if (textBox == null || string.IsNullOrEmpty(textBox.Text))
                return false;

            string lastLetter = textBox.Text.Substring(textBox.Text.Length - 1);
            Regex regex = new Regex("[а-яА-Я]");
            return regex.IsMatch(lastLetter);
        }

        /// <inheritdoc/>
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            if (!e.Handled)
            {
                if (!IsOpen)
                {
                    if (IsFocused)
                    {
                        if (e.Delta.Y < 0)
                            SelectNext();
                        else
                            SelectPrev();

                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void SelectNext()
        {
            int next = SelectedIndex + 1;

            if (next >= Items?.Cast<object>().Count())
                next = 0;

            SelectedIndex = next;
        }

        private void SelectPrev()
        {
            int prev = SelectedIndex - 1;

            if (prev < 0)
                prev = Items?.Cast<object>().Count() - 1 ?? 0;

            SelectedIndex = prev;
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (Popup1 != null)
            {
                Popup1.Opened -= PopupOpened;
                Popup1.Closed -= PopupClosed;
            }

            Popup1 = this.FindControl<Popup>(nameof(Popup1));
            Popup1.PlacementTarget = TextBox1;
            Popup1.Opened += PopupOpened;
            Popup1.Closed += PopupClosed;
            base.OnTemplateApplied(e);
        }


        private void PopupOpened(object sender, EventArgs e)
        {
            _subscriptionsOnOpen?.Dispose();
            _subscriptionsOnOpen = null;
            if (this.GetVisualRoot() is TopLevel toplevel)
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

        #endregion

        #region Constructor

        public TestComboBox()
        {
            FontWeight = FontWeight.Normal;
            Background = Brushes.White;
            FontFamily = new FontFamily("Calibri");
            FontSize = 14;
            FontStyle = FontStyle.Normal;

            InitializeComponent();
        }

        #endregion

        #region Properties

        public string Watermark
        {
            get => this.TextBox1.Watermark;
            set => TextBox1.Watermark = value;
        }

        public IEnumerable Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }

        public int TabIndex
        {
            get => _tabIndex;
            set => SetAndRaise(TabIndexProperty, ref _tabIndex, value);
        }

        public bool Disabled
        {
            get => _disabled;
            set => SetAndRaise(DisabledProperty, ref _disabled, value);
        }

        public IEnumerable FilteredItems
        {
            get => _filteredItems;
            set => SetAndRaise(FilteredItemsProperty, ref _filteredItems, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndRaise(SelectedIndexProperty, ref _selectedIndex, value);
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
        }

        public string SelectedViewItem
        {
            get => _selectedViewItem;
            set => SetAndRaise(SelectedViewItemProperty, ref _selectedViewItem, value);
        }

        public bool IsOpen
        {
            get => _isOpen;
            set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
        }

        /// <summary>
        /// Будет ли подсвечиваться фильтр в результате поиска
        /// </summary>
        /// <remarks>Необходимо чтобы в колеекции элементы были типа FormatItemViewModel,
        /// а ItemTemplate состоял из 3 элементов этой вью модели (см. пример ParmaFullAddressView.xaml)</remarks>
        public bool IsFormatSearch
        {
            get => _isFormatSearch;
            set => SetAndRaise(IsFormatSearchProperty, ref _isFormatSearch, value);
        }

        public IDataTemplate ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public string FilterPath
        {
            get => GetValue(FilterPathProperty);
            set => SetValue(FilterPathProperty, value);
        }

        public double MaxHeightDropDown
        {
            get => GetValue(MaxHeightDropDownProperty);
            set => SetValue(MaxHeightDropDownProperty, value);
        }

        public bool IsTextBoxFocus
        {
            get => GetValue(IsTextBoxFocusProperty);
            set => SetValue(IsTextBoxFocusProperty, value);
        }

        public bool ClearButtonNotVisible
        {
            get => _clearButtonNotVisible;
            set
            {
                SetAndRaise(ClearButtonNotVisibleProperty, ref _clearButtonNotVisible, value);
                ClearButton.IsVisible = !value;
            }
        }

        public IBrush ArrowColor
        {
            get => GetValue(ArrowColorProperty);
            set => SetValue(ArrowColorProperty, value);
        }

        public IBrush ButtonColor
        {
            get => GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        #endregion

        #region Controls

        public ListBox ListBox1 => this.FindControl<ListBox>(nameof(ListBox1));
        public TextBox TextBox1 => this.FindControl<TextBox>(nameof(TextBox1));
        public Button ClearButton => this.FindControl<Button>(nameof(ClearButton));
        public Popup Popup1 { get; set; }

        /// <summary>
        /// <remarks>Не находится кнопка по nameof.</remarks>
        /// </summary>
        public ToggleButton ToggleButton => this.FindControl<ToggleButton>("ToggleButtonArrow");

        #endregion

        #region Commands

        public void ClearCommand()
        {
            SetTip(TextBox1, null);
            SelectedViewItem = string.Empty;
            Filter = string.Empty;
            ListBox1.SelectedIndex = -1;
            SelectedIndex = -1;
            SelectedItem = null;
            TextBox1.Focus();
            IsOpen = false;
        }

        #endregion

        #region PropertieChanges

        protected override void Filtering(string value)
        {
            base.Filtering(value);

            //TODO также добавил проверку на пустоту, надеюсь ничего не сломает
            if (Items == null || !Items.Cast<object>().Any())
            {
                return;
            }

            if (!string.IsNullOrEmpty(value))
            {
                IsOpen = true;
            }

            if (!string.IsNullOrEmpty(FilterPath))
            {
                FilteredItems = Items.Cast<object>().Where(
                    x => GetValueByStringProperty(x, FilterPath).ToLower().Contains(value.ToLower()));
            }
            else
            {
                FilteredItems = Items.Cast<string>().Where(x => x.ToLower().Contains(value.ToLower()));
            }

            FormatSearchResult(FilteredItems, value);
        }

        protected override void NoFiltering(string value)
        {
            base.NoFiltering(value);

            if (Items == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(value))
            {
                IsOpen = true;
            }

            FilteredItems = Items.Cast<object>();

            FormatSearchResult(FilteredItems, value);
        }

        protected override int GetCountItems()
        {
            return Items?.OfType<object>().Count() ?? 0;
        }

        private void ToolTipChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            SetTip(TextBox1, ToolTip);
        }

        private void DisabledChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            if ((bool)obj.NewValue)
            {
                TextBox1.IsReadOnly = true;
                TextBox1.IsEnabled = false;
                ToggleButton.IsEnabled = false;
            }
            else
            {
                TextBox1.IsReadOnly = false;
                TextBox1.IsEnabled = true;
                ToggleButton.IsEnabled = true;
            }
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

            if (e.NewValue is IItem item && item.Key == "-1")
            {
                //Это условие возникает когда нажато "Загрузить еще"
                SelectedItem = e.OldValue;
                return;
            }

            if (!string.IsNullOrEmpty(FilterPath))
            {
                SelectedViewItem = e.NewValue != null ? GetValueByStringProperty(e.NewValue, FilterPath) : string.Empty;
            }
            else
            {
                SelectedViewItem = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
            }
        }

        private void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            SetTip(TextBox1, null);

            FilteredItems = Items;
            //TODO: в SingleDictionaryValueSelector динамически подгружается данные. Т е обновляются Items. Из-за того что в TestComboBox Enumerable  а не ObservableCollection
            // нужно обновлять полностью массив, но при этом не нужно чтобы сбивался выбранный индекс
            if (SelectedItem != null) //иначе скидывает фильтр после фильтрации
                SelectedViewItem = string.Empty;
        }

        private void FilteredItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            ListBox1.IsVisible = e.NewValue != null &&
                                ((e.NewValue as IEnumerable) ?? throw new InvalidOperationException()).Cast<object>()
                                .Count() != 0;
        }

        private void FormatSearchResult(IEnumerable items, string filter)
        {
            //if (!IsFormatSearch)
            //{
            //    return;
            //}

            //foreach (var item in items)
            //{
            //    if (!(item is FormatItemViewModel formatItem)) continue;

            //    var firstIndex = formatItem.Value.ToUpper().IndexOf(filter.ToUpper(), StringComparison.Ordinal);

            //    if (firstIndex == 0)
            //    {
            //        formatItem.BeforeText = string.Empty;
            //        formatItem.FormatText = formatItem.Value.Substring(0, filter.Length);
            //        formatItem.AfterText = formatItem.Value.Substring(filter.Length);
            //    }
            //    else if (firstIndex == -1)
            //    {
            //        // TODO по идее сюда попадать не должно, это означает что поиск сработал криво и в результирующий список попали элементы,
            //        // которые не соответствуют поиску, думаю это связано с изменением комбобокса
            //        Log.Error("Лишний элемент в результате поиска: " + formatItem.Value);
            //    }
            //    else
            //    {
            //        formatItem.BeforeText = formatItem.Value.Substring(0, firstIndex);
            //        formatItem.FormatText = formatItem.Value.Substring(firstIndex, filter.Length);
            //        formatItem.AfterText = formatItem.Value.Substring(firstIndex + filter.Length);
            //    }
            //}
        }

        #endregion
    }
}
