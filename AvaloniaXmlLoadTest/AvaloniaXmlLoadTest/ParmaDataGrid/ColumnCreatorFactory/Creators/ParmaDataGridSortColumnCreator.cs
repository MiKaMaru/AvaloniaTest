using Avalonia.Controls;
using AvaloniaXmlLoadTest.DataGridDomain;
using AvaloniaXmlLoadTest.DataGridDomain.Interfaces;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ColumnCreatorFactory.Creators
{
    /// <summary>
    /// Фабрика по созданию колонок для сортировок данных в гриде в в <see cref="ParmaDataGridOd"/>
    /// </summary>
    public abstract class ParmaDataGridSortColumnCreator : IDataGridColumnCreator
    {
        /// <summary>
        /// Создать колонку
        /// </summary>
        /// <param name="info">Модель колонки</param>
        /// <param name="additional">Дополнительные параметры</param>
        /// <returns></returns>
        public abstract DataGridColumn CreateColumn(ParmaDataGridOdColumnInfo info);

        /// <summary>
        /// Возвращает минимальную и максимальную ширину колонки.
        /// </summary>
        /// <param name="widthInfo">Информация о ширине колонки</param>
        /// <returns>Минимальная и максимальная ширина колонки</returns>
        public (double minWidth, double maxWidth) GetWidth(DataGridLength widthInfo)
        {
            double minWidth = 100;
            double maxWidth = widthInfo.UnitType != DataGridLengthUnitType.Auto && widthInfo.Value > minWidth
                ? widthInfo.Value
                : 200;

            return (minWidth, maxWidth);
        }
    }
}