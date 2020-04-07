using System;
using System.Reactive;
using System.Reactive.Linq;
using AvaloniaXmlLoadTest.ParmaDataGrid.Models;
using ReactiveUI;

namespace AvaloniaXmlLoadTest.ParmaDataGrid.ViewModels
{
    public class TextFilterHeaderViewModel : AbstractFilterHeaderViewModel
    {
        private readonly TextConditionViewModel _firstConditionViewModel;
        private readonly TextConditionViewModel _lastConditionViewModel;

        public override AbstractConditionViewModel FirstConditionViewModel => _firstConditionViewModel;

        public override AbstractConditionViewModel LastConditionViewModel => _lastConditionViewModel;

        public TextFilterHeaderViewModel(GridFilter filter)
            :base(filter)
        {
            _firstConditionViewModel = new TextConditionViewModel(filter.FirstCondition);
            _lastConditionViewModel = new TextConditionViewModel(filter.LastCondition);

            this.WhenAnyValue(x => x.FirstConditionViewModel.IsActive, x => x.LastConditionViewModel.IsActive)
                .Select(x => x.Item1 || x.Item2)
                .BindTo(this, x => x.IsActive);
        }
    }
}
