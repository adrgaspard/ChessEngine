using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.UI.WPF.Models;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFGameManagerViewModel : GameManagerViewModel
    {
        public WPFGameManagerViewModel() : base(new DispatcherService(App.Current.Dispatcher))
        {
        }

        protected override GameViewModel GenerateNewGameViewModel()
        {
            return new WPFGameViewModel(GameLoader.Load(FENConsts.StartFEN), GameHashing, GetPlayersFactory(), GetClocksFactory());
        }
    }
}
