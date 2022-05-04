using ChessEngine.Core.Castling;
using ChessEngine.Core.Castling.Tools;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation.Tools;
using ChessEngine.Core.Match;
using static ChessEngine.Core.Interactions.Generation.Caching.MovementGenerationCache;
using static ChessEngine.Core.Interactions.Generation.Tools.MovementGeneratorTools;

namespace ChessEngine.Core.Interactions.Generation
{
    public class MovementGenerator : IMovementGenerator
    {
        public readonly PromotionGenerationType PromotionGenerationType;
        public readonly bool IncludeQuietMovements;

        public MovementGenerator(PromotionGenerationType promotionGenerationType = PromotionGenerationType.AllPromotions, bool includeQuietMovements = true)
        {
            PromotionGenerationType = promotionGenerationType;
            IncludeQuietMovements = includeQuietMovements;
        }

        public IList<Movement> GenerateMovements(Game game, AttackData attackData)
        {
            List<Movement> result = new(64);
            GenerateKingMovements(game, attackData, result);
            if (!attackData.IsDoubleCheck)
            {
                GeneratePawnMovements(game, attackData, result);
                GenerateKnightMovements(game, attackData, result);
                GenerateSlidingMovements(game, attackData, result);
            }
            return result;
        }

        protected void GenerateKingMovements(Game game, AttackData attackData, IList<Movement> result)
        {
            foreach (Movement movement in GetKingMovements(game.Board[game.CurrentPlayer]))
            {
                Position target = movement.NewPosition;
                Piece targetPiece = game.Board[target];
                if (targetPiece.Colour == game.CurrentPlayer)
                {
                    continue;
                }
                bool isCapture = targetPiece.Colour == game.OpponentPlayer;
                if (!isCapture && ((!IncludeQuietMovements) || attackData.IsPositionInCheckRange(target)))
                {
                    continue;
                }
                if (!attackData.IsPositionAttacked(target))
                {
                    if ((movement.Flag & MovementFlag.KingCastling) == MovementFlag.None)
                    {
                        result.Add(movement);
                    }
                    else
                    {
                        if (!attackData.IsCheck && !isCapture)
                        {
                            if (target.File == CastlingConsts.KingFileAfterKingSideCastling && game.State.CastlingState.CanCastle(game.CurrentPlayer, CastlingSide.KingSide))
                            {
                                Piece castlingPiece = game.Board[new Position(target.Rank, CastlingConsts.RookInitialFileByCastlingSide[CastlingSide.KingSide])];
                                Position intermediatePosition = new(target.Rank, CastlingConsts.KingSideCastlingIntermediateFile);
                                if (castlingPiece.Type == PieceType.Rook && castlingPiece.Colour == game.CurrentPlayer &&
                                    game.Board[intermediatePosition] == PieceConsts.NoPiece && !attackData.IsPositionAttacked(intermediatePosition))
                                {
                                    result.Add(movement);
                                }
                            }
                            else if (target.File == CastlingConsts.KingFileAfterQueenSideCastling && game.State.CastlingState.CanCastle(game.CurrentPlayer, CastlingSide.QueenSide))
                            {
                                Piece castlingPiece = game.Board[new Position(target.Rank, CastlingConsts.RookInitialFileByCastlingSide[CastlingSide.QueenSide])];
                                Position intermediatePosition = new(target.Rank, CastlingConsts.QueenSideCastlingIntermediateFile);
                                if (castlingPiece.Type == PieceType.Rook && castlingPiece.Colour == game.CurrentPlayer &&
                                    game.Board[intermediatePosition] == PieceConsts.NoPiece && !attackData.IsPositionAttacked(intermediatePosition) &&
                                    game.Board[new Position(target.Rank, (sbyte)(target.File - 1))] == PieceConsts.NoPiece)
                                {
                                    result.Add(movement);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void GeneratePawnMovements(Game game, AttackData attackData, IList<Movement> result)
        {
            PieceGroup pawns = game.Board[PieceType.Pawn, game.CurrentPlayer];
            for (byte i = 0; i < pawns.CurrentSize; i++)
            {
                Position position = pawns[i];
                if (IncludeQuietMovements)
                {
                    foreach (Movement movement in GetPawnQuietMovements(position, game.CurrentPlayer))
                    {
                        if (game.Board[movement.NewPosition] == PieceConsts.NoPiece &&
                            (!attackData.IsPositionPinned(position) || AreOnDirectionAxe((movement.NewPosition - movement.OldPosition).ToDirection(), position, game.Board[game.CurrentPlayer])))
                        {
                            if (!attackData.IsCheck || attackData.IsPositionInCheckRange(movement.NewPosition))
                            {
                                if (movement.Flag != MovementFlag.PawnPush || game.Board[movement.OldPosition + new Position(BoardConsts.PawnDirections[game.CurrentPlayer], 0)] == PieceConsts.NoPiece)
                                {
                                    AddPawnMovementInFunctionOfPromotionType(result, movement);
                                }
                            }
                        }
                    }
                }
                foreach (Movement movement in GetPawnAllCaptureMovements(position, game.CurrentPlayer))
                {
                    if (attackData.IsPositionPinned(position) && !AreOnDirectionAxe(movement.NewPosition - movement.OldPosition, movement.OldPosition, game.Board[game.CurrentPlayer]))
                    {
                        continue;
                    }
                    if ((movement.Flag & MovementFlag.PawnEnPassantCapture) == MovementFlag.None)
                    {
                        if (game.Board[movement.NewPosition].Colour == game.OpponentPlayer)
                        {
                            if (attackData.IsCheck && !attackData.IsPositionInCheckRange(movement.NewPosition))
                            {
                                continue;
                            }
                            AddPawnMovementInFunctionOfPromotionType(result, movement);
                        }
                    }
                    else
                    {
                        if (movement.NewPosition == game.State.EnPassantTarget)
                        {
                            if (!IsCheckAfterEnPassant(game, attackData, movement.OldPosition, game.State.EnPassantTarget + new Position(BoardConsts.PawnDirections[game.OpponentPlayer], 0)))
                            {
                                result.Add(movement);
                            }
                        }
                    }
                }
            }
        }

        protected void AddPawnMovementInFunctionOfPromotionType(IList<Movement> result, Movement movement)
        {
            if ((movement.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None)
            {
                switch (PromotionGenerationType)
                {
                    case PromotionGenerationType.AllPromotions:
                        result.Add(movement);
                        break;
                    case PromotionGenerationType.PromotionToQueenAndKnightOnly:
                        if ((movement.Flag & MovementFlag.PawnPromotionToQueen | MovementFlag.PawnPromotionToKnight) != MovementFlag.None)
                        {
                            result.Add(movement);
                        }
                        break;
                    case PromotionGenerationType.PromotionToQueenOnly:
                        if ((movement.Flag & MovementFlag.PawnPromotionToQueen) != MovementFlag.None)
                        {
                            result.Add(movement);
                        }
                        break;
                    default:
                        throw new NotSupportedException($"{PromotionGenerationType} is not a supported {nameof(PromotionGenerationType)}.");
                }
            }
            else
            {
                result.Add(movement);
            }
        }

        protected void GenerateKnightMovements(Game game, AttackData attackData, IList<Movement> result)
        {
            PieceGroup knights = game.Board[PieceType.Knight, game.CurrentPlayer];
            for (byte i = 0; i < knights.CurrentSize; i++)
            {
                Position position = knights[i];
                if (attackData.IsPositionPinned(position))
                {
                    continue;
                }
                foreach (Movement movement in GetKnightMovements(position))
                {
                    Position target = movement.NewPosition;
                    Piece targetPiece = game.Board[target];
                    bool isCapture = targetPiece.Colour == game.OpponentPlayer;
                    if (IncludeQuietMovements || isCapture)
                    {
                        if (targetPiece.Colour == game.CurrentPlayer || (attackData.IsCheck && !attackData.IsPositionInCheckRange(target)))
                        {
                            continue;
                        }
                        result.Add(movement);
                    }
                }
            }
        }

        protected void GenerateSlidingMovements(Game game, AttackData attackData, IList<Movement> result)
        {
            PieceGroup bishops = game.Board[PieceType.Bishop, game.CurrentPlayer];
            for (byte i = 0; i < bishops.CurrentSize; i++)
            {
                GenerateSlidingMovements(game, attackData, result, bishops[i], GetBishopGroupedAndOrderedMovements(bishops[i]));
            }
            PieceGroup rooks = game.Board[PieceType.Rook, game.CurrentPlayer];
            for (byte i = 0; i < rooks.CurrentSize; i++)
            {
                GenerateSlidingMovements(game, attackData, result, rooks[i], GetRookGroupedAndOrderedMovements(rooks[i]));
            }
            PieceGroup queens = game.Board[PieceType.Queen, game.CurrentPlayer];
            for (byte i = 0; i < queens.CurrentSize; i++)
            {
                GenerateSlidingMovements(game, attackData, result, queens[i], GetQueenGroupedAndOrderedMovements(queens[i]));
            }
        }

        protected void GenerateSlidingMovements(Game game, AttackData attackData, IList<Movement> result, Position position, IReadOnlyList<Tuple<Position, IReadOnlyList<Movement>>> groupedAndOrderedMovements)
        {
            bool isPinned = attackData.IsPositionPinned(position);
            if (attackData.IsCheck && isPinned)
            {
                return;
            }
            foreach (Tuple<Position, IReadOnlyList<Movement>> offsetOrderedMovements in groupedAndOrderedMovements)
            {
                if (isPinned && !AreOnDirectionAxe(offsetOrderedMovements.Item1, position, game.Board[game.CurrentPlayer]))
                {
                    continue;
                }
                foreach (Movement movement in offsetOrderedMovements.Item2)
                {
                    Position target = movement.NewPosition;
                    Piece targetPiece = game.Board[target];
                    if (targetPiece.Colour == game.CurrentPlayer)
                    {
                        break;
                    }
                    bool isCapture = targetPiece != PieceConsts.NoPiece;
                    bool movePreventsCheck = attackData.IsPositionInCheckRange(target);
                    if ((movePreventsCheck || !attackData.IsCheck) && (IncludeQuietMovements || isCapture))
                    {
                        result.Add(movement);
                    }
                    if (isCapture || movePreventsCheck)
                    {
                        break;
                    }
                }
            }
        }
    }
}
