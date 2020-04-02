using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaXmlLoadTest.TestControls;
using System.Collections.Generic;

namespace AvaloniaXmlLoadTest
{
    public class ExampleWindow : Window
    {
        private TestComboBox tcbList => this.FindControl<TestComboBox>("tcbList");
        private List<string> exampleStrings;
        public ExampleWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            exampleStrings = new List<string>();
            InitializeStrings();
            tcbList.Items = exampleStrings;
        }

        private void InitializeStrings()
        {
            for (int i = 0; i < 88; i++)
            {
                exampleStrings.Add("example " + i);
            }
        }
    }
}
