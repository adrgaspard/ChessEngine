using ChessEngine.Core.Interactions;
using ChessEngine.UI.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ChessEngine.UI.WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ChessPositionUC.xaml
    /// </summary>
    public partial class ChessPositionUC : UserControl
    {
        public ChessPositionUC()
        {
            InitializeComponent();
            Drop += OnDrop;
        }

        protected void OnDrop(object sender, DragEventArgs eventArgs)
        {
            if (eventArgs.Data.GetData(typeof(WPFChessPositionViewModel)) is WPFChessPositionViewModel oldPositionVM && DataContext is WPFChessPositionViewModel newPositionVM)
            {
                oldPositionVM.GameVM.TreatChessMovementRequestCommand.Execute(new Movement(oldPositionVM.Position, newPositionVM.Position, MovementFlag.None));
            }
        }
    }
}
