using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Views
{
    /// <summary>
    /// Заголовок с текстом и кнопкой фильтров для колонки.
    /// </summary>
    public class FilterHeaderView : ReactiveUserControl<AbstractFilterHeaderViewModel>
    {
        public TextBlock NameTextBlock => this.FindControl<TextBlock>(nameof(NameTextBlock));
        public Button FilterButton => this.FindControl<Button>(nameof(FilterButton));
        public Image FilterImage => this.FindControl<Image>(nameof(FilterImage));
        public Popup FilterPopup => this.FindControl<Popup>(nameof(FilterPopup));
        public Button CloseButton => this.FindControl<Button>(nameof(CloseButton));
        public ContentControl FirstConditionContentControl => this.FindControl<ContentControl>(nameof(FirstConditionContentControl));
        public ComboBox OperatorComboBox => this.FindControl<ComboBox>(nameof(OperatorComboBox));
        public ContentControl LastConditionContentControl => this.FindControl<ContentControl>(nameof(LastConditionContentControl));
        public Button ApplyButton => this.FindControl<Button>(nameof(ApplyButton));
        public Button ClearButton => this.FindControl<Button>(nameof(ClearButton));

        public FilterHeaderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, x => x.Name, x => x.NameTextBlock.Text).DisposeWith(disposables);
                FilterButton.AddHandler(Button.ClickEvent, (o, e) =>
                    {
                        if (!FilterPopup.IsOpen)
                        {
                            ViewModel.Reset();
                            FilterPopup.Open();
                        }
                    })
                    .DisposeWith(disposables);
                this.WhenAnyValue(x => x.ViewModel.IsActive)
                    .Subscribe(x =>
                    {
                        FilterImage.Classes.Set(":active", x);
                        FilterImage.Classes.Set(":inactive", !x);
                    })
                    .DisposeWith(disposables);
                CloseButton.AddHandler(Button.ClickEvent, (o, e) =>
                    {
                        FilterPopup.Close();
                        e.Handled = true;
                    })
                    .DisposeWith(disposables);
                this.Bind(ViewModel, x => x.FirstConditionViewModel, x => x.FirstConditionContentControl.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.FilterOperatorItems, x => x.OperatorComboBox.Items).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.FilterOperatiorItemIndex, x => x.OperatorComboBox.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.LastConditionViewModel, x => x.LastConditionContentControl.Content).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.ApplyCommand, x => x.ApplyButton).DisposeWith(disposables).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.ClearCommand, x => x.ClearButton).DisposeWith(disposables).DisposeWith(disposables);

                if (Application.Current.Windows.Any())
                {
                    Window window = Application.Current.Windows.Last();

                    window.AddHandler(PointerPressedEvent, (o, e) =>
                        {
                            if (FilterPopup.IsOpen && !IsParent(e.Source, FilterPopup.PopupRoot))
                            {
                                FilterPopup.Close();
                            }
                        }, handledEventsToo: true)
                    .DisposeWith(disposables);

                    Observable.FromEventPattern(h => window.Deactivated += h, h => window.Deactivated -= h)
                    .Subscribe(x =>
                        {
                            if (FilterPopup.IsOpen)
                            {
                                FilterPopup.Close();
                            }
                        })
                    .DisposeWith(disposables);
                }
            });

            AvaloniaXamlLoader.Load(this);
        }

        private bool IsParent(IInteractive interactive, IInteractive interactiveParent)
        {
            if (interactive == null)
            {
                return false;
            }
            else if (interactive.InteractiveParent == interactiveParent)
            {
                return true;
            }
            else
            {
                return IsParent(interactive.InteractiveParent, interactiveParent);
            }
        }
    }
}
