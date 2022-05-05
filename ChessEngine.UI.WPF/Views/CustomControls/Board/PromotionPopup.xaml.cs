using ChessEngine.MVVM.ViewModels;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    public partial class PromotionPopup : MetroWindow
    {
        public PromotionPopup(PromotionViewModel promotionVM)
        {
            DataContext = promotionVM;
            InitializeComponent();
        }

        protected void OnChoiceValidated(object sender, RoutedEventArgs eventArgs)
        {
            DialogResult = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs eventArgs)
        {
            eventArgs.Cancel = DialogResult == null;
        }
    }
}
