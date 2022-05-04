using ChessEngine.MVVM.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Color AppColor => Colors.Gold;

        public string AppName => "ChessEngine";
    }
}
