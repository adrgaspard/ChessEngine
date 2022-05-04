﻿using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessEngine.UI.WPF.Views.CustomControls.Board
{
    public partial class BoardUC : UserControl
    {
        protected WPFGameViewModel? GameVM { get; set; }

        protected Panel RootElement { get; init; }

        protected Grid Grid { get; init; }

        protected Canvas Canvas { get; init; }

        protected bool IsMouseDown { get; set; }

        protected bool IsDragAndDrop { get; set; }

        protected bool IsSelected { get; set; }

        protected bool IsDragAndDropInitialization { get; set; }

        protected Point MouseDownPoint { get; set; }

        protected PositionUC? ClickedPositionUC { get; set; }

        protected PieceUC? SelectedPieceUC { get; set; }

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

        public BoardUC()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
            RootElement = LogicalTreeHelper.GetChildren(this).Cast<Panel>().First();
            IList<Panel> panels = LogicalTreeHelper.GetChildren(RootElement).Cast<Panel>().ToList();
            Grid = (Grid)panels.First(panel => panel.GetType() == typeof(Grid));
            Canvas = (Canvas)panels.First(panel => panel.GetType() == typeof(Canvas));
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                Grid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
                Grid.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });
            }
            PointOfView = Colour.Black;
        }

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            Grid.Children.Clear();
            GameVM = eventArgs.NewValue as WPFGameViewModel;
            if (GameVM is not null)
            {
                for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
                {
                    for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                    {
                        PositionUC child = new(GameVM.PositionVMList[i][j]);
                        Grid.Children.Add(child);
                    }
                }
                UpdateBoardPointOfView();
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
                if (ClickedPositionUC is null || (GameVM.Game.Board[ClickedPositionUC.PositionVM.Position].Colour & gameVM.DrageablePieces) == Colour.None) { return; }
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
                Movement movement;
                PositionUC? clickedPositionUC = GetPositionUserControlAt(MouseDownPoint);
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
                        // TODO : Show promotion menu eventually.
                        movement = new(clickedPositionUC.PositionVM.Position, positionUC.PositionVM.Position, MovementFlag.None);
                        GameVM.TreatChessMovementRequestCommand.Execute(movement);
                        clickedPositionUC.UpdatePieceUC();
                    }
                    else
                    {
                        IsSelected = true;
                    }
                }
                else
                {
                    Canvas.Children.Remove(SelectedPieceUC);
                    // TODO : Show promotion menu eventually.
                    movement = new(clickedPositionUC.PositionVM.Position, positionUC.PositionVM.Position, MovementFlag.None);
                    GameVM.TreatChessMovementRequestCommand.Execute(movement);
                    clickedPositionUC.UpdatePieceUC();
                    IsSelected = false;
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
                ClickedPositionUC.UpdatePieceUC();
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

        protected PositionUC? GetPositionUserControlAt(Point point)
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
            return Grid.Children.OfType<UIElement>().First(element => Grid.GetColumn(element) == file && Grid.GetRow(element) == rank) as PositionUC;
        }
    }
}
