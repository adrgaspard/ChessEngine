using System.Collections.ObjectModel;

namespace ChessEngine.Core.Castling.Tools
{
    public static class CastlingConsts
    {
        private const sbyte KingInitialFile = 4;
        public const sbyte KingCastlingOffset = 2;

        public const sbyte KingFileAfterKingSideCastling = 6;
        public const sbyte KingFileAfterQueenSideCastling = 2;
        public const sbyte KingSideCastlingIntermediateFile = 5;
        public const sbyte QueenSideCastlingIntermediateFile = 3;

        private const sbyte KingSideRookInitialFile = 7;
        private const sbyte QueenSideRookInitialFile = 0;

        private const sbyte KingSideRookFileAfterCastling = KingSideRookInitialFile - 2;
        private const sbyte QueenSideRookFileAfterCastling = QueenSideRookInitialFile + 3;

        public static readonly IReadOnlyDictionary<sbyte, CastlingSide> CastlingSideByKingFileAfterCastlingIndexes = new ReadOnlyDictionary<sbyte, CastlingSide>(new Dictionary<sbyte, CastlingSide>(2)
        {
            { KingInitialFile + KingCastlingOffset, CastlingSide.KingSide },
            { KingInitialFile - KingCastlingOffset, CastlingSide.QueenSide }
        });

        public static readonly IReadOnlyDictionary<CastlingSide, sbyte> RookInitialFileByCastlingSide = new ReadOnlyDictionary<CastlingSide, sbyte>(new Dictionary<CastlingSide, sbyte>(2)
        {
            { CastlingSide.KingSide, KingSideRookInitialFile },
            { CastlingSide.QueenSide, QueenSideRookInitialFile }
        });

        public static readonly IReadOnlyDictionary<CastlingSide, sbyte> RookFileAfterCastlingByCastlingSide = new ReadOnlyDictionary<CastlingSide, sbyte>(new Dictionary<CastlingSide, sbyte>(2)
        {
            { CastlingSide.KingSide, KingSideRookFileAfterCastling },
            { CastlingSide.QueenSide, QueenSideRookFileAfterCastling }
        });
    }
}
