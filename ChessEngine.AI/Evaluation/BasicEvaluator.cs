using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Evaluation
{
    public class BasicEvaluator : EvaluatorBase
    {
        public BasicEvaluator(IPieceEvaluator pieceEvaluator) : base(pieceEvaluator)
        {
        }

        public override int Evaluate(Game game, Colour aiSide)
        {
            int whiteEvaluation = CountMaterial(game.Board, Colour.White);
            int blackEvaluation = CountMaterial(game.Board, Colour.Black);
            int evaluation = whiteEvaluation - blackEvaluation;
            return evaluation * (game.CurrentPlayer == Colour.White ? 1 : -1);
        }
    }
}
