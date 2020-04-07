namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models
{
    /// <summary>
    /// Поведение вьюмодели, имеющей коллекцию подписок
    /// </summary>
    public interface IListenableViewModel
    {
        /// <summary>
        /// Инициализация подписок
        /// </summary>
        void InitSubscriptions();
    }
}