using AvaloniaXmlLoadTest.Interfaces;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.Utils
{
    /// <summary>
    /// ViewModel айтема , который отображается в списке.
    /// </summary>
    public class ItemViewModel : ReactiveObject, IItem
    {
        private string _key;
        private string _code;
        private string _value;

        /// <summary>
        /// Ключ.
        /// </summary>
        public virtual string Key
        {
            get => _key;
            set => this.RaiseAndSetIfChanged(ref _key, value);
        }

        /// <summary>
        /// Код.
        /// </summary>
        public string Code
        {
            get => _code;
            set => this.RaiseAndSetIfChanged(ref _code, value);
        }

        /// <summary>
        /// Значение(отображается пользователю). 
        /// </summary>
        public virtual string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        /// <summary>
        /// Создается новая viewModel
        /// </summary>
        public ItemViewModel()
        {
        }

        /// <summary>
        /// Создается новая viewModel
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        public ItemViewModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Создается новая viewModel
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="code">Код.</param>
        /// <param name="value">Значение.</param>
        public ItemViewModel(string key, string code, string value)
        {
            Key = key;
            Code = code;
            Value = value;
        }

        /// <summary>
        /// Возвращает <see cref="Value"/> как строковое представление.
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}
