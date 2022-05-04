using ChessEngine.Core.Castling;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Transposition.Contracts
{
    public interface IGameHashing<THash>
    {
        THash GenerateHashValue(Board board, GameState state, Colour currentPlayer);

        THash IncrementHashOnCurrentPlayerUpdate(THash hash);

        THash IncrementHashOnBoardUpdate(THash hash, Position oldOrNewposition, Piece oldOrNewPiece);

        THash IncrementHashOnPawnPushUpdate(THash hash, Position oldOrNewEnPassantTarget);

        THash IncrementHashOnCastlingStateUpdate(THash hash, CastlingState oldOrNewCastlingState);
    }
}
