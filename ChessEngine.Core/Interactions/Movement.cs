using ChessEngine.Core.Environment;

namespace ChessEngine.Core.Interactions
{
    public struct Movement : IEquatable<Movement>
    {
        public readonly Position OldPosition;
        public readonly Position NewPosition;
        public readonly MovementFlag Flag;

        public Movement(Position oldPosition, Position newPosition, MovementFlag flag)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
            Flag = flag;
        }

        public static bool operator ==(Movement left, Movement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Movement left, Movement right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is Movement movement && Equals(movement);
        }

        public bool Equals(Movement other)
        {
            return OldPosition == other.OldPosition && NewPosition == other.NewPosition && Flag == other.Flag;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OldPosition, NewPosition, Flag);
        }

        public override string ToString()
        {
            return $"Old: {OldPosition}, New: {NewPosition}, {nameof(Flag)}: {Flag}";
        }
    }
}
