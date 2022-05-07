using ChessEngine.Core.Environment;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Contracts
{
    public interface IEvaluator
    {
        int Evaluate(Game game, Colour currentSide);
    }
}
