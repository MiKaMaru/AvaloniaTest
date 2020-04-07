namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models
{
    /// <summary>
    /// Опреатор условия.
    /// </summary>
    public enum GridConditionOperator
    {
        /// <summary>
        /// Меньше.
        /// </summary>
        IsLessThan,

        /// <summary>
        /// Меньше или равно.
        /// </summary>
        IsLessThanOrEqualTo,

        /// <summary>
        /// Равно.
        /// </summary>
        IsEqualTo,

        /// <summary>
        /// Не равно.
        /// </summary>
        IsNotEqualTo,

        /// <summary>
        /// Больше или равно.
        /// </summary>
        IsGreaterThanOrEqualTo,

        /// <summary>
        /// Больше.
        /// </summary>
        IsGreaterThan,

        /// <summary>
        /// Начинается с.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Оканчиваетс на.
        /// </summary>
        EndsWith,

        /// <summary>
        /// Содержит.
        /// </summary>
        Contains,

        /// <summary>
        /// Не содержит.
        /// </summary>
        DoesNotContain,

        /// <summary>
        /// Входит в.
        /// </summary>
        IsContainedIn,

        /// <summary>
        /// Не входит в.
        /// </summary>
        IsNotContainedIn,

        /// <summary>
        /// Пусто.
        /// </summary>
        IsEmpty,

        /// <summary>
        /// Не пусто.
        /// </summary>
        IsNotEmpty
    }
}
