using AvaloniaXmlLoadTest.ParmaDataGrid.Helpers;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// Логичекая операция между условиями фильтра.
    /// </summary>
    public class FilterOperationViewModel
    {
        /// <summary>
        /// Операция.
        /// </summary>
        public GridFilterOperator Operator { get; private set; }

        /// <summary>
        /// Строковое имя операции. 
        /// </summary>
        public string Name { get; private set; }

        public FilterOperationViewModel(GridFilterOperator op)
        {
            Operator = op;
            Name = FilterOperatorHelper.GetName(op);
        }
    }
}
