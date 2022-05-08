﻿using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.AI.Transposition;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI.Search
{
    public class TranspositionAlphaBetaResearcher : SortedResearcherBase
    {
		public TranspositionTable TranspositionTable { get; protected init; }

        public TranspositionAlphaBetaResearcher(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementGenerator movementGenerator, IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst, TranspositionTable transpositionTable) : base(evaluator, sorter, attackDataGenerator, quietMovementGenerator, movementGenerator, movementMigrator, captureAnalyst)
        {
			TranspositionTable = transpositionTable;
        }

        public override void LaunchResearch(Game game, int depth, CancellationToken token)
        {
			if (game != TranspositionTable.Game)
            {
				throw new ArgumentException($"{nameof(game)} and {nameof(TranspositionTable)}.{nameof(TranspositionTable.Game)} must be the same.");
            }
			Search(game, game.CurrentPlayer, depth, 0, NegativeInfinity, PositiveInfinity, token);
        }

		protected int Search(Game game, Colour aiSide, int depth, int numPlaysFromRoot, int alpha, int beta, CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				return DrawScore;
			}
			if (numPlaysFromRoot > 0)
			{
				if (game.HashsHistory.ContainsKey(game.Hash))
				{
					return DrawScore;
				}
				alpha = Math.Max(alpha, LoseScore + numPlaysFromRoot);
				beta = Math.Min(beta, WinScore - numPlaysFromRoot);
				if (alpha >= beta)
				{
					return alpha;
				}
			}
			int transpositionTableLookup = TranspositionTable.LookupEvaluation(depth, numPlaysFromRoot, alpha, beta);
			if (transpositionTableLookup != TranspositionTable.NoLookup)
			{
				if (numPlaysFromRoot == 0)
				{
					BestFound = TranspositionTable.GetStoredMovement();
				}
				return transpositionTableLookup;
			}
			if (depth == 0)
			{
				return CaptureAnalyst.SearchCaptures(game, alpha, beta);
			}
			AttackData attackData = AttackDataGenerator.GenerateAttackData(game);
			IList<Movement> movements = Sorter.Sort(game, attackData, MovementGenerator.GenerateMovements(game, attackData));
			if (movements.Count == 0)
			{
				return attackData.IsCheck ? LoseScore + numPlaysFromRoot : DrawScore;
			}
			NodeType evaluationType = NodeType.UpperBound;
			Movement localBestFound = NoMovement;
			foreach (Movement movement in movements)
            {
				MovementMigrator.Up(game, movement);
				int evaluation = -Search(game, aiSide, depth - 1, numPlaysFromRoot + 1, -beta, -alpha, token);
				MovementMigrator.Down(game, movement);
				if (evaluation >= beta)
                {
					TranspositionTable.StoreEvaluation(depth, numPlaysFromRoot, beta, NodeType.LowerBound, movement);
					return beta;
                }
				if (evaluation > alpha)
                {
					evaluationType = NodeType.Exact;
					localBestFound = movement;
					alpha = evaluation;
					if (numPlaysFromRoot == 0)
                    {
						BestFound = movement;
                    }
                }
            }
			TranspositionTable.StoreEvaluation(depth, numPlaysFromRoot, alpha, evaluationType, localBestFound);
			return alpha;

		}
	}
}
