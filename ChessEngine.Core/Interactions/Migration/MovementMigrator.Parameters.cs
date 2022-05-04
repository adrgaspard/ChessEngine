using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Transposition.Contracts;

namespace ChessEngine.Core.Interactions.Migration
{
    public partial class MovementMigrator : IMovementMigrator
    {
        public readonly bool RecordMovesInGameHistory;
        protected readonly IGameHashing<ulong> GameHashing;

        public MovementMigrator(IGameHashing<ulong> gameHashing, bool recordMovesInGameHistory)
        {
            GameHashing = gameHashing;
            RecordMovesInGameHistory = recordMovesInGameHistory;
        }
    }
}
