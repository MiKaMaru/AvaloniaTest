using System.Collections.Generic;
using System.Reactive;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    ///  ViewModel для текстового условия.
    /// </summary>
    public class TextConditionViewModel : AbstractConditionViewModel
    {
        /// <summary>
        /// Текст по умолчанию.
        /// </summary>
        private static readonly string _defaultValue = string.Empty;

        /// <summary>
        /// Учитывать регистр по умолчанию.
        /// </summary>
        private const bool _defaulIsCaseSensitive = false;

        /// <summary>
        /// Список отображаемых операторов для данного условия.
        /// </summary>
        private static readonly ConditionOperatorViewModel[] _operatorItems = new ConditionOperatorViewModel[]
        {
            new ConditionOperatorViewModel(GridConditionOperator.IsEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsNotEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.StartsWith),
            new ConditionOperatorViewModel(GridConditionOperator.EndsWith),
            new ConditionOperatorViewModel(GridConditionOperator.Contains),
            new ConditionOperatorViewModel(GridConditionOperator.DoesNotContain),
            new ConditionOperatorViewModel(GridConditionOperator.IsContainedIn),
            new ConditionOperatorViewModel(GridConditionOperator.IsNotContainedIn),
            new ConditionOperatorViewModel(GridConditionOperator.IsEmpty),
            new ConditionOperatorViewModel(GridConditionOperator.IsNotEmpty),
            new ConditionOperatorViewModel(GridConditionOperator.IsLessThan),
            new ConditionOperatorViewModel(GridConditionOperator.IsLessThanOrEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsGreaterThanOrEqualTo),
            new ConditionOperatorViewModel(GridConditionOperator.IsGreaterThan)
        };

        /// <summary>
        /// Текущий текст.
        /// </summary>
        private string _text;

        /// <summary>
        /// Текущее учитывание регистра.
        /// </summary>
        private bool _isCaseSensitive;

        /// <summary>
        /// Текущий текст.
        /// </summary>
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        /// <summary>
        /// Текущее учитывание регистра.
        /// </summary>
        public bool IsCaseSensitive
        {
            get => _isCaseSensitive;
            set => this.RaiseAndSetIfChanged(ref _isCaseSensitive, value);
        }

        public override GridConditionOperator DefaultConditionOperator => GridConditionOperator.Contains;

        /// <summary>
        /// Список отображаемых операторов для данного условия.
        /// </summary>
        public override IList<ConditionOperatorViewModel> OperatorItems => _operatorItems;

        public TextConditionViewModel(GridCondition condition)
            : base(condition)
        {
            Text = _defaultValue;
            IsCaseSensitive = _defaulIsCaseSensitive;
        }

        protected override void LoadData()
        {
            base.LoadData();

            Text = _condition.Value;
            IsCaseSensitive = _condition.IsCaseSensitive;
        }

        protected override void SaveData()
        {
            base.SaveData();

            _condition.Value = Text;
            _condition.IsCaseSensitive = IsCaseSensitive;
            _condition.IsEnable = _condition.IsEnable || !string.IsNullOrEmpty(Text);
        }

        protected override void ResetData()
        {
            base.ResetData();

            _condition.Value = _defaultValue;
            _condition.IsCaseSensitive = _defaulIsCaseSensitive;
        }
    }
}
