namespace ChessEngine.Core.Match
{
    public struct EndGameData
    {
        public readonly EndGameType Type;
        public readonly bool IsThreefoldRepetitionRuleActivated;
        public readonly bool IsFiftyMoveRuleActivated;

        public EndGameData(EndGameType type, bool isThreefoldRepetitionRuleActivated, bool isFiftyMoveRuleActivated)
        {
            Type = type;
            IsThreefoldRepetitionRuleActivated = isThreefoldRepetitionRuleActivated;
            IsFiftyMoveRuleActivated = isFiftyMoveRuleActivated;
        }
    }
}
