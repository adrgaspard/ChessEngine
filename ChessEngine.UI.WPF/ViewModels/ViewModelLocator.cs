using ChessEngine.Core.Transposition.Zobrist;
using ChessEngine.MVVM.ViewModels;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainVM { get; init; }

        public WPFGameViewModel GameVM { get; init; }

        public PromotionViewModel PromotionVM { get; init; }

        public ViewModelLocator()
        {
            MainVM = new();
            GameVM = new(new ZobristHashing());
            PromotionVM = new();
        }
    }
}
