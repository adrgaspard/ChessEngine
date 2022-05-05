using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    /// <summary>
    /// Logique d'interaction pour PromotionPopup.xaml
    /// </summary>
    public partial class PromotionPopup : MetroWindow
    {
        public PromotionPopup()
        {
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
