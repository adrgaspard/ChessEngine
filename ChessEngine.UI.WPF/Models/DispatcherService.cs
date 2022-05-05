using ChessEngine.MVVM.Models;
using System;
using System.Windows.Threading;

namespace ChessEngine.UI.WPF.Models
{
    public class DispatcherService : IDispatcherService
    {
        protected Dispatcher Dispatcher { get; init; }

        public DispatcherService(Dispatcher dispatcherObject)
        {
            Dispatcher = dispatcherObject;
        }

        public void InvokeAsync(Action action)
        {
            Dispatcher.InvokeAsync(action);
        }
    }
}
