namespace ChessEngine.UI.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainVM { get; init; }

        public WPFGameManagerViewModel GameManagerVM { get; init; }

        public ViewModelLocator()
        {
            MainVM = new();
            GameManagerVM = new();
        }
    }
}
