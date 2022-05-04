using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Contracts
{
    public interface IEndGameChecker
    {
        EndGameData CheckEndGame(Game game, AttackData attackData, IEnumerable<Movement> legalMovements);
    }
}
