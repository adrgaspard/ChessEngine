using ChessEngine.Core.Environment;

namespace ChessEngine.Core.Match
{
    public class Game
    {
        public readonly Board Board;
        public readonly Stack<GameState> StateHistory;
        public readonly IDictionary<ulong, byte> HashsHistory;

        public GameState State { get; set; }

        public Colour CurrentPlayer { get; set; }

        public Colour OpponentPlayer => CurrentPlayer == Colour.White ? Colour.Black : Colour.White;

        public uint TotalMoveCounter { get; set; }

        public ulong Hash { get; set; }

        public Game(Board board, Stack<GameState> gameStateHistory, Colour currentPlayer, uint totalMoveCounter, ulong currentHash)
        {
            Board = board;
            StateHistory = new(gameStateHistory);
            HashsHistory = new Dictionary<ulong, byte>();
            State = StateHistory.Peek();
            CurrentPlayer = currentPlayer;
            TotalMoveCounter = totalMoveCounter;
            Hash = currentHash;
            HashsHistory[currentHash] = 1;
        }
    }
}
