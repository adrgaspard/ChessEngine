using ChessEngine.UI.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessEngine.UI.WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ChessPieceUC.xaml
    /// </summary>
    public partial class ChessPieceUC : UserControl
    {
        public ChessPieceUC()
        {
            InitializeComponent();
            MouseMove += OnMouseMove;
        }

        protected void OnMouseMove(object sender, MouseEventArgs eventArgs)
        {
            if (eventArgs.LeftButton == MouseButtonState.Pressed && sender is ChessPieceUC userControl && DataContext is WPFChessPositionViewModel vm)
            {
                vm.GameVM.DragedChessPosition = vm.Position;
                vm.GameVM.IsOnDragDrop = true;
                DragDrop.DoDragDrop(userControl, DataContext, DragDropEffects.Copy | DragDropEffects.Move);
                vm.GameVM.IsOnDragDrop = false;
            }
        }
    }
}
