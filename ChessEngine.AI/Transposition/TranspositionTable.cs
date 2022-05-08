using ChessEngine.AI.Abstractions;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI.Transposition
{
    public class TranspositionTable
    {
        public const int NoLookup = ResearcherBase.NegativeInfinity - 1000000;

        public Game Game { get; protected init; }

        protected TranspositionTableEntry[] Entries { get; init; }

        protected ulong Size { get; init; }

        public ulong Index => Game.Hash % Size;

        protected bool IsEnabled { get; set; }

        public TranspositionTable(Game game, ulong size)
        {
            Game = game;
            Entries = new TranspositionTableEntry[size];
            Size = size;
            IsEnabled = true;
        }

        public Movement GetStoredMovement()
        {
            return Entries[Index].Movement;
        }

        public void Clear()
        {
            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = new();
            }
        }

        public int LookupEvaluation(int depth, int numPlaysFromRoot, int alpha, int beta)
        {
            if (!IsEnabled)
            {
                return NoLookup;
            }
            TranspositionTableEntry entry = Entries[Index];
            if (entry.Key == Game.Hash && entry.Depth >= depth)
            {
                int correctedScore = GetCorrectRetrievedMateScore(entry.Value, numPlaysFromRoot);
                return entry.Type switch
                {
                    NodeType.Exact => correctedScore,
                    NodeType.LowerBound => correctedScore <= alpha ? correctedScore : NoLookup,
                    NodeType.UpperBound => correctedScore >= beta ? correctedScore : NoLookup,
                    _ => NoLookup,
                };
            }
            return NoLookup;
        }

        public void StoreEvaluation(int depth, int numPlaysFromRoot, int evaluation, NodeType type, Movement movement)
        {
            if (!IsEnabled)
            {
                return;
            }
            TranspositionTableEntry entry = new(Game.Hash, GetCorrectMateScoreForStorage(evaluation, numPlaysFromRoot), (byte)depth, movement, type);
            Entries[Index] = entry;
        }

        protected int GetCorrectMateScoreForStorage(int score, int numPlySearched)
        {
            if (ResearcherBase.IsMateScore(score))
            {
                int sign = Math.Sign(score);
                return (score * sign + numPlySearched) * sign;
            }
            return score;
        }

        protected int GetCorrectRetrievedMateScore(int score, int numPlySearched)
        {
            if (ResearcherBase.IsMateScore(score))
            {
                int sign = Math.Sign(score);
                return (score * sign - numPlySearched) * sign;
            }
            return score;
        }
    }
}
