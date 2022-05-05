using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    public partial class BoardUC : UserControl
    {
        public static readonly DependencyProperty PointOfViewProperty = DependencyProperty.Register(nameof(PointOfView), typeof(Colour), typeof(BoardUC), new PropertyMetadata(OnPointOfViewPropertyChanged));

        public Colour PointOfView
        {
            get => (Colour)GetValue(PointOfViewProperty);
            set
            {
                if (PointOfView != value)
                {
                    SetValue(PointOfViewProperty, value);
                    UpdateBoardPointOfView();
                }
            }
        }

        protected WPFGameViewModel? GameVM { get; set; }

        protected Panel RootElement { get; init; }

        protected Grid Grid { get; init; }

        protected Canvas Canvas { get; init; }

        protected IDictionary<Position, PositionUC> Childrens { get; set; }

        protected bool IsMouseDown { get; set; }

        protected bool IsDragAndDrop { get; set; }

        protected bool IsSelected { get; set; }

        protected bool IsDragAndDropInitialization { get; set; }

        protected Point MouseDownPoint { get; set; }

        protected PositionUC ClickedPositionUC { get; set; }

        protected PieceUC? SelectedPieceUC { get; set; }

        protected Movement InitialRequest { get; set; }

        public BoardUC()
        {
            InitializeComponent();
            Childrens = new Dictionary<Position, PositionUC>();
            DataContextChanged += OnDataContextChanged;
            RootElement = LogicalTreeHelper.GetChildren(this).Cast<Panel>().First();
            IList<Panel> panels = LogicalTreeHelper.GetChildren(RootElement).Cast<Panel>().ToList();
            Grid = (Grid)panels.First(panel => panel.GetType() == typeof(Grid));
            Canvas = (Canvas)panels.First(panel => panel.GetType() == typeof(Canvas));
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                Grid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
                Grid.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });
            }
            PointOfView = Colour.White;
        }

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            Grid.Children.Clear();
            Childrens.Clear();
            if (eventArgs.OldValue is WPFGameViewModel oldGameVM)
            {
                oldGameVM.PropertyChanged -= OnGameVMPropertyChanged;
            }
            GameVM = eventArgs.NewValue as WPFGameViewModel;
            if (GameVM is not null)
            {
                GameVM.PropertyChanged += OnGameVMPropertyChanged;
                for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
                {
                    for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                    {
                        PositionUC child = new(GameVM.PositionVMList[i][j]);
                        Grid.Children.Add(child);
                        Childrens.Add(new(i, j), child);
                    }
                }
                UpdateBoardPointOfView();
            }
        }

        protected void OnGameVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(WPFGameViewModel.NeedPromotionTypeSpecification) && GameVM?.NeedPromotionTypeSpecification is true)
            {
                PromotionViewModel promotionVM = new();
                PromotionPopup promotionPopup = new(promotionVM);
                promotionPopup.Owner = Window.GetWindow(this);
                Childrens[InitialRequest.NewPosition].UpdatePieceUCFromPiece(new(PieceType.Pawn, GameVM.Game.CurrentPlayer));
                promotionPopup.ShowDialog();
                MovementFlag flag = promotionVM.SelectedPromotionType switch
                {
                    PieceType.Knight => MovementFlag.PawnPromotionToKnight,
                    PieceType.Bishop => MovementFlag.PawnPromotionToBishop,
                    PieceType.Rook => MovementFlag.PawnPromotionToRook,
                    PieceType.Queen => MovementFlag.PawnPromotionToQueen,
                    _ => throw new NotSupportedException($"The selected promotion type is not valid."),
                };
                GameVM.PlayerVM.TreatMovementRequestCommand.Execute(new Movement(InitialRequest.OldPosition, InitialRequest.NewPosition, flag));
            }

        }

        protected static void OnPointOfViewPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if ((Colour)eventArgs.NewValue != Colour.None)
            {
                (dependencyObject as BoardUC)?.UpdateBoardPointOfView();
            }
        }

        protected void UpdateBoardPointOfView()
        {
            foreach (PositionUC positionUC in Grid.Children.Cast<PositionUC>())
            {
                switch (PointOfView)
                {
                    case Colour.White:
                        Grid.SetRow(positionUC, BoardConsts.BoardSize - positionUC.PositionVM.Position.Rank - 1);
                        Grid.SetColumn(positionUC, positionUC.PositionVM.Position.File);
                        break;
                    case Colour.Black:
                        Grid.SetRow(positionUC, positionUC.PositionVM.Position.Rank);
                        Grid.SetColumn(positionUC, BoardConsts.BoardSize - positionUC.PositionVM.Position.File - 1);
                        break;
                    default:
                        throw new NotSupportedException($"{PointOfView} is not supported by {nameof(BoardUC)}.");
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs eventArgs)
        {
            base.OnMouseDown(eventArgs);
            if (eventArgs.ChangedButton == MouseButton.Left && GameVM is WPFGameViewModel gameVM)
            {
                IsMouseDown = true;
                MouseDownPoint = eventArgs.GetPosition(Grid);
                ClickedPositionUC = GetPositionUserControlAt(eventArgs.GetPosition(Grid));
                if (ClickedPositionUC is null || (GameVM.Game.Board[ClickedPositionUC.PositionVM.Position].Colour & gameVM.PlayerVM.PlayableColour) == Colour.None) { return; }
                gameVM.SelectedPosition = ClickedPositionUC.PositionVM.Position;
                SelectedPieceUC = ClickedPositionUC.PieceUC;
                IsDragAndDropInitialization = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs eventArgs)
        {
            base.OnMouseUp(eventArgs);
            if (eventArgs.ChangedButton == MouseButton.Left && GameVM is WPFGameViewModel gameVM)
            {
                IsMouseDown = false;
                IsDragAndDropInitialization = false;
                gameVM.SelectedPosition = BoardConsts.NoPosition;
                PositionUC? positionUC = GetPositionUserControlAt(eventArgs.GetPosition(Grid));
                if (positionUC is null)
                {
                    if (SelectedPieceUC is null) { return; }
                    Canvas.Children.Remove(SelectedPieceUC);
                    SelectedPieceUC = null;
                    return;
                }
                PositionUC clickedPositionUC = GetPositionUserControlAt(MouseDownPoint);
                if (positionUC.PositionVM.Position == clickedPositionUC.PositionVM.Position)
                {
                    if (IsDragAndDrop)
                    {
                        Canvas.Children.Remove(SelectedPieceUC);
                        ClickedPositionUC.PieceUC = SelectedPieceUC;
                        IsSelected = false;
                        SelectedPieceUC = null;
                        IsDragAndDrop = false;
                        return;
                    }
                    if (IsSelected)
                    {
                        IsSelected = false;
                        InitialRequest = new(clickedPositionUC.PositionVM.Position, positionUC.PositionVM.Position, MovementFlag.None);
                        GameVM.PlayerVM.TreatMovementRequestCommand.Execute(InitialRequest);
                        clickedPositionUC.UpdatePieceUCFromViewModel();
                    }
                    else
                    {
                        IsSelected = true;
                    }
                }
                else
                {
                    Canvas.Children.Remove(SelectedPieceUC);
                    IsSelected = false;
                    InitialRequest = new(clickedPositionUC.PositionVM.Position, positionUC.PositionVM.Position, MovementFlag.None);
                    GameVM.PlayerVM.TreatMovementRequestCommand.Execute(InitialRequest);
                    clickedPositionUC.UpdatePieceUCFromViewModel();
                    IsDragAndDrop = false;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs eventArgs)
        {
            base.OnMouseMove(eventArgs);
            Point point = eventArgs.GetPosition(Grid);
            if (point.X < 0 || point.Y < 0 || point.X > Grid.ActualWidth || point.Y > Grid.ActualHeight)
            {
                if (GameVM is WPFGameViewModel gameVM)
                {
                    gameVM.SelectedPosition = BoardConsts.NoPosition;
                }
                if (SelectedPieceUC is null) { return; }
                Canvas.Children.Remove(SelectedPieceUC);
                ClickedPositionUC.UpdatePieceUCFromViewModel();
                SelectedPieceUC = null;
                IsSelected = false;
                return;
            }
            if (!IsMouseDown || SelectedPieceUC is null) { return; }
            if (IsDragAndDropInitialization && (MouseDownPoint - eventArgs.GetPosition(Grid)).Length > 5)
            {
                ClickedPositionUC.PieceUC = null;
                double width = SelectedPieceUC.ActualWidth;
                double height = SelectedPieceUC.ActualHeight;
                Canvas.Children.Add(SelectedPieceUC);
                SelectedPieceUC.Width = width;
                SelectedPieceUC.Height = height;
                IsDragAndDropInitialization = false;
                IsDragAndDrop = true;
            }
            Canvas.SetLeft(SelectedPieceUC, eventArgs.GetPosition(this).X - SelectedPieceUC.ActualWidth / 2);
            Canvas.SetTop(SelectedPieceUC, eventArgs.GetPosition(this).Y - SelectedPieceUC.ActualHeight / 2);
        }

        protected PositionUC GetPositionUserControlAt(Point point)
        {
            int rank = 0;
            int file = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;
            foreach (RowDefinition rowDefinition in Grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                {
                    break;
                }
                rank++;
            }
            foreach (ColumnDefinition columnDefinition in Grid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                {
                    break;
                }
                file++;
            }
            return (PositionUC)Grid.Children.OfType<UIElement>().First(element => Grid.GetColumn(element) == file && Grid.GetRow(element) == rank);
        }
    }
}
