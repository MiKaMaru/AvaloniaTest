﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Media;
using Avalonia.Collections;
using Avalonia.Interactivity;
using Avalonia.Data.Converters;

namespace AvaloniaDataGridTest
{

    public class Person : System.ComponentModel.INotifyDataErrorInfo, INotifyPropertyChanged
    {
        string _firstName;
        string _lastName;
        int _numerick = 0;
        bool _check = false;
        public void RunTheThing()
        {
            // Code for executing the command here.
            Console.WriteLine(Check ? "YES" : "NO");
            _firstName = "John";
            //Check = !(Check);
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetError(nameof(FirstName), "First Name Required");
                else
                    SetError(nameof(FirstName), null);

                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }

        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetError(nameof(LastName), "Last Name Required");
                else
                    SetError(nameof(LastName), null);

                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public bool Check
        {
            get => _check;
            set { _check = value; }
        }
   

        public int Numerick
        {
            get => _numerick;
            set { _numerick = value; }
        }

        Dictionary<string, List<string>> _errorLookup = new Dictionary<string, List<string>>();

        void SetError(string propertyName, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                if (_errorLookup.Remove(propertyName))
                    OnErrorsChanged(propertyName);
            }
            else
            {
                if (_errorLookup.TryGetValue(propertyName, out List<string> errorList))
                {
                    errorList.Clear();
                    errorList.Add(error);
                }
                else
                {
                    var errors = new List<string> { error };
                    _errorLookup.Add(propertyName, errors);
                }

                OnErrorsChanged(propertyName);
            }
        }

        public bool HasErrors => _errorLookup.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errorLookup.TryGetValue(propertyName, out List<string> errorList))
                return errorList;
            else
                return null;
        }
    }


    public class GDPValueConverter : Avalonia.Data.Converters.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int gdp)
            {
                if (gdp <= 5000)
                    return Brushes.Orange;
                else if (gdp <= 10000)
                    return Brushes.Yellow;
                else
                    return Brushes.LightGreen;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MainWindow : Window
    {
        private StackPanel stkTest;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var dg1 = this.FindControl<DataGrid>("dataGrid1");
            dg1.IsReadOnly = true;

            var collectionView1 = new Avalonia.Collections.DataGridCollectionView(Countries.All);
            //collectionView.GroupDescriptions.Add(new Avalonia.Collections.PathGroupDescription("Region"));

            dg1.Items = collectionView1;

            var dg2 = this.FindControl<DataGrid>("dataGridGrouping");
            dg2.IsReadOnly = true;

            var collectionView2 = new Avalonia.Collections.DataGridCollectionView(Countries.All);
            //collectionView2.GroupDescriptions.Add(new Avalonia.Collections.PathGroupDescription("Region"));

            dg2.Items = collectionView2;

            var dg3 = this.FindControl<DataGrid>("dataGridEdit");
            dg3.IsReadOnly = false;

            var items = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe" },
                new Person { FirstName = "Elizabeth", LastName = "Thomas" },
                new Person { FirstName = "Zack", LastName = "Ward" }
            };
            var collectionView3 = new Avalonia.Collections.DataGridCollectionView(items);

            dg3.Items = collectionView3;

            var addButton = this.FindControl<Button>("btnAdd");
            addButton.Click += (a, b) => collectionView3.AddNew();
            var detailsButton = this.FindControl<Button>("btnDetails");

        }

        //private void Clicker(object sender, RoutedEventArgs e)
        //{
        //    //var countsComboBox = this.FindControl<ComboBox>("cbCounts");
        //   // countsComboBox.Items += "1";
        //}

        
        //private void dataGrid_CellContentClick (object sender, DataGridCellEventArgs)
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
