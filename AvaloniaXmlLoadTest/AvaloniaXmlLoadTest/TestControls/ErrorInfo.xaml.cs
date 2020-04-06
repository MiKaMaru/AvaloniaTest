using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaXmlLoadTest.TestControls
{
    public class ErrorInfo : UserControl
    {
		public static readonly StyledProperty<string> ValueProperty;
		static ErrorInfo()
		{
			ValueProperty = AvaloniaProperty.Register<ErrorInfo, string>(
				nameof(Value),
				defaultValue: string.Empty);
			ValueProperty.Changed.AddClassHandler<ErrorInfo>(x => x.ValueChanged);
		}

		public ErrorInfo()
		{
			InitializeComponent();
			ToolTip.SetTip(StackPanel1, Value);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void ValueChanged(AvaloniaPropertyChangedEventArgs e)
		{
			ToolTip.SetTip(StackPanel1, e.NewValue);
		}

		public StackPanel StackPanel1 => this.FindControl<StackPanel>(nameof(StackPanel1));
		public string ImagePath => "Assets|ico_error.png";

		public string Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}
	}
}
