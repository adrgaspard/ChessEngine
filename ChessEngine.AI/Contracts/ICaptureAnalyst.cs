using ChessEngine.Core.Match;

namespace ChessEngine.AI.Contracts
{
    public interface ICaptureAnalyst
    {
        int SearchCaptures(Game game, int alpha, int beta);
    }
}
