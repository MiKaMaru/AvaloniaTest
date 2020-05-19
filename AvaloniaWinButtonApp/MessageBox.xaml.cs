using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AvaloniaWinButtonApp
{
    public class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            NoYes,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public MessageBox()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public static Task<MessageBoxResult> Show(string text, string title, MessageBoxButtons buttons, Window parent = null)
        {

            int currentFocus = 0;

            var msgbox = new MessageBox()
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = text;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

            var res = MessageBoxResult.Ok;
            List<IInputElement> allButtons = new List<IInputElement>();
            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption, Focusable = false };
                btn.Click += (_, __) => {
                    res = r;
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
                allButtons.Add(btn);
            }



            void KeyDown(object sender, KeyEventArgs args)
            {
                if (allButtons.Count <= 1)
                    return;
                switch (args.Key)
                {
                    case Key.Left:
                        if (currentFocus > 0)
                            currentFocus--;
                        else
                            currentFocus = allButtons.Count - 1;
                        break;
                    case Key.Tab:
                    case Key.Right:
                        if (currentFocus < allButtons.Count - 1)
                            currentFocus++;
                        else
                            currentFocus = 0;
                        break;
                    default: return;
                }
                Application.Current.FocusManager.Focus(allButtons[currentFocus]);
                //FocusManager.Instance.Focus(allButtons[currentFocus], NavigationMethod.Tab);
            };

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            {
                AddButton("Ok", MessageBoxResult.Ok, true);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Да", MessageBoxResult.Yes);
                AddButton("Нет", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.NoYes)
            {
                AddButton("Нет", MessageBoxResult.No, true);
                AddButton("Да", MessageBoxResult.Yes);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                AddButton("Отмена", MessageBoxResult.Cancel, true);

            buttonPanel.KeyDown += KeyDown;

            var tcs = new TaskCompletionSource<MessageBoxResult>();


            void CancelClosing(object sender, CancelEventArgs args)
            {
                args.Cancel = true;
            }

            msgbox.Closed += (sender, args) =>
            {
                tcs.TrySetResult(res);
                buttonPanel.KeyDown -= KeyDown;
            };

            if (parent != null)
            {

                msgbox.ShowDialog(parent);
            }
            else
            {
                msgbox.Show();
            }
            Application.Current.FocusManager.Focus(allButtons[0]);
            //Тестирование
            //FocusManager.Instance.Focus(allButtons[0], NavigationMethod.Tab);
            return tcs.Task;
        }

    }
}
