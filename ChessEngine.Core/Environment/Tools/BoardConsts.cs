using System.Collections.ObjectModel;

namespace ChessEngine.Core.Environment.Tools
{
    public static class BoardConsts
    {
        public const byte NumberOfPlayers = 2;
        public const byte BoardSize = 8;
        public const byte NumberOfPositions = BoardSize * BoardSize;
        public static readonly Position NoPosition = new(sbyte.MinValue, sbyte.MinValue);
        public static readonly Position WhiteKingInitialPosition = new(0, 4);
        public static readonly Position BlackKingInitialPosition = new(7, 4);
        public static readonly Position KingSideWhiteRookInitialPosition = new(0, 7);
        public static readonly Position QueenSideWhiteRookInitialPosition = new(0, 0);
        public static readonly Position KingSideBlackRookInitialPosition = new(7, 7);
        public static readonly Position QueenSideBlackRookInitialPosition = new(7, 0);

        public static readonly IReadOnlyDictionary<Colour, byte> ColourIndexes = new ReadOnlyDictionary<Colour, byte>(new Dictionary<Colour, byte>(2)
        {
            { Colour.White, 0 },
            { Colour.Black, 1 },
        });

        public static readonly IReadOnlyDictionary<Colour, sbyte> PawnInitialRank = new ReadOnlyDictionary<Colour, sbyte>(new Dictionary<Colour, sbyte>(2)
        {
            { Colour.White, 1 },
            { Colour.Black, BoardSize - 2 }
        });

        public static readonly IReadOnlyDictionary<Colour, sbyte> PawnRankAfterEnPassantCapture = new ReadOnlyDictionary<Colour, sbyte>(new Dictionary<Colour, sbyte>(2)
        {
            { Colour.White, BoardSize - 3 },
            { Colour.Black, 2 }
        });

        public static readonly IReadOnlyDictionary<Colour, sbyte> PawnFinalRank = new ReadOnlyDictionary<Colour, sbyte>(new Dictionary<Colour, sbyte>(2)
        {
            { Colour.White, BoardSize - 1 },
            { Colour.Black, 0 }
        });

        public static readonly IReadOnlyDictionary<Colour, sbyte> PawnDirections = new ReadOnlyDictionary<Colour, sbyte>(new Dictionary<Colour, sbyte>(2)
        {
            { Colour.White, 1 },
            { Colour.Black, -1 }
        });

        public static readonly IReadOnlyDictionary<PieceType, byte> MaxInstancesOfPieceOnBoardForOnePlayer = new ReadOnlyDictionary<PieceType, byte>(new Dictionary<PieceType, byte>(5)
        {
            { PieceType.Pawn, 8 },
            { PieceType.Knight, 10 },
            { PieceType.Bishop, 10 },
            { PieceType.Rook, 10 },
            { PieceType.Queen, 9 }
        });
    }
}
