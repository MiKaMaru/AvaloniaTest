using AvaloniaXmlLoadTest.ParmaDataGrid.Helpers;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// Оператор для условия.
    /// </summary>
    public class ConditionOperatorViewModel
    {
        /// <summary>
        /// Оператор.
        /// </summary>
        public GridConditionOperator Operator { get; private set; }

        /// <summary>
        /// Строковое имя оператора. 
        /// </summary>
        public string Name { get; private set; }

        public ConditionOperatorViewModel(GridConditionOperator op)
        {
            Operator = op;
            Name = ConditionOperatorHelper.GetName(op);
        }
    }
}
