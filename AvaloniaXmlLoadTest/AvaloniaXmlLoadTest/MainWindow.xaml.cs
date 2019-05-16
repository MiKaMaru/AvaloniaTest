using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Portable.Xaml;

namespace AvaloniaXmlLoadTest
{
    public class MainWindow : Window
    {
        private MaskedTextBox mtbTest;
        private StackPanel stkTest;
        private Button btnShow;

       
        private StackPanel stkBind;
        private string xamlLoader = @"<TextBox xmlns='https://github.com/avaloniaui' Grid.Row='2' Grid.Column='1' Name='mtbEat'/>";
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
            InitializeControls();
        }

        private void InitializeControls()
        {
            mtbTest = new MaskedTextBox();
            mtbTest.Name = "mtbTest";
            stkTest = this.FindControl<StackPanel>("stkTest");
            btnShow = this.FindControl<Button>("btnShow");

            
            stkBind = this.FindControl<StackPanel>("stkBind");

            // формирование объекта из разметки в строке
            var loader = new AvaloniaXamlLoader();
            var window = (TextBox) loader.Load(xamlLoader); 
            stkBind.Children.Add(window);
        }

        private void Show(object sender, RoutedEventArgs e)
        {
            stkTest.Children.Add(mtbTest);
        }
    }
}
