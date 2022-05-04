using ChessEngine.Core.Environment;
using System.Collections.ObjectModel;

namespace ChessEngine.Core.Serialization.FEN.Tools
{
    public static class FENConsts
    {
        public const string StartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public const string DebugFEN = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8";

        public static readonly IReadOnlyDictionary<char, PieceType> SymbolsToPieceType = new ReadOnlyDictionary<char, PieceType>(new Dictionary<char, PieceType>()
        {
            { 'k', PieceType.King },
            { 'p', PieceType.Pawn },
            { 'n', PieceType.Knight },
            { 'b', PieceType.Bishop },
            { 'r', PieceType.Rook },
            { 'q', PieceType.Queen }
        });
    }
}
