﻿using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.BranchAndBound.Abstractions
{
    public abstract class BranchAndBoundChessAI : IChessAI
    {
        public abstract Movement SelectMovement(Game game, IList<Movement> legalMovements);
    }
}