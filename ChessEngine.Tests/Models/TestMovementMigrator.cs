using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Tests.Models
{
    public class TestMovementMigrator : IMovementMigrator
    {
        public readonly List<Movement> MovementHistory;

        protected readonly IMovementMigrator Migrator;

        public TestMovementMigrator(IMovementMigrator migrator)
        {
            Migrator = migrator;
            MovementHistory = new();
        }

        public void Up(Game game, Movement movement)
        {
            MovementHistory.Add(movement);
            Migrator.Up(game, movement);
        }

        public void Down(Game game, Movement movement)
        {
            Migrator.Down(game, movement);
            MovementHistory.Remove(MovementHistory.LastOrDefault());
        }
    }
}
