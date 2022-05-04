using ChessEngine.Core.Castling;
using ChessEngine.Core.Castling.Tools;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Migration
{
    public partial class MovementMigrator : IMovementMigrator
    {
        public void Up(Game game, Movement movement)
        {
            ulong hash = game.Hash;
            Position oldEnPassantFile = game.State.EnPassantTarget;
            byte oldHalfMoveCounter = game.State.HalfMoveCounter;
            Position newEnPassantTarget = BoardConsts.NoPosition;
            Piece capturedPiece = PieceConsts.NoPiece;
            CastlingState oldCastleState = game.State.CastlingState;
            CastlingState newCastleState = oldCastleState;
            PieceType capturedPieceType = game.Board[movement.NewPosition].Type;
            Piece movePiece = game.Board[movement.OldPosition];
            bool isPromotion = (movement.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None;
            bool isEnPassant = movement.Flag == MovementFlag.PawnEnPassantCapture;
            if (capturedPieceType != PieceType.None && !isEnPassant)
            {
                capturedPiece = game.Board[movement.NewPosition];
                hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.NewPosition, capturedPiece);
            }
            if (movePiece.Type == PieceType.King)
            {
                newCastleState = newCastleState.WithPlayerCastleOrKingMove(game.CurrentPlayer);
            }
            Piece pieceOnTargetPosition = movePiece;
            if (isPromotion)
            {
                PieceType promoteType = PieceType.None;
                switch (movement.Flag)
                {
                    case MovementFlag.PawnPromotionToQueen:
                        promoteType = PieceType.Queen;
                        break;
                    case MovementFlag.PawnPromotionToRook:
                        promoteType = PieceType.Rook;
                        break;
                    case MovementFlag.PawnPromotionToBishop:
                        promoteType = PieceType.Bishop;
                        break;
                    case MovementFlag.PawnPromotionToKnight:
                        promoteType = PieceType.Knight;
                        break;
                }
                pieceOnTargetPosition = new(promoteType, game.CurrentPlayer);
            }
            else
            {
                switch (movement.Flag)
                {
                    case MovementFlag.PawnEnPassantCapture:
                        Position enPassantPosition = new((sbyte)(movement.NewPosition.Rank + BoardConsts.PawnDirections[game.OpponentPlayer]), movement.NewPosition.File);
                        capturedPiece = game.Board[enPassantPosition];
                        game.Board[enPassantPosition] = PieceConsts.NoPiece;
                        hash = GameHashing.IncrementHashOnBoardUpdate(hash, enPassantPosition, capturedPiece);
                        break;
                    case MovementFlag.KingCastling:
                        CastlingSide castlingSide = CastlingConsts.CastlingSideByKingFileAfterCastlingIndexes[movement.NewPosition.File];
                        sbyte castlingRookInitialFile = CastlingConsts.RookInitialFileByCastlingSide[castlingSide];
                        sbyte castlingRookFileAfterCastling = CastlingConsts.RookFileAfterCastlingByCastlingSide[castlingSide];
                        Piece rook = new(PieceType.Rook, game.CurrentPlayer);
                        Position rookInitialPosition = new(movement.NewPosition.Rank, castlingRookInitialFile);
                        Position rookPositionAfterCastling = new(movement.NewPosition.Rank, castlingRookFileAfterCastling);
                        game.Board[rookInitialPosition] = PieceConsts.NoPiece;
                        game.Board[rookPositionAfterCastling] = rook;
                        hash = GameHashing.IncrementHashOnBoardUpdate(hash, rookInitialPosition, rook);
                        hash = GameHashing.IncrementHashOnBoardUpdate(hash, rookPositionAfterCastling, rook);
                        break;
                }
            }
            game.Board[movement.NewPosition] = pieceOnTargetPosition;
            game.Board[movement.OldPosition] = PieceConsts.NoPiece;
            if (movement.Flag == MovementFlag.PawnPush)
            {
                newEnPassantTarget = new((sbyte)(movement.OldPosition.Rank + BoardConsts.PawnDirections[game.CurrentPlayer]), movement.OldPosition.File);
                hash = GameHashing.IncrementHashOnPawnPushUpdate(hash, newEnPassantTarget);
            }
            if (oldCastleState != CastlingState.None)
            {
                if (movement.NewPosition == BoardConsts.KingSideWhiteRookInitialPosition || movement.OldPosition == BoardConsts.KingSideWhiteRookInitialPosition)
                {
                    newCastleState = newCastleState.WithPlayerRookMove(Colour.White, CastlingSide.KingSide);
                }
                if (movement.NewPosition == BoardConsts.QueenSideWhiteRookInitialPosition || movement.OldPosition == BoardConsts.QueenSideWhiteRookInitialPosition)
                {
                    newCastleState = newCastleState.WithPlayerRookMove(Colour.White, CastlingSide.QueenSide);
                }
                if (movement.NewPosition == BoardConsts.KingSideBlackRookInitialPosition || movement.OldPosition == BoardConsts.KingSideBlackRookInitialPosition)
                {
                    newCastleState = newCastleState.WithPlayerRookMove(Colour.Black, CastlingSide.KingSide);
                }
                if (movement.NewPosition == BoardConsts.QueenSideBlackRookInitialPosition || movement.OldPosition == BoardConsts.QueenSideBlackRookInitialPosition)
                {
                    newCastleState = newCastleState.WithPlayerRookMove(Colour.Black, CastlingSide.QueenSide);
                }
            }
            hash = GameHashing.IncrementHashOnCurrentPlayerUpdate(hash);
            hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.OldPosition, movePiece);
            hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.NewPosition, pieceOnTargetPosition);
            if (oldEnPassantFile != BoardConsts.NoPosition)
            {
                hash = GameHashing.IncrementHashOnPawnPushUpdate(hash, oldEnPassantFile);
            }
            if (newCastleState != oldCastleState)
            {
                hash = GameHashing.IncrementHashOnCastlingStateUpdate(hash, oldCastleState);
                hash = GameHashing.IncrementHashOnCastlingStateUpdate(hash, newCastleState);
            }
            game.CurrentPlayer = game.OpponentPlayer;
            game.TotalMoveCounter++;
            oldHalfMoveCounter++;
            game.Hash = hash;
            if (RecordMovesInGameHistory)
            {
                if (movePiece.Type == PieceType.Pawn || capturedPieceType != PieceType.None)
                {
                    game.HashsHistory.Clear();
                    oldHalfMoveCounter = 0;
                }
                else
                {
                    if (game.HashsHistory.ContainsKey(hash))
                    {
                        game.HashsHistory[hash]++;
                    }
                    else
                    {
                        game.HashsHistory[hash] = 1;
                    }
                }
            }
            GameState gameState = new(newCastleState, newEnPassantTarget, capturedPiece, oldHalfMoveCounter);
            game.StateHistory.Push(gameState);
            game.State = gameState;
        }

    }
}
