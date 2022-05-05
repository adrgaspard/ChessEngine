using ChessEngine.MVVM.ViewModels.Abstractions;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Color AppColor => Colors.Gold;

        public string AppName => "ChessEngine";
    }
}
