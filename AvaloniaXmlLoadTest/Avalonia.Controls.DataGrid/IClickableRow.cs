namespace Avalonia.Controls
{
    /// <summary>
    /// Этотт интерфейс реализуют вью модели строк, которые подписываются на клик.
    /// Работает это только когда выставлено свойство <see cref="DataGrid.CurentRowAutoBehaviourDisabled"/>
    /// </summary>
    public interface IClickableRow
    {
        /// <summary>
        /// Кликнули по строчке.
        /// </summary>
        void Click();
    }
}
