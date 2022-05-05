using ChessEngine.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    /// <summary>
    /// Logique d'interaction pour PieceUC.xaml
    /// </summary>
    public partial class PieceUC : UserControl
    {
        protected Panel RootElement { get; init; }

        protected Image Image { get; init; }

        protected Piece Piece { get; init; }

        public PieceUC(Piece piece)
        {
            InitializeComponent();
            Piece = piece;
            RootElement = LogicalTreeHelper.GetChildren(this).Cast<Panel>().First();
            IList<FrameworkElement> elements = LogicalTreeHelper.GetChildren(RootElement).Cast<FrameworkElement>().ToList();
            Image = (Image)elements.First(element => element.GetType() == typeof(Image));
            Image.Source = piece.Type switch
            {
                PieceType.King => (Piece.Colour == Colour.Black ? FindResource("BlackKing") : FindResource("WhiteKing")) as ImageSource,
                PieceType.Pawn => (Piece.Colour == Colour.Black ? FindResource("BlackPawn") : FindResource("WhitePawn")) as ImageSource,
                PieceType.Knight => (Piece.Colour == Colour.Black ? FindResource("BlackKnight") : FindResource("WhiteKnight")) as ImageSource,
                PieceType.Bishop => (Piece.Colour == Colour.Black ? FindResource("BlackBishop") : FindResource("WhiteBishop")) as ImageSource,
                PieceType.Rook => (Piece.Colour == Colour.Black ? FindResource("BlackRook") : FindResource("WhiteRook")) as ImageSource,
                PieceType.Queen => (Piece.Colour == Colour.Black ? FindResource("BlackQueen") : FindResource("WhiteQueen")) as ImageSource,
                _ => throw new NotSupportedException($"{piece.Type} is not supported by {nameof(PieceUC)}"),
            };
        }
    }
}
