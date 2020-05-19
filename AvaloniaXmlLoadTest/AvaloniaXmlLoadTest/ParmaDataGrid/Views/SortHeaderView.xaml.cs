using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Views
{
    public class SortHeaderView : ReactiveUserControl<SortHeaderViewModel>
    {
        public TextBlock NameTextBlock => this.FindControl<TextBlock>(nameof(NameTextBlock));
        public Path SortIcon => this.FindControl<Path>(nameof(SortIcon));

        public SortHeaderView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Name, x => x.NameTextBlock.Text).DisposeWith(disposables);
                this.ViewModel.WhenAnyValue(x => x.SortType).Subscribe(x =>
                {
                    SortIcon.Classes.Set(":sortnone", x == SortType.None);
                    SortIcon.Classes.Set(":sortasc", x == SortType.Asc);
                    SortIcon.Classes.Set(":sortdesc", x == SortType.Desc);
                }).DisposeWith(disposables);
                this.AddHandler(UserControl.PointerPressedEvent, (o, e) => ViewModel.ChangeSort()).DisposeWith(disposables);
            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
