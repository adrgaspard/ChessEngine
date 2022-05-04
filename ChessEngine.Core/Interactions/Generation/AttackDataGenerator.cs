using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation.Tools;
using ChessEngine.Core.Match;
using static ChessEngine.Core.Interactions.Generation.Caching.MovementGenerationCache;

namespace ChessEngine.Core.Interactions.Generation
{
    public class AttackDataGenerator : IAttackDataGenerator
    {
        public AttackDataGenerator()
        {
            if (BoardConsts.NumberOfPositions > 64)
            {
                throw new NotSupportedException($"The board with more than 64 positions are not supported by the {nameof(AttackDataGenerator)}.");
            }
        }

        public AttackData GenerateAttackData(Game game)
        {
            bool isCheck = false;
            bool isDoubleCheck = false;
            ulong pinMask = 0;
            ulong checkMask = 0;
            ulong slidingPiecesAttackMap = GenerateSlidingPiecesAttackMap(game);
            PieceType offsetsPiece = game.Board[PieceType.Queen, game.OpponentPlayer].CurrentSize == 0
                ? (game.Board[PieceType.Bishop, game.OpponentPlayer].CurrentSize == 0
                    ? (game.Board[PieceType.Rook, game.OpponentPlayer].CurrentSize == 0 ? PieceType.None : PieceType.Rook)
                    : (game.Board[PieceType.Rook, game.OpponentPlayer].CurrentSize == 0 ? PieceType.Bishop : PieceType.Queen))
                : PieceType.Queen;
            foreach (Position offset in offsetsPiece == PieceType.None ? new List<Position>(0) : PieceConsts.SlidingOffsets[offsetsPiece])
            {
                bool isDiagonal = Math.Abs(offset.Rank) == Math.Abs(offset.File);
                bool isCurrentPlayerPieceInterposition = false;
                ulong positionMask = 0;
                Position kingPosition = game.Board[game.CurrentPlayer];
                for (sbyte i = 1; i <= GetRangeSizeWithOffset(kingPosition, offset); i++)
                {
                    Position target = kingPosition + (offset * i);
                    Piece piece = game.Board[target];
                    positionMask |= 1ul << target.ConvertToMaskIndex();
                    if (piece != PieceConsts.NoPiece)
                    {
                        if (piece.Colour == game.CurrentPlayer)
                        {
                            if (isCurrentPlayerPieceInterposition)
                            {
                                break;
                            }
                            else
                            {
                                isCurrentPlayerPieceInterposition = true;
                            }
                        }
                        else
                        {
                            if (isDiagonal && (piece.Type & (PieceType.Queen | PieceType.Bishop)) != 0 || !isDiagonal && (piece.Type & (PieceType.Queen | PieceType.Rook)) != 0)
                            {
                                if (isCurrentPlayerPieceInterposition)
                                {
                                    pinMask |= positionMask;
                                }
                                else
                                {
                                    checkMask |= positionMask;
                                    isDoubleCheck = isCheck;
                                    isCheck = true;
                                }
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (isDoubleCheck)
                {
                    break;
                }
            }

            PieceGroup opponentKnights = game.Board[PieceType.Knight, game.OpponentPlayer];
            ulong knightAttackMap = 0;
            bool isKnightCheck = false;

            for (byte i = 0; i < opponentKnights.CurrentSize; i++)
            {
                Position position = opponentKnights[i];
                knightAttackMap |= GetKnightAttackMask(position);
                if (!isKnightCheck && knightAttackMap.ContainsPosition(game.Board[game.CurrentPlayer]))
                {
                    isKnightCheck = true;
                    isDoubleCheck = isCheck;
                    isCheck = true;
                    checkMask |= 1ul << position.ConvertToMaskIndex();
                }
            }

            PieceGroup opponentPawns = game.Board[PieceType.Pawn, game.OpponentPlayer];
            ulong pawnAttackMap = 0;
            bool isPawnCheck = false;
            for (byte i = 0; i < opponentPawns.CurrentSize; i++)
            {
                Position position = opponentPawns[i];
                ulong currentPawnAttackMap = GetPawnAttackMap(position, game.OpponentPlayer);
                pawnAttackMap |= currentPawnAttackMap;
                if (!isPawnCheck && currentPawnAttackMap.ContainsPosition(game.Board[game.CurrentPlayer]))
                {
                    isPawnCheck = true;
                    isDoubleCheck = isCheck;
                    isCheck = true;
                    checkMask |= 1ul << position.ConvertToMaskIndex();
                }
            }

            ulong attackMapWithoutPawns = slidingPiecesAttackMap | knightAttackMap | GetKingAttackMask(game.Board[game.OpponentPlayer]);
            ulong attackMap = attackMapWithoutPawns | pawnAttackMap;
            return new(attackMap, attackMapWithoutPawns, pinMask, checkMask, isCheck, isDoubleCheck);
        }

        protected static ulong GenerateSlidingPiecesAttackMap(Game game)
        {
            ulong attackMap = 0;
            PieceGroup opponentBishops = game.Board[PieceType.Bishop, game.OpponentPlayer];
            for (byte i = 0; i < opponentBishops.CurrentSize; i++)
            {
                UpdateAttackMapWithSlidingPiece(game, ref attackMap, opponentBishops[i], PieceConsts.SlidingOffsets[PieceType.Bishop]);
            }
            PieceGroup opponentRooks = game.Board[PieceType.Rook, game.OpponentPlayer];
            for (byte i = 0; i < opponentRooks.CurrentSize; i++)
            {
                UpdateAttackMapWithSlidingPiece(game, ref attackMap, opponentRooks[i], PieceConsts.SlidingOffsets[PieceType.Rook]);
            }
            PieceGroup opponentQueens = game.Board[PieceType.Queen, game.OpponentPlayer];
            for (byte i = 0; i < opponentQueens.CurrentSize; i++)
            {
                UpdateAttackMapWithSlidingPiece(game, ref attackMap, opponentQueens[i], PieceConsts.SlidingOffsets[PieceType.Queen]);
            }
            return attackMap;
        }

        protected static void UpdateAttackMapWithSlidingPiece(Game game, ref ulong attackMap, Position position, IReadOnlyList<Position> offsets)
        {
            foreach (Position offset in offsets)
            {
                for (sbyte i = 1; i <= GetRangeSizeWithOffset(position, offset); i++)
                {
                    Position target = position + (offset * i);
                    Piece piece = game.Board[target];
                    attackMap |= 1ul << target.ConvertToMaskIndex();
                    if (piece != PieceConsts.NoPiece && (piece.Type != PieceType.King || piece.Colour != game.CurrentPlayer))
                    {
                        break;
                    }
                }
            }
        }
    }
}
