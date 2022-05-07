using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using System.Collections.ObjectModel;

namespace ChessEngine.AI.Evaluation.Caching
{
    public static class EvaluationCache
    {
        public static readonly IReadOnlyDictionary<Position, byte> CenterManhattanDistance;
        public static readonly IReadOnlyDictionary<(Position, Position), byte> OrthogonalDistance;
        public static readonly IReadOnlyDictionary<(Position, Position), byte> KingDistance;

        static EvaluationCache()
        {
            Dictionary<Position, byte> centerManhattanDistance = new(BoardConsts.NumberOfPositions);
            Dictionary<(Position, Position), byte> orthogonalDistance = new(BoardConsts.NumberOfPositions * BoardConsts.NumberOfPositions);
            Dictionary<(Position, Position), byte> kingDistance = new(BoardConsts.NumberOfPositions * BoardConsts.NumberOfPositions);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Position position = new(i, j);
                    byte fileDistanceFromCenter = (byte)Math.Max(BoardConsts.BoardSize / 2 - 1 - position.File, position.File - (BoardConsts.BoardSize / 2));
                    byte rankDistanceFromCenter = (byte)Math.Max(BoardConsts.BoardSize / 2 - 1 - position.Rank, position.Rank - (BoardConsts.BoardSize / 2));
                    centerManhattanDistance.Add(position, (byte)(fileDistanceFromCenter + rankDistanceFromCenter));
                    for (sbyte k = 0; k < BoardConsts.BoardSize; k++)
                    {
                        for (sbyte l = 0; l < BoardConsts.BoardSize; l++)
                        {
                            Position otherPosition = new(k, l);
                            byte rankDistance = (byte)Math.Abs(position.Rank - otherPosition.Rank);
                            byte fileDistance = (byte)Math.Abs(position.File - otherPosition.File);
                            orthogonalDistance.Add((position, otherPosition), (byte)(fileDistance + rankDistance));
                            kingDistance.Add((position, otherPosition), Math.Max(fileDistance, rankDistance));
                        }
                    }
                }
            }
            CenterManhattanDistance = new ReadOnlyDictionary<Position, byte>(centerManhattanDistance);
            OrthogonalDistance = new ReadOnlyDictionary<(Position, Position), byte>(orthogonalDistance);
            KingDistance = new ReadOnlyDictionary<(Position, Position), byte>(kingDistance);
        }
    }
}
