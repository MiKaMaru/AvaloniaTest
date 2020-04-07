using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Views
{
    public class HeaderView : ReactiveUserControl<HeaderViewModel>
    {
        public TextBlock NameTextBlock => this.FindControl<TextBlock>(nameof(NameTextBlock));

        public HeaderView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Name, x => x.NameTextBlock.Text).DisposeWith(disposables);
            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
