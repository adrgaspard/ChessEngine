using ChessEngine.Core.Environment.Tools;

namespace ChessEngine.Core.Environment
{
    public class PieceGroup
    {
        protected readonly Position[] OccupiedPositions;
        protected readonly Dictionary<Position, byte> OccupedPositionsIndexes;

        public byte CurrentSize { get; protected set; }

        public Position this[byte index] => OccupiedPositions[index];

        public PieceGroup(byte maxPieces)
        {
            OccupiedPositions = new Position[maxPieces + 1];
            OccupedPositionsIndexes = new(BoardConsts.BoardSize * BoardConsts.BoardSize);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    OccupedPositionsIndexes.Add(new(i, j), byte.MaxValue);
                }
            }
            CurrentSize = 0;
        }

        internal void AddPieceAtPosition(Position position)
        {
            OccupiedPositions[CurrentSize] = position;
            OccupedPositionsIndexes[position] = CurrentSize;
            CurrentSize++;
        }

        internal void RemovePieceFromPosition(Position position)
        {
            byte pieceIndex = OccupedPositionsIndexes[position];
            OccupiedPositions[pieceIndex] = OccupiedPositions[CurrentSize - 1];
            OccupedPositionsIndexes[OccupiedPositions[pieceIndex]] = pieceIndex;
            CurrentSize--;
        }

    }
}
