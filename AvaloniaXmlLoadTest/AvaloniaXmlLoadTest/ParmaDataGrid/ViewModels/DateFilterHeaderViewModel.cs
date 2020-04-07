using System.Reactive.Linq;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    public class DateFilterHeaderViewModel : AbstractFilterHeaderViewModel
    {
        private readonly DateConditionViewModel _firstConditionViewModel;
        private readonly DateConditionViewModel _lastConditionViewModel;

        public override AbstractConditionViewModel FirstConditionViewModel => _firstConditionViewModel;

        public override AbstractConditionViewModel LastConditionViewModel => _lastConditionViewModel;

        public DateFilterHeaderViewModel(GridFilter filter)
            : base(filter)
        {
            _firstConditionViewModel = new DateConditionViewModel(filter.FirstCondition);
            _lastConditionViewModel = new DateConditionViewModel(filter.LastCondition);

            this.WhenAnyValue(x => x.FirstConditionViewModel.IsActive, x => x.LastConditionViewModel.IsActive)
                .Select(x => x.Item1 || x.Item2)
                .BindTo(this, x => x.IsActive);
        }
    }
}
