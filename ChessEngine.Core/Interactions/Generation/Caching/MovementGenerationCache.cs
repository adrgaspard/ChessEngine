using ChessEngine.Core.Castling.Tools;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions.Generation.Tools;
using System.Collections.ObjectModel;

namespace ChessEngine.Core.Interactions.Generation.Caching
{
    public static class MovementGenerationCache
    {
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Movement>> KingClassicMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Movement>> KingCastlingMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Movement>> KingAllMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnQuietMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnClassicCaptureOnLowerSideMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnClassicCaptureOnUpperSideMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnEnPassantCaptureOnLowerSideMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnEnPassantCaptureOnUpperSideMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnClassicCaptureMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnEnPassantCaptureMovements;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>> PawnCaptureMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Movement>> KnightMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> BishopGroupedAndOrderedMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> RookGroupedAndOrderedMovements;
        private static readonly IReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> QueenGroupedAndOrderedMovements;
        private static readonly IReadOnlyDictionary<ValueTuple<Position, Position>, byte> NumberOfPositionsWithOffsets;
        private static readonly IReadOnlyDictionary<Position, ulong> KnightAttackMasks;
        private static readonly IReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, ulong>> PawnAttackMasks;
        private static readonly IReadOnlyDictionary<Position, ulong> KingAttackMasks;

        static MovementGenerationCache()
        {
            Dictionary<Position, IReadOnlyList<Movement>> kingClassicMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> kingCastlingMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> kingAllMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnQuietMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnQuietMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnClassicCaptureOnLowerSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnClassicCaptureOnLowerSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnClassicCaptureOnUpperSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnClassicCaptureOnUpperSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnEnPassantCaptureOnLowerSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnEnPassantCaptureOnLowerSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnEnPassantCaptureOnUpperSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnEnPassantCaptureOnUpperSideMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnClassicCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnClassicCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnEnPassantCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnEnPassantCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> whitePawnAllCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> blackPawnAllCaptureMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Movement>> knightMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> bishopGroupedAndOrderedMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> rookGroupedAndOrderedMovements = new(BoardConsts.NumberOfPositions);
            Dictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>> queenGroupedAndOrderedMovements = new(BoardConsts.NumberOfPositions);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Position position = new(i, j);
                    kingClassicMovements.Add(position, new ReadOnlyCollection<Movement>(GenerateKingClassicMovements(position)));
                    kingCastlingMovements.Add(position, new ReadOnlyCollection<Movement>(GenerateKingCastlingMovements(position)));
                    kingAllMovements.Add(position, new ReadOnlyCollection<Movement>(kingClassicMovements[position].Concat(kingCastlingMovements[position]).ToList()));
                    whitePawnQuietMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnQuietMovements(position, Colour.White)));
                    blackPawnQuietMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnQuietMovements(position, Colour.Black)));
                    whitePawnClassicCaptureOnLowerSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.White, -1, false)));
                    blackPawnClassicCaptureOnLowerSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.Black, -1, false)));
                    whitePawnClassicCaptureOnUpperSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.White, 1, false)));
                    blackPawnClassicCaptureOnUpperSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.Black, 1, false)));
                    whitePawnEnPassantCaptureOnLowerSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.White, -1, true)));
                    blackPawnEnPassantCaptureOnLowerSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.Black, -1, true)));
                    whitePawnEnPassantCaptureOnUpperSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.White, 1, true)));
                    blackPawnEnPassantCaptureOnUpperSideMovements.Add(position, new ReadOnlyCollection<Movement>(GeneratePawnCaptureMovements(position, Colour.Black, 1, true)));
                    whitePawnAllCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(whitePawnClassicCaptureOnLowerSideMovements[position]
                        .Concat(whitePawnClassicCaptureOnUpperSideMovements[position]).Concat(whitePawnEnPassantCaptureOnLowerSideMovements[position])
                        .Concat(whitePawnEnPassantCaptureOnUpperSideMovements[position]).ToList()));
                    blackPawnAllCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(blackPawnClassicCaptureOnLowerSideMovements[position]
                        .Concat(blackPawnClassicCaptureOnUpperSideMovements[position]).Concat(blackPawnEnPassantCaptureOnLowerSideMovements[position])
                        .Concat(blackPawnEnPassantCaptureOnUpperSideMovements[position]).ToList()));
                    whitePawnClassicCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(whitePawnClassicCaptureOnLowerSideMovements[position]
                        .Concat(whitePawnClassicCaptureOnUpperSideMovements[position]).ToList()));
                    blackPawnClassicCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(blackPawnClassicCaptureOnLowerSideMovements[position]
                        .Concat(blackPawnClassicCaptureOnUpperSideMovements[position]).ToList()));
                    whitePawnEnPassantCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(whitePawnEnPassantCaptureOnLowerSideMovements[position]
                        .Concat(whitePawnEnPassantCaptureOnUpperSideMovements[position]).ToList()));
                    blackPawnEnPassantCaptureMovements.Add(position, new ReadOnlyCollection<Movement>(blackPawnEnPassantCaptureOnLowerSideMovements[position]
                        .Concat(blackPawnEnPassantCaptureOnUpperSideMovements[position]).ToList()));
                    knightMovements.Add(position, new ReadOnlyCollection<Movement>(GenerateKnightMovements(position)));
                    bishopGroupedAndOrderedMovements.Add(position, new ReadOnlyCollection<Tuple<Position, IReadOnlyList<Movement>>>(PieceConsts.SlidingOffsets[PieceType.Bishop]
                        .Select(offset => new Tuple<Position, IReadOnlyList<Movement>>(offset, new ReadOnlyCollection<Movement>(GenerateSlidingMovements(position, offset)))).ToList()));
                    rookGroupedAndOrderedMovements.Add(position, new ReadOnlyCollection<Tuple<Position, IReadOnlyList<Movement>>>(PieceConsts.SlidingOffsets[PieceType.Rook]
                        .Select(offset => new Tuple<Position, IReadOnlyList<Movement>>(offset, new ReadOnlyCollection<Movement>(GenerateSlidingMovements(position, offset)))).ToList()));
                    queenGroupedAndOrderedMovements.Add(position, new ReadOnlyCollection<Tuple<Position, IReadOnlyList<Movement>>>(PieceConsts.SlidingOffsets[PieceType.Queen]
                        .Select(offset => new Tuple<Position, IReadOnlyList<Movement>>(offset, new ReadOnlyCollection<Movement>(GenerateSlidingMovements(position, offset)))).ToList()));
                }
            }
            KingClassicMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Movement>>(kingClassicMovements);
            KingCastlingMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Movement>>(kingCastlingMovements);
            KingAllMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Movement>>(kingAllMovements);
            PawnQuietMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnQuietMovements },
                { Colour.Black, blackPawnQuietMovements }
            });
            PawnClassicCaptureOnLowerSideMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnClassicCaptureOnLowerSideMovements },
                { Colour.Black, blackPawnClassicCaptureOnLowerSideMovements }
            });
            PawnClassicCaptureOnUpperSideMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnClassicCaptureOnUpperSideMovements },
                { Colour.Black, blackPawnClassicCaptureOnUpperSideMovements }
            });
            PawnEnPassantCaptureOnLowerSideMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnEnPassantCaptureOnLowerSideMovements },
                { Colour.Black, blackPawnEnPassantCaptureOnLowerSideMovements }
            });
            PawnEnPassantCaptureOnUpperSideMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnEnPassantCaptureOnUpperSideMovements },
                { Colour.Black, blackPawnEnPassantCaptureOnUpperSideMovements }
            });
            PawnClassicCaptureMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnClassicCaptureMovements },
                { Colour.Black, blackPawnClassicCaptureMovements }
            });
            PawnEnPassantCaptureMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnEnPassantCaptureMovements },
                { Colour.Black, blackPawnEnPassantCaptureMovements }
            });
            PawnCaptureMovements = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>(new Dictionary<Colour, IReadOnlyDictionary<Position, IReadOnlyList<Movement>>>()
            {
                { Colour.White, whitePawnAllCaptureMovements },
                { Colour.Black, blackPawnAllCaptureMovements }
            });
            KnightMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Movement>>(knightMovements);
            BishopGroupedAndOrderedMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>>(bishopGroupedAndOrderedMovements);
            RookGroupedAndOrderedMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>>(rookGroupedAndOrderedMovements);
            QueenGroupedAndOrderedMovements = new ReadOnlyDictionary<Position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>>>(queenGroupedAndOrderedMovements);
            NumberOfPositionsWithOffsets = new ReadOnlyDictionary<ValueTuple<Position, Position>, byte>(GenerateNumberOfPositionsToOffsets());
            KnightAttackMasks = new ReadOnlyDictionary<Position, ulong>(GenerateKnightAttackMasks());
            PawnAttackMasks = new ReadOnlyDictionary<Colour, IReadOnlyDictionary<Position, ulong>>(GenereatePawnAttackMasks());
            KingAttackMasks = new ReadOnlyDictionary<Position, ulong>(GenerateKingAttackMasks());
        }

        public static IReadOnlyList<Movement> GetKingMovements(Position position)
        {
            return KingAllMovements[position];
        }

        public static IReadOnlyList<Movement> GetKingClassicMovements(Position position)
        {
            return KingClassicMovements[position];
        }

        public static IReadOnlyList<Movement> GetKingCastlingMovements(Position position)
        {
            return KingCastlingMovements[position];
        }

        public static IReadOnlyList<Movement> GetPawnAllCaptureMovements(Position position, Colour colour)
        {
            return PawnCaptureMovements[colour][position];
        }

        public static IReadOnlyList<Movement> GetPawnQuietMovements(Position position, Colour colour)
        {
            return PawnQuietMovements[colour][position];
        }

        public static IReadOnlyList<Movement> GetPawnPartialCaptureMovements(Position position, Colour colour, bool isEnPassant)
        {
            return isEnPassant ? PawnEnPassantCaptureMovements[colour][position] : PawnClassicCaptureMovements[colour][position];
        }

        public static IReadOnlyList<Movement> GetPawnCaptureOnLowerSideMovements(Position position, Colour colour, bool isEnPassant)
        {
            return isEnPassant ? PawnEnPassantCaptureOnLowerSideMovements[colour][position] : PawnClassicCaptureOnLowerSideMovements[colour][position];
        }

        public static IReadOnlyList<Movement> GetPawnCaptureOnUpperSideMovements(Position position, Colour colour, bool isEnPassant)
        {
            return isEnPassant ? PawnEnPassantCaptureOnUpperSideMovements[colour][position] : PawnClassicCaptureOnUpperSideMovements[colour][position];
        }

        public static IReadOnlyList<Movement> GetKnightMovements(Position position)
        {
            return KnightMovements[position];
        }

        public static IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>> GetBishopGroupedAndOrderedMovements(Position position)
        {
            return BishopGroupedAndOrderedMovements[position];
        }

        public static IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>> GetRookGroupedAndOrderedMovements(Position position)
        {
            return RookGroupedAndOrderedMovements[position];
        }

        public static IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>> GetQueenGroupedAndOrderedMovements(Position position)
        {
            return QueenGroupedAndOrderedMovements[position];
        }

        public static byte GetRangeSizeWithOffset(Position position, Position offset)
        {
            return NumberOfPositionsWithOffsets[(position, offset)];
        }

        public static ulong GetKnightAttackMask(Position position)
        {
            return KnightAttackMasks[position];
        }

        public static ulong GetPawnAttackMap(Position position, Colour colour)
        {
            return PawnAttackMasks[colour][position];
        }

        public static ulong GetKingAttackMask(Position position)
        {
            return KingAttackMasks[position];
        }

        private static IList<Movement> GenerateKingClassicMovements(Position position)
        {
            return new List<Movement>(8)
            {
                new Movement(position, new((sbyte)(position.Rank - 1), (sbyte)(position.File - 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 1), position.File), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 1), (sbyte)(position.File + 1)), MovementFlag.None),
                new Movement(position, new(position.Rank, (sbyte)(position.File - 1)), MovementFlag.None),
                new Movement(position, new(position.Rank, (sbyte)(position.File + 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 1), (sbyte)(position.File - 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 1), position.File), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 1), (sbyte)(position.File + 1)), MovementFlag.None),
            }.Where(movement => movement.NewPosition.IsOnBoard()).ToList();
        }

        private static IList<Movement> GenerateKingCastlingMovements(Position position)
        {
            if (position == BoardConsts.WhiteKingInitialPosition || position == BoardConsts.BlackKingInitialPosition)
            {
                return new List<Movement>(2)
                {
                    new(position, new(position.Rank, (sbyte)(position.File + CastlingConsts.KingCastlingOffset)), MovementFlag.KingCastling),
                    new(position, new(position.Rank, (sbyte)(position.File - CastlingConsts.KingCastlingOffset)), MovementFlag.KingCastling),
                };
            }
            else
            {
                return new List<Movement>(0);
            }
        }

        private static IList<Movement> GeneratePawnQuietMovements(Position position, Colour pawnColour)
        {
            sbyte initialRank = BoardConsts.PawnInitialRank[pawnColour];
            sbyte finalRank = BoardConsts.PawnFinalRank[pawnColour];
            sbyte direction = BoardConsts.PawnDirections[pawnColour];
            sbyte lastRankBeforeFinalRank = (sbyte)(finalRank - direction);
            List<Movement> result = new(4);
            if (position.Rank != finalRank)
            {
                Position newPosition = new((sbyte)(position.Rank + direction), position.File);
                if (position.Rank == lastRankBeforeFinalRank)
                {
                    result.Add(new(position, newPosition, MovementFlag.PawnPromotionToBishop));
                    result.Add(new(position, newPosition, MovementFlag.PawnPromotionToKnight));
                    result.Add(new(position, newPosition, MovementFlag.PawnPromotionToRook));
                    result.Add(new(position, newPosition, MovementFlag.PawnPromotionToQueen));
                }
                else
                {
                    result.Add(new(position, newPosition, MovementFlag.None));
                    if (position.Rank == initialRank)
                    {
                        result.Add(new(position, new((sbyte)(position.Rank + direction * 2), position.File), MovementFlag.PawnPush));
                    }
                }
            }
            return result;
        }

        private static IList<Movement> GeneratePawnCaptureMovements(Position position, Colour pawnColour, sbyte attackSideSign, bool isEnPassant)
        {
            sbyte initialRank = BoardConsts.PawnInitialRank[pawnColour];
            sbyte finalRank = BoardConsts.PawnFinalRank[pawnColour];
            sbyte direction = BoardConsts.PawnDirections[pawnColour];
            sbyte lastRankBeforeFinalRank = (sbyte)(finalRank - direction);
            sbyte rankAfterEnPassantCapture = BoardConsts.PawnRankAfterEnPassantCapture[pawnColour];
            List<Movement> result = new(4);
            if (position.Rank != finalRank)
            {
                Position newPosition = new((sbyte)(position.Rank + direction), (sbyte)(position.File + attackSideSign));
                if (isEnPassant)
                {
                    if (newPosition.Rank == rankAfterEnPassantCapture)
                    {
                        result.Add(new(position, newPosition, MovementFlag.PawnEnPassantCapture));
                    }
                }
                else
                {
                    if (position.Rank == lastRankBeforeFinalRank)
                    {
                        result.Add(new(position, newPosition, MovementFlag.PawnPromotionToBishop));
                        result.Add(new(position, newPosition, MovementFlag.PawnPromotionToKnight));
                        result.Add(new(position, newPosition, MovementFlag.PawnPromotionToRook));
                        result.Add(new(position, newPosition, MovementFlag.PawnPromotionToQueen));
                    }
                    else
                    {
                        result.Add(new(position, newPosition, MovementFlag.None));
                    }
                }
            }
            return result.Where(movement => movement.NewPosition.IsOnBoard()).ToList();
        }

        private static IList<Movement> GenerateKnightMovements(Position position)
        {
            return new List<Movement>(8)
            {
                new Movement(position, new((sbyte)(position.Rank + 2), (sbyte)(position.File + 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 2), (sbyte)(position.File - 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 2), (sbyte)(position.File + 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 2), (sbyte)(position.File - 1)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 1), (sbyte)(position.File + 2)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank + 1), (sbyte)(position.File - 2)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 1), (sbyte)(position.File + 2)), MovementFlag.None),
                new Movement(position, new((sbyte)(position.Rank - 1), (sbyte)(position.File - 2)), MovementFlag.None),
            }.Where(movement => movement.NewPosition.IsOnBoard()).ToList();
        }

        private static IList<Movement> GenerateSlidingMovements(Position position, Position offsetDirection)
        {
            List<Movement> result = new(BoardConsts.BoardSize - 1);
            Position iterator = position + offsetDirection;
            while (iterator.IsOnBoard())
            {
                result.Add(new(position, iterator, MovementFlag.None));
                iterator += offsetDirection;
            }
            return result;
        }

        private static IDictionary<ValueTuple<Position, Position>, byte> GenerateNumberOfPositionsToOffsets()
        {
            IReadOnlyList<Position> offsets = PieceConsts.SlidingOffsets[PieceType.Queen];
            Dictionary<ValueTuple<Position, Position>, byte> result = new(BoardConsts.NumberOfPlayers * offsets.Count);
            foreach (Position offset in offsets)
            {
                for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
                {
                    for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                    {
                        byte count = 0;
                        Position position = new(i, j);
                        Position iterator = position + offset;
                        while (iterator.IsOnBoard())
                        {
                            count++;
                            iterator += offset;
                        }
                        result.Add(new(position, offset), count);
                    }
                }
            }
            return result;
        }

        private static IDictionary<Position, ulong> GenerateKnightAttackMasks()
        {
            Dictionary<Position, ulong> knightKnightAttackMask = new(BoardConsts.NumberOfPositions);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Position position = new(i, j);
                    ulong mask = 0;
                    foreach (Movement movement in KnightMovements[position])
                    {
                        mask |= 1ul << movement.NewPosition.ConvertToMaskIndex();
                    }
                    knightKnightAttackMask.Add(position, mask);
                }
            }
            return knightKnightAttackMask;
        }

        private static IDictionary<Colour, IReadOnlyDictionary<Position, ulong>> GenereatePawnAttackMasks()
        {
            Dictionary<Colour, IReadOnlyDictionary<Position, ulong>> result = new();
            foreach (Colour colour in PawnQuietMovements.Keys)
            {
                Dictionary<Position, ulong> dictionary = new();
                for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
                {
                    for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                    {
                        Position position = new(i, j);
                        ulong mask = 0;
                        foreach (Movement movement in PawnClassicCaptureOnLowerSideMovements[colour][position].Concat(PawnClassicCaptureOnUpperSideMovements[colour][position]))
                        {
                            mask |= 1ul << movement.NewPosition.ConvertToMaskIndex();
                        }
                        dictionary.Add(position, mask);
                    }
                }
                result.Add(colour, new ReadOnlyDictionary<Position, ulong>(dictionary));
            }
            return result;
        }

        private static IDictionary<Position, ulong> GenerateKingAttackMasks()
        {
            Dictionary<Position, ulong> kingAttackMask = new(BoardConsts.NumberOfPositions);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Position position = new(i, j);
                    ulong mask = 0;
                    foreach (Movement movement in KingClassicMovements[position])
                    {
                        mask |= 1ul << movement.NewPosition.ConvertToMaskIndex();
                    }
                    kingAttackMask.Add(position, mask);
                }
            }
            return kingAttackMask;
        }
    }
}
