using ChessEngine.Core.Interactions;

namespace ChessEngine.MVVM.Models
{
    public class MovementExecutionEventArgs : EventArgs
    {
        public Movement Movement { get; protected init; }

        public MovementExecutionDirection Direction { get; protected init; }

        public MovementExecutionEventArgs(Movement movement, MovementExecutionDirection direction)
        {
            Movement = movement;
            Direction = direction;
        }
    }
}
