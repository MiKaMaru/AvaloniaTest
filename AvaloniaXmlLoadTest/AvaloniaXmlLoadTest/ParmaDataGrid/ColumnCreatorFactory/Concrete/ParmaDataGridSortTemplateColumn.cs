using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels;
using AvaloniaXmlLoadTest.ParmaDataGrid.Views;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
    /// <summary>
    /// Колонка для <see cref="ParmaDataGrid"/>, в которой может быть любой контрол, при этом можно запускать сортировку />
    /// </summary>
    public class ParmaDataGridSortTemplateColumn : DataGridTemplateColumn, IParmaDataGridSortColumn
    {
        public static readonly DirectProperty<ParmaDataGridSortTemplateColumn, string> NameProperty =
            AvaloniaProperty.RegisterDirect<ParmaDataGridSortTemplateColumn, string>(
                nameof(Name),
                (o) => o.Name,
                (o, v) => o.Name = v,
                null);

        public static readonly DirectProperty<ParmaDataGridSortTemplateColumn, SortType> SortTypeProperty =
            AvaloniaProperty.RegisterDirect<ParmaDataGridSortTemplateColumn, SortType>(
                nameof(SortType),
                (o) => o.SortType,
                (o, v) => o.SortType = v,
                SortType.None);
        
        public static readonly DirectProperty<ParmaDataGridSortTemplateColumn, object> ColumnIdProperty =
            AvaloniaProperty.RegisterDirect<ParmaDataGridSortTemplateColumn, object>(
                nameof(ColumnId),
                (o) => o.ColumnId,
                (o, v) => o.ColumnId = v,
                null);

        private string _name;
        private SortType _sortType;
        private object _сolumnId;
        private bool _frozenSortState = false;
        private readonly Subject<Unit> _changeSortSubject;

        /// <inheritdoc cref="Name"/>
        public string Name
        {
            get => _name;
            set => SetAndRaise(NameProperty, ref _name, value);
        }

        /// <inheritdoc cref="SortType"/>
        public SortType SortType
        {
            get => _sortType;
            set => SetAndRaise(SortTypeProperty, ref _sortType, value);
        }

        /// <inheritdoc cref="ColumnId"/>
        public object ColumnId
        {
            get => _сolumnId;
            set => SetAndRaise(ColumnIdProperty, ref _сolumnId, value);
        }

        /// <inheritdoc cref="ChangeSort"/>
        public IObservable<Unit> ChangeSort { get => _changeSortSubject; }

        public ParmaDataGridSortTemplateColumn(FuncDataTemplate funcDataTemplate)
        {
            CellTemplate = funcDataTemplate;

            _changeSortSubject = new Subject<Unit>();

            SortHeaderViewModel headerViewModel = new SortHeaderViewModel();

            this.WhenAnyValue(x => x.SortType).BindTo(headerViewModel, x => x.SortType);
            this.WhenAnyValue(x => x.Name).BindTo(headerViewModel, x => x.Name);
            this.WhenAnyValue(x => x.SortType).Subscribe(x =>
            {
                if (OwningGrid != null)
                {
                    // помечаем, что изменять текущую колонку нельзя
                    _frozenSortState = true;
                    var columns = OwningGrid.Columns.Select(y => y as ParmaDataGridSortTemplateColumn).Where(y => y != null && !y._frozenSortState);
                    foreach (var column in columns)
                    {
                        column.SortType = SortType.None;
                    }
                    _frozenSortState = false;
                    _changeSortSubject.OnNext(Unit.Default);
                }
            });
            headerViewModel.WhenAnyValue(x => x.SortType).BindTo(this, x => x.SortType);

            Header = new SortHeaderView { DataContext = headerViewModel };
        }
    }
}