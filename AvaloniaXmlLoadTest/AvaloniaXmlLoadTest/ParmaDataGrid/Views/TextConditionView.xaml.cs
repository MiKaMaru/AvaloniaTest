using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Views
{
    public class TextConditionView : ReactiveUserControl<TextConditionViewModel>
    {
        public ComboBox OperationComboBox => this.FindControl<ComboBox>(nameof(OperationComboBox));
        public TextBox TextTextBox => this.FindControl<TextBox>(nameof(TextTextBox));
        public ToggleButton IsCaseSensitiveToggleButton => this.FindControl<ToggleButton>(nameof(IsCaseSensitiveToggleButton));

        public TextConditionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.OperatorItems, x => x.OperationComboBox.Items).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.ConditionOperatorItemIndex, x => x.OperationComboBox.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Text, x => x.TextTextBox.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.IsEnableControl, x => x.TextTextBox.IsEnabled).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.IsCaseSensitive, x => x.IsCaseSensitiveToggleButton.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.IsEnableControl, x => x.IsCaseSensitiveToggleButton.IsEnabled).DisposeWith(disposables);
            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
