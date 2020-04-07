﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Portable.Xaml;
using System.Collections.Generic;
using Avalonia.Controls.Presenters;
using AvaloniaXmlLoadTest.TestControls;
using DynamicData.Binding;
using AvaloniaXmlLoadTest.Interfaces;
using AvaloniaXmlLoadTest.Utils;
using System.Text;
using AvaloniaXmlLoadTest.ParmaDataGrid;

namespace AvaloniaXmlLoadTest
{
    public class MainWindow : Window
    {
        private TestComboBox tcbList => this.FindControl<TestComboBox>("tcbList");
        private TestTreeBox ttbTree => this.FindControl<TestTreeBox>("ttbTree");
        private Button btnShowWindow => this.FindControl<Button>("btnShowWindow");
        private Button btnShowWindow9000 => this.FindControl<Button>("btnShowWindow9000");
        private Button btnAddDynamic => this.FindControl<Button>("btnAddDynamic");
        private AvaloniaXmlLoadTest.ParmaDataGrid.ParmaDataGrid dg2 => this.FindControl<AvaloniaXmlLoadTest.ParmaDataGrid.ParmaDataGrid>("dataGrid2");
        private StackPanel stk01 => this.FindControl<StackPanel>("stk01");

        private List<string> exampleStrings;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeStrings();
            tcbList.Items = exampleStrings;
            ttbTree.Items = new ObservableCollectionExtended<IHierarchicalItem>(InitializeTree());
            ttbTree.SelectedItem = ttbTree.Items[0].Children[0];
            btnShowWindow.Click += BtnShowWindow_Click;
            btnShowWindow9000.Click += BtnShowWindow9000_Click;
            InitDataGrid();
            btnAddDynamic.Click += BtnAddDynamic_Click;
        }

        private void BtnAddDynamic_Click(object sender, RoutedEventArgs e)
        {
            DynamicControlAdd();
        }

        private void InitDataGrid()
        {
            var dg1 = this.FindControl<DataGrid>("dataGrid1");
            dg1.IsReadOnly = true;
            var collectionView1 = new Avalonia.Collections.DataGridCollectionView(Countries.All, dg1);
            //collectionView.GroupDescriptions.Add(new Avalonia.Collections.PathGroupDescription("Region"));
            dg1.Items = collectionView1;

            dg2.IsReadOnly = true;
            var collectionView2 = new Avalonia.Collections.DataGridCollectionView(Countries.All, dg2);
            //collectionView.GroupDescriptions.Add(new Avalonia.Collections.PathGroupDescription("Region"));
            dg2.Items = collectionView2;
        }

        private void BtnShowWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = new ExampleWindow();
            window.Show();
        }
        private void BtnShowWindow9000_Click(object sender, RoutedEventArgs e)
        {
            var window = new ExampleWindow();
            window.Height = 9000;
            window.Show();
        }

        private ObservableCollectionExtended<IHierarchicalItem> InitializeTree()
        {
            var items = new ObservableCollectionExtended<IHierarchicalItem>();
            for (int i = 1; i < 40; i++)
            {
                var hIVM = new HierarchicalItemViewModel(i.ToString(), "l_example" + i.ToString(), i.ToString())
                { 
                    IsExpanded = true
                };
                
                for (int j = i * 10; j < i * 10 + 10; j++)
                {

                    var _hIVM = new HierarchicalItemViewModel(j.ToString(), "example" + j.ToString(), i.ToString())
                    {
                        IsSelectionItem = true
                    };
                    hIVM.Children.Add(_hIVM);
                }

                items.Add(hIVM);
            }
            return items;
        }

        private void InitializeStrings()
        {
            exampleStrings = new List<string>();
            for (int i = 0; i < 88; i++)
            {
                exampleStrings.Add("example " + i);
            }
        }

        private void BtnShowExampleWindow_Click(object sender, RoutedEventArgs e)
        {
            var exampleWindow = new ExampleWindow();
            exampleWindow.Show();
        }
        private void DynamicControlAdd()
        {
            var xamlLoader = @"<ToolTip xmlns='https://github.com/avaloniaui' Width='25'
            Height = '25' Tip ='Example Error ToolTip. Sometimes disappears when re-hovering or on second hover'/>";
            var loader = new AvaloniaXamlLoader();
            var tb = (ToolTip)loader.Load(xamlLoader);
            stk01.Children.Add(tb);
        }

    }
}
