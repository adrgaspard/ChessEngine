using ChessEngine.Core.Interactions;

namespace ChessEngine.MVVM.Models
{
    public class MovementFindingEventArgs : EventArgs
    {
        public readonly Movement Movement;

        public MovementFindingEventArgs(Movement movement)
        {
            Movement = movement;
        }
    }
}
