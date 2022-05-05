using ChessEngine.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
