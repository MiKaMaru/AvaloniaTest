using Avalonia.Diagnostics.ViewModels;
using AvaloniaXmlLoadTest.Utils;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <summary>
    /// ViewModel для заголовка колонки.
    /// </summary>
    public class HeaderViewModel : ViewModelBase
    {
        /// <summary>
        /// Имя колонки.
        /// </summary>
        [Reactive] public string Name { get; set; }
    }
}
