using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaXmlLoadTest.DataGridDomain.Interfaces;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
    /// <summary>
    /// DataGrid, основанный на родном.
    /// </summary>
    public class ParmaDataGrid : DataGrid
    {

        /// <summary>
        /// Запретить колонкам менять свою позицию
        /// Костыль.
		/// </summary>
        public void LockColumnsReorder(params int[] indexes)
        {
            ColumnReordered += (sender, args) =>
            {
                if (indexes == null)
                    return;

                for (int i = 0; i < indexes.Length; i++)
                    Columns[i].DisplayIndex = i;
            };
        }

        /// <summary>
        /// Костыль для правильного отображения полосы прокрутки.
        /// После первоначального отображения компонента
        /// возможна неверная отрисовка либо отсутствие полосы прокрутки.
        /// </summary>
        /// <param name="dg">Грид</param>
        public void FixScroll(DataGrid dg)
        {
            try
            {
                var items = dg.Items.OfType<object>().ToArray();
                var column = dg.Columns.First();
                dg.ScrollIntoView(items.Last(), column);
                dg.ScrollIntoView(items.First(), column);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// Логика выбора элементов в таблице.
        /// </summary>
        /// <param name="disposable"><inheritdoc cref="CompositeDisposable"/></param>
        public void ChooseLogic(CompositeDisposable disposable)
        {
            // todo баг при выделении с помощью Control по строке и CheckBox

            // todo баг при снятии выделения с помощью Shift
            /*
             * нажать на строку X
             * зажать Shift
             * нажать на строку Y
             * нажать на строку X
             * отжать Shift
             */

            // в ближайшее время доделаю (Гайдамак)

//            // нажатие на CheckBox
//            Observable.FromEventPattern<DataGridCellPointerPressedEventArgs>(
//                    h => CellPointerPressed += h,
//                    h => CellPointerPressed -= h)
//                .Where(e =>
//                    e.EventArgs.PointerPressedEventArgs.MouseButton == MouseButton.Left &&
//                    e.EventArgs.Column != null &&
//                    e.EventArgs.Column.IsReadOnly &&
//                    e.EventArgs.Cell?.Content is CheckBox)
//                .Select(e => e.EventArgs)
//                .Subscribe(e =>
//                {
////                    Console.WriteLine("CheckBox"); // del
//                    var checkBox = (CheckBox) e.Cell.Content;
//
//                    // нажат Shift
//                    if (e.PointerPressedEventArgs.InputModifiers ==
//                        (e.PointerPressedEventArgs.InputModifiers | InputModifiers.Shift))
//                        checkBox.IsChecked = true;
//                    else
//                        checkBox.IsChecked = !checkBox.IsChecked;
//
//                    calculate?.Invoke();
//                }).DisposeWith(disposable);

            // нажатие не на CheckBox
            Observable.FromEventPattern<DataGridCellPointerPressedEventArgs>(
                    h => CellPointerPressed += h,
                    h => CellPointerPressed -= h)
                .Where(e =>
                    e.EventArgs.PointerPressedEventArgs.MouseButton == MouseButton.Left &&
                    e.EventArgs.Row.DataContext is IRecordForGrid &&
                    !(e.EventArgs.Cell.Content is CheckBox))
                .Select(e => e.EventArgs)
                .Subscribe(e =>
                {
//                    Console.WriteLine($"Cell = {e.Row.GetIndex()}; Selected = {SelectedIndex}"); // del
					var record = (IRecordForGrid) e.Row.DataContext;

                    // нажат Control
                    if (e.PointerPressedEventArgs.InputModifiers ==
                        (e.PointerPressedEventArgs.InputModifiers | InputModifiers.Control))
                    {
                        record.IsChecked = !record.IsChecked;
                    }
                    // нажат Shift
                    else if (e.PointerPressedEventArgs.InputModifiers ==
                             (e.PointerPressedEventArgs.InputModifiers | InputModifiers.Shift))
                    {
//                        if (e.Row.GetIndex() == SelectedIndex)
//                            SingleChoose(record, true);
//                        else
                            record.IsChecked = true;
                    }
                    else
                    {
                        SingleChoose(record, !record.IsChecked);
                    }

                }).DisposeWith(disposable);

            // выбор нескольких элементов (вызывается после нажатия)
            Observable.FromEventPattern<SelectionChangedEventArgs>(
                    h => SelectionChanged += h,
                    h => SelectionChanged -= h)
                .Select(e => e.Sender as DataGrid)
                .Where(dg => dg.SelectedItems.Count > 1)
                .Subscribe(dg =>
                {
//                    Console.WriteLine("SelectionChanged"); // del
					foreach (var item in dg.Items.Cast<IRecordForGrid>())
                        item.IsChecked = false;

                    foreach (var item in dg.SelectedItems.Cast<IRecordForGrid>())
                        item.IsChecked = true;

                }).DisposeWith(disposable);

            void SingleChoose(IRecordForGrid record, bool isChecked)
            {
                record.IsChecked = isChecked;
                foreach (var item in Items)
                    if (item != record)
                        ((IRecordForGrid) item).IsChecked = false;
            }
        }
    }
}
