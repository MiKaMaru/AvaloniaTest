using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaXmlLoadTest.DataGridDomain.Extensions;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Views
{
    public class DateConditionView : ReactiveUserControl<DateConditionViewModel>
    {
        public ComboBox OperationComboBox => this.FindControl<ComboBox>(nameof(OperationComboBox));
        public DatePicker DateDatePicker => this.FindControl<DatePicker>(nameof(DateDatePicker));

        public DateConditionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.OperatorItems, x => x.OperationComboBox.Items).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.ConditionOperatorItemIndex, x => x.OperationComboBox.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Date, x => x.DateDatePicker.SelectedDate).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.IsEnableControl, x => x.DateDatePicker.IsEnabled).DisposeWith(disposables);
                this.WhenAnyValue(x => x.DateDatePicker.SelectedDate).Subscribe(x =>
                {
                    if (!x.HasValue)
                    {
                        TextBox textBox = DateDatePicker.GetTextBoxInDatePicker();
                        if (textBox != null)
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                });
            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
