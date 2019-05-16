using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvaloniaDataGridTest
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
