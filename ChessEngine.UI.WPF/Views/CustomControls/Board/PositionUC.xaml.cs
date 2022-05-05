using ChessEngine.Core.Environment.Tools;
using ChessEngine.UI.WPF.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    /// <summary>
    /// Logique d'interaction pour PositionUC.xaml
    /// </summary>
    public partial class PositionUC : UserControl
    {
        public WPFPositionViewModel PositionVM { get; protected init; }

        protected Panel RootElement { get; init; }

        protected UserControl Control { get; init; }

        public PieceUC? PieceUC
        {
            get => Control.Content as PieceUC;
            set => Control.Content = value;
        }

        public PositionUC(WPFPositionViewModel positionVM)
        {
            InitializeComponent();
            DataContext = positionVM;
            PositionVM = positionVM;
            DataContextChanged += OnDataContextChanged;
            RootElement = LogicalTreeHelper.GetChildren(this).Cast<Panel>().First();
            Control = LogicalTreeHelper.GetChildren(RootElement).Cast<FrameworkElement>().Where(element => element.GetType() == typeof(UserControl)).Cast<UserControl>().First();
            UpdatePieceUC();
            PositionVM.PropertyChanged += OnPositionVMPropertyChanged;
        }

        protected void OnPositionVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(WPFPositionViewModel.Piece))
            {
                UpdatePieceUC();
            }
        }

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            throw new NotSupportedException("The datacontext can't be changed");
        }

        public void UpdatePieceUC()
        {
            PieceUC = PositionVM.Piece == PieceConsts.NoPiece ? null : new(PositionVM.Piece);
        }
    }
}
