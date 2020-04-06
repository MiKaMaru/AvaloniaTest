using Avalonia;
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

namespace AvaloniaXmlLoadTest
{
    public class MainWindow : Window
    {
        private TestComboBox tcbList => this.FindControl<TestComboBox>("tcbList");
        private TestTreeBox ttbTree => this.FindControl<TestTreeBox>("ttbTree");
        private Button btnShowExampleWindow => this.FindControl<Button>("btnShowExampleWindow");
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
            btnShowExampleWindow.Click += BtnShowExampleWindow_Click;

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

        //private TextBox tbLog => this.FindControl<TextBox>("tbLog");
        //private Button btnCreate => this.FindControl<Button>("btnCreate");
        //private ComboBox cbPopup => this.FindControl<ComboBox>("cbPopup");
        //private StackPanel stk00 => this.FindControl<StackPanel>("stk00");
        //private StackPanel stk01 => this.FindControl<StackPanel>("stk01");
        //private StackPanel stk10 => this.FindControl<StackPanel>("stk10");
        //private StackPanel stk11 => this.FindControl<StackPanel>("stk11");
        //private Button btnOpenControl => this.FindControl<Button>("btnOpenControl");
        //private Button btnCloseControl => this.FindControl<Button>("btnCloseControl");
        //private ContentPresenter cpContent => this.FindControl<ContentPresenter>("cpContent");

        //private bool isOpenControl = false;


        //private List<string> exampleStrings;
        //private void DynControlInit()
        //{
        //    //    btnCreate.Click += BtnCreate_Click;
        //    //    exampleStrings = new List<string>();
        //    //    InitializeStrings();
        //    //    cbPopup.Items = exampleStrings;
        //    //    btnShowExampleWindow.Click += BtnShowExampleWindow_Click;
        //    //    btnOpenControl.Click += BtnOpenControl_Click;
        //    //    btnCloseControl.Click += BtnCloseControl_Click;
        //}

        //private void BtnCloseControl_Click(object sender, RoutedEventArgs e)
        //{
        //    if (isOpenControl)
        //    {
        //        isOpenControl = !isOpenControl;
        //        cpContent.Content = null;
        //        tbLog.Text += "\n Content closed";
        //    }
        //    else
        //    {
        //        tbLog.Text += "\n nthg";
        //    }
        //}

        //private void BtnOpenControl_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!isOpenControl)
        //    {
        //        isOpenControl = !isOpenControl;
        //        cpContent.Content = new ExampleControl();

        //        tbLog.Text += "\n Content opened";
        //    }
        //    else
        //    {

        //        tbLog.Text += "\n nthg";
        //    }
        //}



        //private void InitializeStrings()
        //{
        //    for (int i = 0; i < 88; i++)
        //    {
        //        exampleStrings.Add("example " + i);
        //    }
        //}

        //private void BtnCreate_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        var str = "btn" + i;
        //        stk11.Children.Add(new Button{Content = str});
        //    }

        //    tbLog.Text += "\n Strings update";
        //}
    }
}
