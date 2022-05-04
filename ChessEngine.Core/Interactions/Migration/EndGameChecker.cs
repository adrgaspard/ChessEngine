using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Generation.Tools;
using ChessEngine.Core.Match;
using static ChessEngine.Core.Match.Tools.GameConsts;

namespace ChessEngine.Core.Interactions.Migration
{
    public class EndGameChecker : IEndGameChecker
    {
        public EndGameData CheckEndGame(Game game, AttackData attackData, IEnumerable<Movement> legalMovements)
        {
            byte numOccurencesInHistory = game.HashsHistory.ContainsKey(game.Hash) ? game.HashsHistory[game.Hash] : (byte)0;
            bool isThreeFoldRepetitionRuleActivated = numOccurencesInHistory >= ThreefoldRepetitionValue;
            bool isFiftyMoveRuleActivated = game.State.HalfMoveCounter >= FiftyMoveValue;
            if (game.State.HalfMoveCounter >= SeventyFiveMoveValue)
            {
                return new(EndGameType.DrawOnSeventyFiveMove, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated);
            }
            if (numOccurencesInHistory >= FivefoldRepetitionValue)
            {
                return new(EndGameType.DrawOnFivefoldRepetition, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated);
            }
            if (!legalMovements.Any())
            {
                if (attackData.IsCheck)
                {
                    return game.CurrentPlayer switch
                    {
                        Colour.White => new(EndGameType.WhiteIsCheckmated, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated),
                        Colour.Black => new(EndGameType.BlackIsCheckmated, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated),
                        _ => throw new NotSupportedException($"The colour {game.CurrentPlayer} is not supported."),
                    };
                }
                else
                {
                    return new(EndGameType.DrawOnStalemate, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated);
                }
            }
            if (game.Board[PieceType.Queen, Colour.White].CurrentSize == 0 && game.Board[PieceType.Queen, Colour.Black].CurrentSize == 0
                && game.Board[PieceType.Rook, Colour.White].CurrentSize == 0 && game.Board[PieceType.Rook, Colour.Black].CurrentSize == 0
                && game.Board[PieceType.Pawn, Colour.White].CurrentSize == 0 && game.Board[PieceType.Pawn, Colour.Black].CurrentSize == 0)
            {
                bool isDeadPosition = game.Board[PieceType.Bishop, Colour.White].CurrentSize == 0 && game.Board[PieceType.Bishop, Colour.Black].CurrentSize == 0
                    && ((game.Board[PieceType.Knight, Colour.White].CurrentSize == 0 && game.Board[PieceType.Knight, Colour.Black].CurrentSize <= 1)
                        || (game.Board[PieceType.Knight, Colour.White].CurrentSize <= 1 && game.Board[PieceType.Knight, Colour.Black].CurrentSize == 0));
                isDeadPosition |= game.Board[PieceType.Knight, Colour.White].CurrentSize == 0 && game.Board[PieceType.Knight, Colour.Black].CurrentSize == 0
                    && ((game.Board[PieceType.Bishop, Colour.White].CurrentSize == 0 && game.Board[PieceType.Bishop, Colour.Black].CurrentSize <= 1)
                        || (game.Board[PieceType.Bishop, Colour.White].CurrentSize <= 1 && game.Board[PieceType.Bishop, Colour.Black].CurrentSize == 0));
                isDeadPosition |= game.Board[PieceType.Knight, Colour.White].CurrentSize == 0 && game.Board[PieceType.Knight, Colour.Black].CurrentSize == 0
                    && game.Board[PieceType.Bishop, Colour.White].CurrentSize == 1 && game.Board[PieceType.Bishop, Colour.Black].CurrentSize == 1
                    && game.Board[PieceType.Bishop, Colour.White][0].ConvertToMaskIndex() % 2 == game.Board[PieceType.Bishop, Colour.Black][0].ConvertToMaskIndex() % 2;
                if (isDeadPosition)
                {
                    return new(EndGameType.DrawOnDeadPosition, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated);
                }
            }
            return new(EndGameType.GameIsNotFinished, isThreeFoldRepetitionRuleActivated, isFiftyMoveRuleActivated);
        }
    }
}
