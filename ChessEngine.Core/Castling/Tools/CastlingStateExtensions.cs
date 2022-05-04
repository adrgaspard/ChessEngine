using ChessEngine.Core.Environment;

namespace ChessEngine.Core.Castling.Tools
{
    public static class CastlingStateExtensions
    {
        public static bool CanCastle(this CastlingState source, Colour colour, CastlingSide side)
        {
            return side == CastlingSide.KingSide
                ? (colour == Colour.White ? (source & CastlingState.WhiteCanCastleOnKingSide) != 0 : (source & CastlingState.BlackCanCastleOnKingSide) != 0)
                : (colour == Colour.White ? (source & CastlingState.WhiteCanCastleOnQueenSide) != 0 : (source & CastlingState.BlackCanCastleOnQueenSide) != 0);
        }

        public static CastlingState WithPlayerCastleOrKingMove(this CastlingState source, Colour colour)
        {
            return colour == Colour.White
                ? source & ~(CastlingState.WhiteCanCastleOnKingSide | CastlingState.WhiteCanCastleOnQueenSide)
                : source & ~(CastlingState.BlackCanCastleOnKingSide | CastlingState.BlackCanCastleOnQueenSide);
        }

        public static CastlingState WithPlayerRookMove(this CastlingState source, Colour colour, CastlingSide side)
        {
            return colour == Colour.White
                ? (side == CastlingSide.KingSide ? source & ~CastlingState.WhiteCanCastleOnKingSide : source & ~CastlingState.WhiteCanCastleOnQueenSide)
                : (side == CastlingSide.KingSide ? source & ~CastlingState.BlackCanCastleOnKingSide : source & ~CastlingState.BlackCanCastleOnQueenSide);
        }
    }
}
