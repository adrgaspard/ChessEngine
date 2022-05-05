using ChessEngine.Core.Environment;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

        public ICommand ChoosePromotionTypeCommand { get; protected init; }

        public event EventHandler<PromotionTypeChosenEventArgs>? PromotionTypeChosen;

        public PromotionViewModel()
        {
            AvailablePromotionTypes = new ReadOnlyCollection<PieceType>(new List<PieceType>() { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight });
            SelectedPromotionType = PieceType.Queen;
            ChoosePromotionTypeCommand = new RelayCommand(ChoosePromotionType);
        }

        protected void ChoosePromotionType()
        {
            PromotionTypeChosen?.Invoke(this, new(SelectedPromotionType));
        }
    }
}
