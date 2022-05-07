using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Contracts
{
    public interface IResearcher
    {
        Movement Search(Game game, int depth, CancellationToken token);
    }
}
