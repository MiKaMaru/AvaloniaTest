using System.Collections.Generic;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Helpers
{
    public static class ConditionOperatorHelper
    {
        private static readonly Dictionary<GridConditionOperator, string> _names = new Dictionary<GridConditionOperator, string>()
        {
            { GridConditionOperator.IsLessThan, "Меньше" },
            { GridConditionOperator.IsLessThanOrEqualTo,  "Меньше или равно"  },
            { GridConditionOperator.IsEqualTo, "Равно"  },
            { GridConditionOperator.IsNotEqualTo, "Не равно"  },
            { GridConditionOperator.IsGreaterThanOrEqualTo, "Больше или равно"},
            { GridConditionOperator.IsGreaterThan, "Больше"  },
            { GridConditionOperator.StartsWith, "Начинается с"  },
            { GridConditionOperator.EndsWith, "Оканчивается на" },
            { GridConditionOperator.Contains, "Содержит" },
            { GridConditionOperator.DoesNotContain, "Не содержит" },
            { GridConditionOperator.IsContainedIn, "Входит в" },
            { GridConditionOperator.IsNotContainedIn, "Не входит в" },
            { GridConditionOperator.IsEmpty, "Пусто" },
            { GridConditionOperator.IsNotEmpty, "Не пусто" }
        };

        public static string GetName(GridConditionOperator conditionOperator)
        {
            return _names.TryGetValue(conditionOperator, out string result) ? result : string.Empty;
        }
    }
}
