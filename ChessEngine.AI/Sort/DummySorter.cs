using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Sort
{
    public class DummySorter : ISorter
    {
        public IList<Movement> Sort(Game game, AttackData attackData, IList<Movement> movements)
        {
            return movements;
        }
    }
}
