using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// ViewModel для заголовка колонки с фильтром.
    /// </summary>
    public abstract class AbstractFilterHeaderViewModel : HeaderViewModel
    {
        /// <summary>
        /// Значение логического оператора.
        /// </summary>
        private const GridFilterOperator DEFAULT_FILTER_OPERATOR = GridFilterOperator.And;

        /// <summary>
        /// Список отображаемых значений логического оператора.
        /// </summary>
        private static readonly IList<FilterOperationViewModel> _filterOperatorItems = new FilterOperationViewModel[]
        {
            new FilterOperationViewModel(GridFilterOperator.And),
            new FilterOperationViewModel(GridFilterOperator.Or)
        };

        private readonly GridFilter _filter;

        /// <summary>
        /// Индекс логически опрератор.
        /// </summary>
        private int _filterOperationItemIndex;

        /// <summary>
        /// ViewModel первого условия.
        /// </summary>
        public abstract AbstractConditionViewModel FirstConditionViewModel { get; }

        /// <summary>
        /// ViewModel последнего( ¯\_(ツ)_/¯ ) условия.
        /// </summary>
        public abstract AbstractConditionViewModel LastConditionViewModel { get; }

        /// <summary>
        /// Индекс логически опрератор.
        /// </summary>
        public int FilterOperatiorItemIndex
        {
            get => _filterOperationItemIndex;
            set => this.RaiseAndSetIfChanged(ref _filterOperationItemIndex, value);
        }

        /// <summary>
        /// Список отображаемых значений логического оператора.
        /// </summary>
        public IList<FilterOperationViewModel> FilterOperatorItems => _filterOperatorItems;

        [Reactive] public bool IsActive { get; set; }

        /// <summary>
        /// Команда "Сбросить".
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }

        /// <summary>
        /// Команда "Применить".
        /// </summary>
        public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

        /// <summary>
        /// Команда "Очистить".
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        public Subject<GridFilter> ApplyFilter { get; private set; }

        protected AbstractFilterHeaderViewModel(GridFilter filter)
        {
            _filter = filter;

            FilterOperatiorItemIndex = FilterOperatorItems.IndexOf(FilterOperatorItems.FirstOrDefault(x => x.Operator == _filter.Operator));

            this.WhenAnyValue(x => x.FirstConditionViewModel.IsActive, x => x.LastConditionViewModel.IsActive)
                .Select(x => x.Item1 || x.Item2)
                .BindTo(this, x => x.IsActive);

            ResetCommand = ReactiveCommand.Create(() => { Reset(); });
            ApplyCommand = ReactiveCommand.Create(() => { Apply(); });
            ClearCommand = ReactiveCommand.Create(() => { Clear(); });

            ApplyFilter = new Subject<GridFilter>();
        }

        /// <summary>
        /// Сбросить. ВЫполняется при показе.
        /// </summary>
        public void Reset()
        {
            FilterOperatiorItemIndex = FilterOperatorItems.IndexOf(FilterOperatorItems.FirstOrDefault(x => x.Operator == _filter.Operator));
            FirstConditionViewModel.Reset();
            LastConditionViewModel.Reset();
        }

        /// <summary>
        /// Применить.
        /// </summary>
        public void Apply()
        {
            _filter.Operator = FilterOperatiorItemIndex >= 0 && FilterOperatiorItemIndex < FilterOperatorItems.Count
                               ? FilterOperatorItems[FilterOperatiorItemIndex].Operator
                               : DEFAULT_FILTER_OPERATOR;
            FirstConditionViewModel.Apply();
            LastConditionViewModel.Apply();
            ApplyFilter.OnNext(_filter);
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            FilterOperatiorItemIndex = FilterOperatorItems.IndexOf(FilterOperatorItems.FirstOrDefault(x => x.Operator == DEFAULT_FILTER_OPERATOR));
            FirstConditionViewModel.Clear();
            LastConditionViewModel.Clear();
            this.RaisePropertyChanged(nameof(IsActive));
            ApplyFilter.OnNext(_filter);
        }
    }
}