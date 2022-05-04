using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;
using ChessEngine.Core.Utils;

namespace ChessEngine.Core.AI
{
    public class RandomChessAI : IChessAI
    {
        public Movement SelectMovement(Game game, IList<Movement> legalMovements)
        {
            return legalMovements[Randomizer.Instance.Next(legalMovements.Count)];
        }
    }
}
