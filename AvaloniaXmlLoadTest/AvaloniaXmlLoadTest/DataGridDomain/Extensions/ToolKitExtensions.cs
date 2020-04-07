using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;


namespace AvaloniaXmlLoadTest.DataGridDomain.Extensions
{
    public static class ToolKitExtensions
    {
        /// <summary>
        /// Получения приватного текстбокса у DatePicker, чтобы была возможность рулить позицией каретки
        /// </summary>
        /// <param name="datePicker"></param>
        /// <returns></returns>
        public static TextBox GetTextBoxInDatePicker(this DatePicker datePicker)
        {
            var datePickerType = typeof(DatePicker);
            var datePickerTextBox = datePickerType.GetField("_textBox", BindingFlags.NonPublic | BindingFlags.Instance);
            return (TextBox)datePickerTextBox.GetValue(datePicker);
        }

        public static TextPresenter GetTextPresenterInTextBox(this TextBox textBox)
        {
            var textBoxType = typeof(TextBox);
            var textPresenterTextBox = textBoxType.GetField("_presenter", BindingFlags.NonPublic | BindingFlags.Instance);
            return (TextPresenter)textPresenterTextBox.GetValue(textBox);
        }
    }
}
