using ChessEngine.Core.Environment;
using ChessEngine.MVVM.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFChessRankViewModel : ViewModelBase
    {
        public WPFChessGameViewModel GameVM { get; init; }

        public int Rank { get; init; }

        public IList<WPFChessPositionViewModel> PositionVMs { get; init; }

        public WPFChessRankViewModel(WPFChessGameViewModel gameVM, int rank, IEnumerable<WPFChessPositionViewModel> positionVMs)
        {
            GameVM = gameVM;
            Rank = rank;
            PositionVMs = new ReadOnlyCollection<WPFChessPositionViewModel>(positionVMs.ToList());
            foreach (WPFChessPositionViewModel positionVM in PositionVMs)
            {
                if (positionVM.Position.Rank != rank)
                {
                    throw new ArgumentException($"At least one {nameof(WPFChessPositionViewModel)} has a {nameof(Position)} with a different {nameof(Position.Rank)} than {Rank}.");
                }
            }
        }
    }
}
