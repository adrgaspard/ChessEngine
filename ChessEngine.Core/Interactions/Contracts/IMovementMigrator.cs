using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Contracts
{
    public interface IMovementMigrator
    {
        void Up(Game game, Movement movement);

        void Down(Game game, Movement movement);
    }
}
