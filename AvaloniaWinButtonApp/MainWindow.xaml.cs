using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AvaloniaWinButtonApp
{
    public class MainWindow : Window
    {
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
            var ShowMessage = this.FindControl<Button>("ShowMessage");
            ShowMessage.Tapped += ShowMessage_Tapped;
        }

        private void ShowMessage_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var b = new MessageBox();
            b.HasSystemDecorations = false;
            MessageBox.Show("Тест", "Титуль", MessageBox.MessageBoxButtons.OkCancel);
        }

    }
    
}
