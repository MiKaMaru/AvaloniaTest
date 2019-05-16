using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvaloniaXmlLoadTest
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
