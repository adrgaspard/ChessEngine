using ChessEngine.Core.Transposition.Zobrist;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainVM { get; init; }

        public WPFChessGameViewModel GameVM { get; init; }

        public WPFGameViewModel GameVM2 { get; init; }

        public ViewModelLocator()
        {
            MainVM = new();
            GameVM = new(new ZobristHashing());
            GameVM2 = new(new ZobristHashing());
        }
    }
}
