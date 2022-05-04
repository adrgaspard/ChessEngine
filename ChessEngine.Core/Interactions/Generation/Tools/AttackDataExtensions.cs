using ChessEngine.Core.Environment;

namespace ChessEngine.Core.Interactions.Generation.Tools
{
    public static class AttackDataExtensions
    {
        public static bool IsPositionPinned(this AttackData source, Position position)
        {
            return source.PinMap.ContainsPosition(position);
        }

        public static bool IsPositionAttacked(this AttackData source, Position position)
        {
            return source.AttackMap.ContainsPosition(position);
        }

        public static bool IsPositionInCheckRange(this AttackData source, Position position)
        {
            return source.IsCheck && ((source.CheckMask >> position.ConvertToMaskIndex()) & 1) != 0;
        }
    }
}
