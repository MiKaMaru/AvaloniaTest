using System.Collections.Generic;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Helpers
{
    public static class FilterOperatorHelper
    {
        private static readonly Dictionary<GridFilterOperator, string> _names = new Dictionary<GridFilterOperator, string>()
        {
            { GridFilterOperator.And, "И" },
            { GridFilterOperator.Or,  "Или" },
        };

        public static string GetName(GridFilterOperator filterOperator)
        {
            return _names.TryGetValue(filterOperator, out string result) ? result : string.Empty;
        }
    }
}
