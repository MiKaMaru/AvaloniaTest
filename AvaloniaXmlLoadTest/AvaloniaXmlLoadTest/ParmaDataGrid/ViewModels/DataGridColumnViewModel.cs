using Avalonia.Controls;
using Avalonia.Diagnostics.ViewModels;
using AvaloniaXmlLoadTest.DataGridDomain.Interfaces;
using AvaloniaXmlLoadTest.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    /// <inheritdoc cref="IDataGridColumnViewModel"/>
	public class DataGridColumnViewModel: ViewModelBase, IDataGridColumnViewModel
	{
        private bool _isVisible;
        private bool _isEnabled = true;
        private IEnumerable<string> _classes;

        public DataGridColumn DataGridColumn { get; set; }

        public Type Type { get; set; }

        public string Name { get; set; }
        
		public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (IsEnabled)
                    this.RaiseAndSetIfChanged(ref _isVisible, value);
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _isEnabled, value);
                if (!value)
                    IsVisible = true;
            }
        }

        public int BaseIndex { get; set; }

        public IEnumerable<string> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                DataGridColumn.CellStyleClasses = new Classes(value);
            }
        }

        public DataGridColumnViewModel(DataGridColumn column)
		{
            DataGridColumn = column;
            if (column is IParmaDataGridSortColumn sortColumn)
                Name = sortColumn.Name;
            else
			    Name = ((column.Header as ContentControl)?.DataContext as HeaderViewModel)?.Name;
			IsVisible = column.IsVisible;
            BaseIndex = column.BaseIndex;
		}

		public void Apply()
		{
            DataGridColumn.IsVisible = IsVisible;
		}
	}
}