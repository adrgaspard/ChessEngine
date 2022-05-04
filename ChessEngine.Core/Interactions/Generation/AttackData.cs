namespace ChessEngine.Core.Interactions.Generation
{
    public struct AttackData
    {
        public readonly ulong AttackMap;
        public readonly ulong AttackMapWithoutPawns;
        public readonly ulong PinMap;
        public readonly ulong CheckMask;
        public readonly bool IsCheck;
        public readonly bool IsDoubleCheck;

        public AttackData(ulong attackMap, ulong attackMapWithoutPawns, ulong pinMap, ulong checkMask, bool isCheck, bool isDoubleCheck)
        {
            AttackMap = attackMap;
            AttackMapWithoutPawns = attackMapWithoutPawns;
            PinMap = pinMap;
            CheckMask = checkMask;
            IsCheck = isCheck;
            IsDoubleCheck = isDoubleCheck;
        }
    }
}
