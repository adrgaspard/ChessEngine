using System.Collections.ObjectModel;

namespace ChessEngine.Core.Environment.Tools
{
    public static class PieceConsts
    {
        public static readonly Piece NoPiece = new(PieceType.None, Colour.None);

        public static readonly IReadOnlyDictionary<PieceType, IReadOnlyList<Position>> SlidingOffsets = new ReadOnlyDictionary<PieceType, IReadOnlyList<Position>>(new Dictionary<PieceType, IReadOnlyList<Position>>()
        {
            { PieceType.Bishop, new ReadOnlyCollection<Position>(new List<Position>(4) { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) } ) },
            { PieceType.Rook, new ReadOnlyCollection<Position>(new List<Position>(4) { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) } ) },
            { PieceType.Queen, new ReadOnlyCollection<Position>(new List<Position>(8) { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1), new(1, 0), new(0, 1), new(-1, 0), new(0, -1) } ) },
        });
    }
}
