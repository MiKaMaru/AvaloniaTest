using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
    /// <summary>
    /// Колонка c CheckBox. Логика под наши нужды.
    /// </summary>
    public class ParmaDataGridCheckBoxColumn : DataGridCheckBoxColumn
    {
        private DataGrid _owningGrid;
        private readonly Dictionary<DataGridRow, IDisposable> _checkBoxSubsribes;
        private bool _isActiveCheckBoxSubsribes;

        public ParmaDataGridCheckBoxColumn()
        {
            _checkBoxSubsribes = new Dictionary<DataGridRow, IDisposable>();
            _isActiveCheckBoxSubsribes = true;

            // CheckBox в заголовке.
            CheckBox headerCheckBox = new CheckBox();
            Observable.FromEventPattern<RoutedEventArgs>(h => headerCheckBox.Click += h, h => headerCheckBox.Click -= h).Subscribe(x =>
            {
                if (OwningGrid == null)
                {
                    return;
                }
                // Времено отключаем "подписки" на изменение CheckBoxов в гриде.
                _isActiveCheckBoxSubsribes = false;
                foreach (var item in OwningGrid.Items)
                {
                    if (GetCellContent(item) is CheckBox checkBox)
                    {
                        checkBox.IsChecked = headerCheckBox.IsChecked;
                        //если выбираются все строки то они добавляются в SelectedItems
                        if(checkBox.IsChecked.HasValue)
                        {
                            if(checkBox.IsChecked.Value)
                                OwningGrid.SelectedItems.Add(item);
                            else if(OwningGrid.SelectedItems.Contains(item))
                            {
                                OwningGrid.SelectedItems.Remove(item);
                            }
                        }
                    }
                }
                _isActiveCheckBoxSubsribes = true;
            });

            Header = headerCheckBox;
        }

        protected override IControl GenerateElement(DataGridCell cell, object dataItem)
        {
            if (OwningGrid != null && _owningGrid == null)
            {
                _owningGrid = OwningGrid;
                _owningGrid.Columns.CollectionChanged += OwningColumnsCollectionChanged;
                _owningGrid.CellPointerPressed += OwningGridCellPointerPressed;
                _owningGrid.LoadingRow += OwningGridLoadingRow;
                _owningGrid.UnloadingRow += OwningGridUnloadingRow;
            }

            return base.GenerateElement(cell, dataItem);
        }

        private void OwningColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Если наша колонка удалилась, значит отписываемся от всякой фигни.
            if (e.Action == NotifyCollectionChangedAction.Remove &&
                e.OldItems.Contains(this) &&
                _owningGrid != null)
            {
                _owningGrid.Columns.CollectionChanged -= OwningColumnsCollectionChanged;
                _owningGrid.CellPointerPressed -= OwningGridCellPointerPressed;
                _owningGrid.LoadingRow -= OwningGridLoadingRow;
                _owningGrid.UnloadingRow -= OwningGridUnloadingRow;
            }
        }

        private async void OwningGridCellPointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
        {
            if (OwningGrid == null)
            {
                return;
            }

            // Если колонка наша, то тупо переключаем чекбоксы.
            if (e.Column == this)
            {
                if (e.Cell.Content is CheckBox checkBox)
                {
                    checkBox.IsChecked = checkBox.IsChecked.HasValue ? !checkBox.IsChecked.Value : true;
                    //архи сложная логика по поиску итема принадлежащего строке
                    if (checkBox.IsChecked.HasValue)
                    {
                        foreach (var item in OwningGrid.Items)
                        {
                            if (GetCellContent(item) is CheckBox itemCheckBox && itemCheckBox == checkBox)
                            {
                                if (checkBox.IsChecked.Value)
                                    OwningGrid.SelectedItems.Add(item);
                                else if (OwningGrid.SelectedItems.Contains(item))
                                {
                                    OwningGrid.SelectedItems.Remove(item);
                                }
                                break; //нужный итем найден
                            }
                        }
                    }
                }
            }
            // Если колонка не наша, ориентируемся на выделенные строки.
            else
            {
                // На данный момент OwningGrid.SelectedItems содержит немного кривый данные, т.к. еще не прошла обработка выделенных элементов.
                // Так что тупо откладываем это.
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var item in OwningGrid.Items)
                    {
                        if (GetCellContent(item) is CheckBox checkBox)
                        {
                            checkBox.IsChecked = OwningGrid.SelectedItems.Contains(item);
                        }
                    }
                });
            }
        }

        private void OwningGridLoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (OwningGrid == null)
            {
                return;
            }

            if (GetCellContent(e.Row) is CheckBox checkBox)
            {
                _checkBoxSubsribes[e.Row] = checkBox.WhenAnyValue(x => x.IsChecked).Subscribe(x =>
                {
                    if (!_isActiveCheckBoxSubsribes)
                    {
                        return;
                    }

                    if (Header is CheckBox headerCheckBox)
                    {
                        headerCheckBox.IsChecked = OwningGrid.Items.Cast<object>()
                            .All(item => GetCellContent(item) is CheckBox checkBox2 ? checkBox2.IsChecked.HasValue && checkBox2.IsChecked.Value : false);
                    }
                });
            }
        }

        private void OwningGridUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (_checkBoxSubsribes.TryGetValue(e.Row, out IDisposable subscribe))
            {
                subscribe.Dispose();
                _checkBoxSubsribes.Remove(e.Row);
            }
        }
    }
}