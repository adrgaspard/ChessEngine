using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;
using System.ComponentModel;

namespace ChessEngine.AI.Abstractions
{
    public abstract class ScoringSorterBase : ISorter
    {
        public ListSortDirection SortDirection { get; protected init; }

        public IPieceEvaluator PieceEvaluator { get; protected init; }

        public ScoringSorterBase(IPieceEvaluator pieceEvaluator, ListSortDirection sortDirection)
        {
            PieceEvaluator = pieceEvaluator;
            SortDirection = sortDirection;
        }

        public IList<Movement> Sort(Game game, AttackData attackData, IList<Movement> movements)
        {
            IEnumerable<Movement> sorted = SortDirection == ListSortDirection.Ascending
                ? movements.OrderBy(movement => GetMovementScore(game, attackData, movement))
                : movements.OrderByDescending(movement => GetMovementScore(game, attackData, movement));
            return sorted.ToList();
        }

        public abstract int GetMovementScore(Game game, AttackData attackData, Movement movement);
    }
}
