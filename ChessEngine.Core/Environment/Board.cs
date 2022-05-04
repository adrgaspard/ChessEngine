using ChessEngine.Core.Environment.Tools;

namespace ChessEngine.Core.Environment
{
    public class Board
    {
        private readonly Piece[,] Squares;
        private readonly Position[] Kings;
        private readonly PieceGroup[] Pawns;
        private readonly PieceGroup[] Knights;
        private readonly PieceGroup[] Bishops;
        private readonly PieceGroup[] Rooks;
        private readonly PieceGroup[] Queens;

        public Piece this[Position position]
        {
            get => Squares[position.Rank, position.File];
            set
            {
                Piece oldPiece = Squares[position.Rank, position.File];
                Squares[position.Rank, position.File] = value;
                byte oldIndex = oldPiece.Colour == Colour.None ? byte.MaxValue : BoardConsts.ColourIndexes[oldPiece.Colour];
                byte newIndex = value.Colour == Colour.None ? byte.MaxValue : BoardConsts.ColourIndexes[value.Colour];
                switch (oldPiece.Type)
                {
                    case PieceType.King:
                        break;
                    case PieceType.Pawn:
                        Pawns[oldIndex].RemovePieceFromPosition(position);
                        break;
                    case PieceType.Knight:
                        Knights[oldIndex].RemovePieceFromPosition(position);
                        break;
                    case PieceType.Bishop:
                        Bishops[oldIndex].RemovePieceFromPosition(position);
                        break;
                    case PieceType.Rook:
                        Rooks[oldIndex].RemovePieceFromPosition(position);
                        break;
                    case PieceType.Queen:
                        Queens[oldIndex].RemovePieceFromPosition(position);
                        break;
                    default:
                        break;
                }
                switch (value.Type)
                {
                    case PieceType.King:
                        Kings[newIndex] = position;
                        break;
                    case PieceType.Pawn:
                        Pawns[newIndex].AddPieceAtPosition(position);
                        break;
                    case PieceType.Knight:
                        Knights[newIndex].AddPieceAtPosition(position);
                        break;
                    case PieceType.Bishop:
                        Bishops[newIndex].AddPieceAtPosition(position);
                        break;
                    case PieceType.Rook:
                        Rooks[newIndex].AddPieceAtPosition(position);
                        break;
                    case PieceType.Queen:
                        Queens[newIndex].AddPieceAtPosition(position);
                        break;
                    default:
                        break;
                }
            }
        }

        public Position this[Colour colour] => Kings[BoardConsts.ColourIndexes[colour]];

        public PieceGroup this[PieceType pieceType, Colour colour] => pieceType switch
        {
            PieceType.King => throw new InvalidOperationException($"To retrieve the position of a {nameof(PieceType.King)}, please use directly the indexer of type [{nameof(Colour)}]."),
            PieceType.Pawn => Pawns[BoardConsts.ColourIndexes[colour]],
            PieceType.Knight => Knights[BoardConsts.ColourIndexes[colour]],
            PieceType.Bishop => Bishops[BoardConsts.ColourIndexes[colour]],
            PieceType.Rook => Rooks[BoardConsts.ColourIndexes[colour]],
            PieceType.Queen => Queens[BoardConsts.ColourIndexes[colour]],
            _ => throw new NotSupportedException($"The piece type {pieceType} is not supported by the board {nameof(PieceGroup)} system."),
        };

        public Board()
        {
            Squares = new Piece[BoardConsts.BoardSize, BoardConsts.BoardSize];
            for (byte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (byte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Squares[i, j] = PieceConsts.NoPiece;
                }
            }
            Kings = new Position[BoardConsts.NumberOfPlayers];
            Pawns = new PieceGroup[BoardConsts.NumberOfPlayers];
            Knights = new PieceGroup[BoardConsts.NumberOfPlayers];
            Bishops = new PieceGroup[BoardConsts.NumberOfPlayers];
            Rooks = new PieceGroup[BoardConsts.NumberOfPlayers];
            Queens = new PieceGroup[BoardConsts.NumberOfPlayers];
            for (byte k = 0; k < BoardConsts.NumberOfPlayers; k++)
            {
                Pawns[k] = new PieceGroup(BoardConsts.MaxInstancesOfPieceOnBoardForOnePlayer[PieceType.Pawn]);
                Knights[k] = new PieceGroup(BoardConsts.MaxInstancesOfPieceOnBoardForOnePlayer[PieceType.Knight]);
                Bishops[k] = new PieceGroup(BoardConsts.MaxInstancesOfPieceOnBoardForOnePlayer[PieceType.Bishop]);
                Rooks[k] = new PieceGroup(BoardConsts.MaxInstancesOfPieceOnBoardForOnePlayer[PieceType.Rook]);
                Queens[k] = new PieceGroup(BoardConsts.MaxInstancesOfPieceOnBoardForOnePlayer[PieceType.Queen]);
            }
        }

        public override string ToString()
        {
            return $"Board (size: {BoardConsts.BoardSize}x{BoardConsts.BoardSize})";
        }

    }
}
