using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Contracts
{
    public interface IAttackDataGenerator
    {
        AttackData GenerateAttackData(Game game);
    }
}
