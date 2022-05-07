using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;
using ChessEngine.Core.Utils;

namespace ChessEngine.Core.AI
{
    public class RandomChessAI : IChessAI
    {
        protected IAttackDataGenerator AttackDataGenerator { get; init; }

        protected IMovementGenerator MovementGenerator { get; init; }

        protected int DelayInMs { get; init; }

        public RandomChessAI(IAttackDataGenerator attackDataGenerator, IMovementGenerator movementGenerator, int delayInMs)
        {
            AttackDataGenerator = attackDataGenerator;
            MovementGenerator = movementGenerator;
            DelayInMs = delayInMs;
        }

        public Movement SelectMovement(Game game, CancellationToken cancellationToken)
        {
            IList<Movement> movements = MovementGenerator.GenerateMovements(game, AttackDataGenerator.GenerateAttackData(game));
            if (DelayInMs > 0)
            {
                Task.Delay(DelayInMs, cancellationToken).Wait(cancellationToken);
            }
            return movements[Randomizer.Instance.Next(movements.Count)];
        }
    }
}
