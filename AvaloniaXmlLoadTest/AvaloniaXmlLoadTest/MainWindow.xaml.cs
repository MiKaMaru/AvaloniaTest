using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Portable.Xaml;
using System.Collections.Generic;

namespace AvaloniaXmlLoadTest
{
    public class MainWindow : Window
    {
        private Button btnCreate => this.FindControl<Button>("btnCreate");
        private ComboBox cbPopup => this.FindControl<ComboBox>("cbPopup");
        private StackPanel stk00 => this.FindControl<StackPanel>("stk00");
        private StackPanel stk01 => this.FindControl<StackPanel>("stk01");
        private StackPanel stk10 => this.FindControl<StackPanel>("stk10");
        private StackPanel stk11 => this.FindControl<StackPanel>("stk11");
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
            btnCreate.Click += BtnCreate_Click;
            exampleStrings = new List<string>();
            InitializeStrings();
            cbPopup.Items = exampleStrings;
            btnShowExampleWindow.Click += BtnShowExampleWindow_Click;
        }

        private void BtnShowExampleWindow_Click(object sender, RoutedEventArgs e)
        {
            var exampleWindow = new ExampleWindow();
            exampleWindow.Show();
        }

        private void InitializeStrings()
        {
            for (int i = 0; i < 88; i++)
            {
                exampleStrings.Add("example " + i);
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                var str = "btn" + i;
                stk11.Children.Add(new Button{Content = str});
            }
        }
    }
}
