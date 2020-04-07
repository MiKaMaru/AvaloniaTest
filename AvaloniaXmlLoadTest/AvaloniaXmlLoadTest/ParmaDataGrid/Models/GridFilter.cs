using System;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.Models
{
    /// <summary>
    /// Фильтр.
    /// </summary>
    public class GridFilter : ICloneable
    {
        /// <summary>
        /// Ключ реквизита.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип фильтра.
        /// </summary>
        public GridFilterType FilterType { get; set; }

        /// <summary>
        /// Оператор.
        /// </summary>
        public GridFilterOperator Operator { get; set; }

        /// <summary>
        /// Первое условие.
        /// </summary>
        public GridCondition FirstCondition { get; private set; }

        /// <summary>
        /// Последнее ( ¯\_(ツ)_/¯ ) условие.
        /// </summary>
        public GridCondition LastCondition { get; private set; }

        /// <summary>
        /// Включен ли фильтр.
        /// </summary>
        public bool IsEnable => FirstCondition.IsEnable || LastCondition.IsEnable;

        public GridFilter()
        {
            Key = 0;
            FilterType = GridFilterType.Text;
            Operator = GridFilterOperator.And;
            FirstCondition = new GridCondition();
            LastCondition = new GridCondition();
        }

        /// <summary>
        /// Отключение фильтра
        /// </summary>
        public void DisableFilter()
        {
            FirstCondition.ResetCondition();
            LastCondition.ResetCondition();
        }

        public object Clone()
        {
            var newFilter = new GridFilter
            {
                Key = this.Key,
                Description = this.Description,
                FilterType = this.FilterType,
                Operator = this.Operator,
            };

            newFilter.FirstCondition.Copy(this.FirstCondition);
            newFilter.LastCondition.Copy(this.LastCondition);
            return newFilter;
        }
    }
}
