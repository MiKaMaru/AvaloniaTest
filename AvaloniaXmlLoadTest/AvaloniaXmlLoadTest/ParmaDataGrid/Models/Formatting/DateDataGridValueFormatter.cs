
using System;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models.Formatting
{
    public class DateDataGridValueFormatter : DatetimeDataGridValueFormatter
    {
        public DateDataGridValueFormatter() : base("dd.MM.yyyy")
        {
        }

        public new static Type Type => typeof(OnlyDate);
    }

    public class OnlyDate
    {
    }
}