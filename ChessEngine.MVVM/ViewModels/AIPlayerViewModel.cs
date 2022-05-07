using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.Models;

namespace ChessEngine.MVVM.ViewModels
{
    public class AIPlayerViewModel : PlayerViewModel
    {
        protected CancellationTokenSource CancellationTokenSource { get; set; }

        public IChessAI ChessAI { get; protected init; }

        public AIPlayerViewModel(GameViewModel gameVM, IDispatcherService dispatcherService, IChessAI chessAI) : base(gameVM, dispatcherService)
        {
            ChessAI = chessAI;
            CancellationTokenSource = new();
        }

        public override void OnTurnToPlayBegan()
        {
            CancellationTokenSource cancellationTokenSource = CancellationTokenSource;
            Task.Run(() =>
            {
                Movement movement = ChessAI.SelectMovement(GameVM.Game, cancellationTokenSource.Token);
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    InvokeMovementFound(movement);
                }
            });
        }

        public override void OnTurnToPlayEnded()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new();
        }
    }
}
