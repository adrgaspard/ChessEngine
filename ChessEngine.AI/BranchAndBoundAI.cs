using ChessEngine.AI.Contracts;
using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.AI
{
    public class BranchAndBoundAI : IChessAI
    {
        protected IResearcher Researcher { get; init; }

        protected int SearchDepth { get; init; }

        public BranchAndBoundAI(IResearcher researcher, int searchDepth)
        {
            Researcher = researcher;
            SearchDepth = searchDepth;
        }

        public Movement SelectMovement(Game game, CancellationToken token)
        {
            return Researcher.Search(game, SearchDepth, token);
        }
    }
}
