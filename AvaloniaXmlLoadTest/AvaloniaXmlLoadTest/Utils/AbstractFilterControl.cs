using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.Utils
{
    /// <summary>
    /// Базовый класс для списков/деревьев с поиском. Вынес общую логику.
    /// </summary>
    public abstract class AbstractFilterControl : UserControl
    {
        public static readonly DirectProperty<AbstractFilterControl, AvaloniaList<FilterOption>> FilterOptionsProperty = AvaloniaProperty.RegisterDirect<AbstractFilterControl, AvaloniaList<FilterOption>>(
                nameof(FilterOptions),
                o => o.FilterOptions,
                (o, v) => o.FilterOptions = v);

        public static readonly DirectProperty<AbstractFilterControl, string> FilterProperty = AvaloniaProperty.RegisterDirect<AbstractFilterControl, string>(
                nameof(Filter),
                o => o.Filter,
                (o, v) => o.Filter = v);

        public static readonly DirectProperty<AbstractFilterControl, double> FilterPauseProperty = AvaloniaProperty.RegisterDirect<AbstractFilterControl, double>(
                nameof(FilterPause),
                o => o.FilterPause,
                (o, v) => o.FilterPause = v);

        public static readonly StyledProperty<string> EmptyTextProperty = AvaloniaProperty.Register<AbstractFilterControl, string>(nameof(EmptyText), defaultValue: "Выберите элемент");

        public static readonly DirectProperty<AbstractFilterControl, Action<string>> FilterChangeHandlerProperty = AvaloniaProperty.RegisterDirect<AbstractFilterControl, Action<string>>(
                nameof(FilterChangeHandler),
                o => o.FilterChangeHandler,
                (o, v) => o.FilterChangeHandler = v);

        private AvaloniaList<FilterOption> _filterOptions = new AvaloniaList<FilterOption>()
        {
            new FilterOption() { ItemsCount = 0, CharCount = 1 },
            new FilterOption() { ItemsCount = 100, CharCount = 3 }
        };
        private string _filter = null;
        private double _filterPause = 0.5;
        private Action<string> _filterChangeHandler;
        private IDisposable _filterPauseSubscribe = null;

        /// <summary>
        /// Настройка запуска фильтрации в зависимостои от количества элементов коллекции и длины значения фильтра.
        /// </summary>
        public AvaloniaList<FilterOption> FilterOptions
        {
            get => _filterOptions;
            set => SetAndRaise(FilterOptionsProperty, ref _filterOptions, value);
        }

        /// <summary>
        /// Значения фильтра
        /// </summary>
        public string Filter
        {
            get => _filter;
            set => SetAndRaise(FilterProperty, ref _filter, value);
        }

        /// <summary>
        /// Пауза перед поиском, пока вводятся символы.
        /// </summary>
        public double FilterPause
        {
            get => _filterPause;
            set => SetAndRaise(FilterPauseProperty, ref _filterPause, value);
        }

        /// <summary>
        /// Надпись при пустом текстбоксе
        /// </summary>
        public string EmptyText
        {
            get => GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        /// <summary>
        /// Где-то используется.
        /// </summary>
        public Action<string> FilterChangeHandler
        {
            get => _filterChangeHandler;
            set => SetAndRaise(FilterChangeHandlerProperty, ref _filterChangeHandler, value);
        }

        protected AbstractFilterControl()
        {
            this.WhenAnyValue(x => x.FilterPause).Subscribe(pause =>
            {
                _filterPauseSubscribe?.Dispose();
                _filterPauseSubscribe = this.WhenAnyValue(x => x.Filter).Throttle(TimeSpan.FromSeconds(pause)).Subscribe(async x => await Dispatcher.UIThread.InvokeAsync(() => FilterChanged(x)));
            });
        }

        /// <summary>
        /// Запускаем фильтр.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void Filtering(string value)
        {
        }

        /// <summary>
        /// Фильтр не запускаем.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void NoFiltering(string value)
        {
        }

        private void FilterChanged(string value)
        {
            FilterChangeHandler?.Invoke(value);

            if (value == null)
            {
                return;
            }

            int count = GetCountItems();
            int charCount = _filterOptions == null || _filterOptions.Count == 0 ? 1 : (_filterOptions?.LastOrDefault(x => count >= x.ItemsCount)?.CharCount ?? int.MaxValue);
            if (value.Length >= charCount)
            {
                Filtering(value);
            }
            else
            {
                NoFiltering(value);
            }
        }

        /// <summary>
        /// Количество элементов.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCountItems()
        {
            return 0;
        }
    }
}
