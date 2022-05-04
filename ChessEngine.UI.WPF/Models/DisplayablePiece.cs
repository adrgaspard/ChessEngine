using ChessEngine.Core.Environment;

namespace ChessEngine.UI.WPF.Models
{
    public class DisplayablePiece
    {
        public PieceType Type { get; protected init; }
        public Colour Colour { get; protected init; }

        public DisplayablePiece(Piece piece)
        {
            Type = piece.Type;
            Colour = piece.Colour;
        }
    }
}
