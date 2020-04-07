using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models
{
    /// <summary>
    /// Условие фильтра.
    /// </summary>
    public class GridCondition : ReactiveObject
    {
        /// <summary>
        /// Оператор.
        /// </summary>
        public GridConditionOperator Operator { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// С учетом регистра.
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Включено ли условие.
        /// </summary>
        [Reactive] public bool IsEnable { get; set; }

        public GridCondition()
        {
            Operator = GridConditionOperator.IsLessThan;
            Value = string.Empty;
            IsCaseSensitive = false;
            IsEnable = false;
        }

        /// <summary>
        /// Копирование значений свойств. 
        /// </summary>
        /// <param name="otherCondition"></param>
        /// <remarks>ICloneable не подходит, поскольку не нужно создавать новый объеект, чтобы не сбросились текущие биндинги</remarks>
        public void Copy(GridCondition otherCondition)
        {
            this.IsEnable = otherCondition.IsEnable;
            this.IsCaseSensitive = otherCondition.IsCaseSensitive;
            this.Operator = otherCondition.Operator;
            this.Value = otherCondition.Value;
        }

        /// <summary>
        /// Сброс
        /// </summary>
        public void ResetCondition()
        {
            Operator = GridConditionOperator.IsLessThan;
            Value = string.Empty;
            IsCaseSensitive = false;
            IsEnable = false;
        }
    }
}
