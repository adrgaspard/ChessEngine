using ChessEngine.Core.Transposition.Zobrist;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainVM { get; init; }

        public WPFGameViewModel GameVM { get; init; }

        public ViewModelLocator()
        {
            MainVM = new();
            GameVM = new(new ZobristHashing());
        }
    }
}
