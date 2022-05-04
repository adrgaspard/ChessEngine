using ChessEngine.Core.Castling;
using ChessEngine.Core.Environment;

namespace ChessEngine.Core.Match
{
    public struct GameState
    {
        public readonly CastlingState CastlingState;
        public readonly Position EnPassantTarget;
        public readonly Piece CapturedPiece;
        public readonly byte HalfMoveCounter;

        public GameState(CastlingState castlingLegagility, Position enPassantTarget, Piece capturedPiece, byte fiftyMoveCounter)
        {
            CastlingState = castlingLegagility;
            EnPassantTarget = enPassantTarget;
            CapturedPiece = capturedPiece;
            HalfMoveCounter = fiftyMoveCounter;
        }
    }
}
