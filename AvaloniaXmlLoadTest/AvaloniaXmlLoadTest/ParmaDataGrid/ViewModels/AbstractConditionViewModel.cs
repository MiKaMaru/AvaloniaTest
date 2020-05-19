using Avalonia.Diagnostics.ViewModels;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using AvaloniaXmlLoadTest.Utils;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// Базовая ViewModel для условия фильтра.
    /// </summary>
    public abstract class AbstractConditionViewModel : ViewModelBase
    {
        /// <summary>
        /// Операторы при которых условие не активно.
        /// </summary>
        private static readonly HashSet<GridConditionOperator> _inactiveConditionOperators = new HashSet<GridConditionOperator>()
        {
            GridConditionOperator.IsLessThan,
            GridConditionOperator.IsLessThanOrEqualTo,
            GridConditionOperator.IsEqualTo,
            GridConditionOperator.IsNotEqualTo,
            GridConditionOperator.IsGreaterThanOrEqualTo,
            GridConditionOperator.IsGreaterThan,
            GridConditionOperator.StartsWith,
            GridConditionOperator.EndsWith,
            GridConditionOperator.Contains,
            GridConditionOperator.DoesNotContain,
            GridConditionOperator.IsContainedIn,
            GridConditionOperator.IsNotContainedIn
        };

        /// <summary>
        /// Операторы при которых доп. поля условия неактивны.
        /// </summary>
        private static readonly HashSet<GridConditionOperator> _disableControlConditionOperators = new HashSet<GridConditionOperator>()
        {
            GridConditionOperator.IsEmpty,
            GridConditionOperator.IsNotEmpty
        };

        /// <summary>
        /// Условие.
        /// </summary>
        protected GridCondition _condition;

        /// <summary>
        /// Текущий индекс оператора условия.
        /// </summary>
        private int _conditionOperatorItemIndex;

        /// <summary>
        /// Индекс оператора условия.
        /// </summary>
        public int ConditionOperatorItemIndex
        {
            get => _conditionOperatorItemIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _conditionOperatorItemIndex, value);
                this.RaisePropertyChanged(nameof(IsEnableControl));
            }
        }

        /// <summary>
        /// Оператор по умолчанию.
        /// </summary>
        public virtual GridConditionOperator DefaultConditionOperator => GridConditionOperator.Contains;

        /// <summary>
        /// Активено ли условие.
        /// </summary>
        [Reactive] public virtual bool IsActive { get; set; }

        /// <summary>
        /// Включены ли доп. контролы.
        /// </summary>
        public bool IsEnableControl => (OperatorItems != null && ConditionOperatorItemIndex >= 0 && ConditionOperatorItemIndex < OperatorItems.Count) &&
                                       !_disableControlConditionOperators.Contains(OperatorItems[ConditionOperatorItemIndex].Operator);


        /// <summary>
        /// Список отображаемых операторов для данного условия.
        /// </summary>
        public abstract IList<ConditionOperatorViewModel> OperatorItems { get; }

        public AbstractConditionViewModel(GridCondition condition)
        {
            _condition = condition;
            _conditionOperatorItemIndex = OperatorItems.IndexOf(OperatorItems.FirstOrDefault(x => x.Operator == condition.Operator));
            this.WhenAnyValue(x => x._condition.IsEnable).BindTo(this, x => x.IsActive);
        }

        /// <summary>
        /// Сброс. Выполняется при показе.
        /// </summary>
        public void Reset()
        {
            LoadData();
        }

        /// <summary>
        /// Применить.
        /// </summary>
        public void Apply()
        {
            SaveData();
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            ResetData();
            LoadData();
        }

        protected virtual void LoadData()
        {
            ConditionOperatorItemIndex = OperatorItems.IndexOf(OperatorItems.FirstOrDefault(x => x.Operator == _condition.Operator));
        }

        protected virtual void SaveData()
        {
            _condition.Operator = ConditionOperatorItemIndex >= 0 && ConditionOperatorItemIndex < OperatorItems.Count
                                  ? OperatorItems[ConditionOperatorItemIndex].Operator
                                  : DefaultConditionOperator;
            _condition.IsEnable = !_inactiveConditionOperators.Contains(_condition.Operator);
        }

        protected virtual void ResetData()
        {
            _condition.Operator = DefaultConditionOperator;
            _condition.IsEnable = false;
        }
    }
}