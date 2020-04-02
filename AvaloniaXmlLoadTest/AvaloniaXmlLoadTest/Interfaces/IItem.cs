namespace AvaloniaXmlLoadTest.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// Значение элемента
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Ключ элемента
        /// </summary>
        string Key { get; set; }
    }
}
