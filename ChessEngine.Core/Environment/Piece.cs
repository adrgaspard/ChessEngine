namespace ChessEngine.Core.Environment
{
    public struct Piece : IEquatable<Piece>
    {
        public readonly PieceType Type;
        public readonly Colour Colour;

        public Piece(PieceType type, Colour colour)
        {
            Type = type;
            Colour = colour;
        }

        public static bool operator ==(Piece left, Piece right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Piece left, Piece right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is Piece piece && Equals(piece);
        }

        public bool Equals(Piece other)
        {
            return Type == other.Type && Colour == other.Colour;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Colour);
        }

        public override string ToString()
        {
            return $"{Colour} {Type}";
        }
    }
}
