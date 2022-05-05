using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class LocalHumanPlayerViewModel : PlayerViewModel
    {
        protected Colour playableColour;
        public Colour PlayableColour
        {
            get => playableColour;
            protected set
            {
                if (playableColour != value)
                {
                    playableColour = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsReadOnly { get; protected init; }

        public ICommand TreatMovementRequestCommand { get; protected set; }

        public LocalHumanPlayerViewModel(GameViewModel gameVM, IDispatcherService dispatcherService, Colour pointOfView, bool isReadOnly) : base(gameVM, dispatcherService)
        {
            PointOfView = pointOfView;
            IsReadOnly = isReadOnly;
            if (IsReadOnly)
            {
                TreatMovementRequestCommand = new RelayCommand<Movement>((movement) => { });
            }
            else
            {
                TreatMovementRequestCommand = new RelayCommand<Movement>(InvokeMovementFound);
            }
        }

        public override void OnTurnToPlayBegan()
        {
            VerifyCanDrag();
        }

        public override void OnTurnToPlayEnded()
        {
            VerifyCanDrag();
        }

        protected void VerifyCanDrag()
        {
            PlayableColour = IsReadOnly || GameVM.EndGameType != Core.Match.EndGameType.GameIsNotFinished ? Colour.None : GameVM.Game.CurrentPlayer & PointOfView;
        }
    }
}
