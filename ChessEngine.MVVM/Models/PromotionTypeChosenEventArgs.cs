using ChessEngine.Core.Environment;

namespace ChessEngine.MVVM.Models
{
    public class PromotionTypeChosenEventArgs : EventArgs
    {
        public PieceType PromotionType { get; protected init; }

        public PromotionTypeChosenEventArgs(PieceType promotionType)
        {
            PromotionType = promotionType;
        }
    }
}
