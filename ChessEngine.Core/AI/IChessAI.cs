using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.AI
{
    public interface IChessAI
    {
        Movement SelectMovement(Game game, IList<Movement> legalMovements);
    }
}
