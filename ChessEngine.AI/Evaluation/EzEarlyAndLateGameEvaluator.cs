using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Match;
using static ChessEngine.AI.Evaluation.Caching.EvaluationCache;
using static ChessEngine.AI.Evaluation.Caching.PieceTableCache;

namespace ChessEngine.AI.Evaluation
{
    public class EzEarlyAndLateGameEvaluator : EvaluatorBase
    {
        public float EndGameMaterialStart { get; protected init; }

        public float EndGameMultiplier { get; protected init; }

        public int ManhattanDistanceMultiplier { get; protected init; }

        public int OrthogonalDistanceBaseValue { get; protected init; }

        public int OrthogonalDistanceMultiplier { get; protected init; }

        public EzEarlyAndLateGameEvaluator(IPieceEvaluator pieceEvaluator, int manhattanDistanceMultiplier, int orthogonalDistanceBaseValue, int orthogonalDistanceMultiplier) : base(pieceEvaluator)
        {
            EndGameMaterialStart = 2 * PieceEvaluator.GetValue(PieceType.Rook) + PieceEvaluator.GetValue(PieceType.Bishop) + PieceEvaluator.GetValue(PieceType.Knight);
            EndGameMultiplier = 1 / EndGameMaterialStart;
            ManhattanDistanceMultiplier = manhattanDistanceMultiplier;
            OrthogonalDistanceBaseValue = orthogonalDistanceBaseValue;
            OrthogonalDistanceMultiplier = orthogonalDistanceMultiplier;
        }

        public override int Evaluate(Game game, Colour aiSide)
        {
            int whiteMaterial = CountMaterial(game.Board, Colour.White);
            int blackMaterial = CountMaterial(game.Board, Colour.Black);
            int whiteMaterialWithoutPawns = whiteMaterial - game.Board[PieceType.Pawn, Colour.White].CurrentSize * PieceEvaluator.GetValue(PieceType.Pawn);
            int blackMaterialWithoutPawns = blackMaterial - game.Board[PieceType.Pawn, Colour.Black].CurrentSize * PieceEvaluator.GetValue(PieceType.Pawn);
            float whiteEndgamePhaseWeight = GetEndGamePhaseWeight(whiteMaterialWithoutPawns);
            float blackEndgamePhaseWeight = GetEndGamePhaseWeight(blackMaterialWithoutPawns);
            int whiteEvaluation = whiteMaterial;
            int blackEvaluation = blackMaterial;
            whiteEvaluation += EvaluateMopUp(game, Colour.White, Colour.Black, whiteMaterial, blackMaterial, blackEndgamePhaseWeight);
            blackEvaluation += EvaluateMopUp(game, Colour.Black, Colour.White, blackMaterial, whiteMaterial, whiteEndgamePhaseWeight);
            whiteEvaluation += EvaluatePieceSquareTables(game, Colour.White, blackEndgamePhaseWeight);
            blackEvaluation += EvaluatePieceSquareTables(game, Colour.Black, whiteEndgamePhaseWeight);
            int evaluation = whiteEvaluation - blackEvaluation;
            return evaluation * (game.CurrentPlayer == Colour.White ? 1 : -1);
        }

        protected float GetEndGamePhaseWeight(int materialCountWithoutPawns)
        {
            return 1 - Math.Min(1, materialCountWithoutPawns * EndGameMultiplier);
        }

        protected int EvaluateMopUp(Game game, Colour currentPlayer, Colour opponentPlayer, int currentPlayerMaterial, int opponentPlayerMaterial, float endGameWeight)
        {
            if (currentPlayerMaterial > opponentPlayerMaterial + PieceEvaluator.GetValue(PieceType.Pawn) * 2 && endGameWeight > 0)
            {
                int mopUpScore = 0;
                Position currentPlayerKingPosition = game.Board[currentPlayer];
                Position opponentPlayerKingPosition = game.Board[opponentPlayer];
                mopUpScore += CenterManhattanDistance[opponentPlayerKingPosition] * ManhattanDistanceMultiplier;
                mopUpScore += (OrthogonalDistanceBaseValue - OrthogonalDistance[(currentPlayerKingPosition, opponentPlayerKingPosition)]) * OrthogonalDistanceMultiplier;
                return (int)(mopUpScore * endGameWeight);
            }
            return 0;
        }

        protected int EvaluatePieceSquareTables(Game game, Colour colour, float endgamePhaseWeight)
        {
            int value = 0;
            value += EvaluatePieceSquareTable(game, colour, PieceType.Pawn);
            value += EvaluatePieceSquareTable(game, colour, PieceType.Knight);
            value += EvaluatePieceSquareTable(game, colour, PieceType.Bishop);
            value += EvaluatePieceSquareTable(game, colour, PieceType.Rook);
            value += EvaluatePieceSquareTable(game, colour, PieceType.Queen);
            int kingScoreForEarlyGame = GetKingPieceTable(game.Board[colour], colour, false);
            value += (int)(kingScoreForEarlyGame * (1 - endgamePhaseWeight));
            return value;
        }

        protected static int EvaluatePieceSquareTable(Game game, Colour colour, PieceType pieceType)
        {
            int result = 0;
            PieceGroup pieceGroup = game.Board[pieceType, colour];
            for (byte i = 0; i < pieceGroup.CurrentSize; i++)
            {
                result += GetClassicPieceTable(pieceGroup[i], game.Board[pieceGroup[i]]);
            }
            return result;
        }

    }
}
