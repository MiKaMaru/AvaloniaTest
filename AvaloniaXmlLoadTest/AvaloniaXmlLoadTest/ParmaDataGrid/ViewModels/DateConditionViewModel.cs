using System;
using System.Collections.Generic;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    ///  ViewModel для условия даты.
    /// </summary>
    public class DateConditionViewModel : AbstractConditionViewModel
    {
        /// <summary>
        /// Дата по умолчанию.
        /// </summary>
        private static readonly DateTime? _defaultValue = null;

        /// <summary>
        /// Список отображаемых операторов для данного условия.
        /// </summary>
        private static readonly IList<ConditionOperatorViewModel> _operatorItems = new ConditionOperatorViewModel[]
        {
            new ConditionOperatorViewModel(GridConditionOperator.IsEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsNotEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsEmpty),
            new ConditionOperatorViewModel(GridConditionOperator.IsNotEmpty),
            new ConditionOperatorViewModel(GridConditionOperator.IsLessThan),
            new ConditionOperatorViewModel(GridConditionOperator.IsLessThanOrEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsGreaterThanOrEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsGreaterThan)
        };

        /// <summary>
        /// Текущая дата.
        /// </summary>
        private DateTime? _date;

        /// <summary>
        /// Текущая дата.
        /// </summary>
        public DateTime? Date
        {
            get => _date;
            set => this.RaiseAndSetIfChanged(ref _date, value);
        }

        public override GridConditionOperator DefaultConditionOperator => GridConditionOperator.IsEqualTo;

        /// <summary>
        /// Список допустимых операторов для данного условия.
        /// </summary>
        public override IList<ConditionOperatorViewModel> OperatorItems => _operatorItems;

        public DateConditionViewModel(GridCondition condition)
            : base(condition)
        {
            Date = string.IsNullOrEmpty(_condition.Value) ? (DateTime?)null : DateTime.Parse(_condition.Value);
        }

        protected override void LoadData()
        {
            base.LoadData();

            Date = string.IsNullOrEmpty(_condition.Value) ? (DateTime?)null : DateTime.Parse(_condition.Value);
        }

        protected override void SaveData()
        {
            base.SaveData();

            _condition.Value = Date == null ? string.Empty : Date.ToString();
            _condition.IsEnable = _condition.IsEnable || Date != _defaultValue;
        }

        protected override void ResetData()
        {
            base.ResetData();

            _condition.Value = _defaultValue == null ? string.Empty : _defaultValue.ToString();
        }
    }
}
