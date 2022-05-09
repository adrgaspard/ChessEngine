using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.Models;
using System.Timers;

namespace ChessEngine.MVVM.ViewModels
{
    public class AIPlayerViewModel : PlayerViewModel
    {
        protected CancellationTokenSource CancellationTokenSource { get; set; }

        public IChessAI ChessAI { get; protected init; }

        protected System.Timers.Timer Timer { get; set; }

        public AIPlayerViewModel(GameViewModel gameVM, IDispatcherService dispatcherService, IChessAI chessAI) : base(gameVM, dispatcherService)
        {
            ChessAI = chessAI;
            CancellationTokenSource = new();
            Timer = new();
            Timer.AutoReset = false;
        }

        public override void OnTurnToPlayBegan()
        {
            CancellationTokenSource cancellationTokenSource = CancellationTokenSource;
            Task.Run(() =>
            {
                ClockParameters parameters = GameVM.Clocks[GameVM.Game.CurrentPlayer].ClockParameters;
                Timer.Interval = Math.Min(5000, Math.Max(parameters.BaseTime.TotalMilliseconds / 180, parameters.IncrementTime.TotalMilliseconds));
                Timer.Elapsed += OnTimerElapsed;
                void OnTimerElapsed(object? sender, ElapsedEventArgs eventArgs)
                {
                    Timer.Elapsed -= OnTimerElapsed;
                    cancellationTokenSource.Cancel();
                }
                Timer.Start();
                Movement movement = ChessAI.SelectMovement(GameVM.Game, cancellationTokenSource.Token);
                Timer.Elapsed -= OnTimerElapsed;
                InvokeMovementFound(movement);
            });
        }

        public override void OnTurnToPlayEnded()
        {
            if (!CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource.Cancel();
            }
            CancellationTokenSource oldCancellationTokenSource = CancellationTokenSource;
            CancellationTokenSource = new();
            oldCancellationTokenSource.Dispose();
        }
    }
}
