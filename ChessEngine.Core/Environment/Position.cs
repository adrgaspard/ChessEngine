namespace ChessEngine.Core.Environment
{
    public struct Position : IEquatable<Position>
    {
        public readonly sbyte Rank;
        public readonly sbyte File;

        public Position(sbyte rank, sbyte file)
        {
            Rank = rank;
            File = file;
        }

        public bool Equals(Position other)
        {
            return Rank == other.Rank && File == other.File;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rank, File);
        }

        public override bool Equals(object? obj)
        {
            return obj is Position value && Equals(value);
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator *(Position left, sbyte right)
        {
            return new((sbyte)(left.Rank * right), (sbyte)(left.File * right));
        }

        public static Position operator +(Position left, Position right)
        {
            return new((sbyte)(left.Rank + right.Rank), (sbyte)(left.File + right.File));
        }

        public static Position operator -(Position left, Position right)
        {
            return new((sbyte)(left.Rank - right.Rank), (sbyte)(left.File - right.File));
        }

        public static Position operator -(Position position)
        {
            return new((sbyte)-position.Rank, (sbyte)-position.File);
        }

        public override string ToString()
        {
            return $"({Rank}, {File})";
        }
    }
}