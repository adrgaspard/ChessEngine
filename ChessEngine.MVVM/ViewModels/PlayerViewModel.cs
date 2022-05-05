using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;

namespace ChessEngine.MVVM.ViewModels
{
    public abstract class PlayerViewModel : ViewModelBase
    {
        public GameViewModel GameVM { get; protected init; }

        public IDispatcherService DispatcherService { get; protected init; }

        protected Colour pointOfView;
        public virtual Colour PointOfView
        {
            get => pointOfView;
            protected set
            {
                if (pointOfView != value)
                {
                    pointOfView = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event EventHandler<MovementFindingEventArgs>? MovementFound;

        protected PlayerViewModel(GameViewModel gameVM, IDispatcherService dispatcherService)
        {
            GameVM = gameVM;
            DispatcherService = dispatcherService;
        }

        public abstract void OnTurnToPlayBegan();

        public abstract void OnTurnToPlayEnded();

        protected void InvokeMovementFound(Movement movement)
        {
            DispatcherService.InvokeAsync(() => MovementFound?.Invoke(this, new(movement)));
        }
    }
}
