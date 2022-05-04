using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;

namespace ChessEngine.Core.Interactions.Generation.Tools
{
    public static class UlongAndPositionExtensionsForMask
    {
        public static int ConvertToMaskIndex(this Position source)
        {
            return source.Rank * BoardConsts.BoardSize + source.File;
        }

        public static Position ConvertToPosition(this int source)
        {
            return new((sbyte)(source / BoardConsts.BoardSize), (sbyte)(source % BoardConsts.BoardSize));
        }

        public static bool ContainsPosition(this ulong source, Position position)
        {
            return ((source >> position.ConvertToMaskIndex()) & 1) != 0;
        }
    }
}
