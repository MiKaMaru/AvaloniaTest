using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace AvaloniaXmlLoadTest.Utils
{
    public class ViewModelBase : ReactiveObject
    {
        public Interaction<(string result, string message), Unit> SendOperationResult { get; } =
            new Interaction<(string result, string message), Unit>();

        public Interaction<string, bool> ConfirmOperation { get; } = new Interaction<string, bool>();

        /// <summary>
        /// Освобождает контролы от родительских контролов.
        /// При переходе между представлениями с помощью ReactiveUI.RoutingState объект представления создается заново, а ViewModel - нет.
        /// Если в представлении есть привязка к свойству Control из VM возникнет ошибка: "The control already has a visual parent.".
        /// Т.е., Control-у назначается родитель, который у него уже есть (контрол из предыдущего представления).
        /// </summary>
        public Interaction<Unit, Unit> ReleaseControls { get; } = new Interaction<Unit, Unit>();
    }
}
