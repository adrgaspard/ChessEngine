using ChessEngine.Core.Environment;
using ChessEngine.MVVM.ViewModels.Abstractions;
using System.Collections.ObjectModel;

namespace ChessEngine.MVVM.ViewModels
{
    public class PromotionViewModel : ViewModelBase
    {
        public IReadOnlyList<PieceType> AvailablePromotionTypes { get; protected init; }

        protected PieceType selectedPromotionType;
        public PieceType SelectedPromotionType
        {
            get => selectedPromotionType;
            set
            {
                if (selectedPromotionType != value)
                {
                    if (AvailablePromotionTypes.Contains(value))
                    {
                        selectedPromotionType = value;
                        RaisePropertyChanged();
                    }
                    else
                    {
                        throw new ArgumentException($"The value {value} is not included in {nameof(AvailablePromotionTypes)}.");
                    }
                }
            }
        }

        public PromotionViewModel()
        {
            AvailablePromotionTypes = new ReadOnlyCollection<PieceType>(new List<PieceType>() { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight });
            SelectedPromotionType = PieceType.Queen;
        }
    }
}
