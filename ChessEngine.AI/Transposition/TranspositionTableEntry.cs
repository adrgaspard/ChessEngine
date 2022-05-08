using ChessEngine.Core.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI.Transposition
{
    public struct TranspositionTableEntry
    {
		public static readonly int MemorySize = Marshal.SizeOf<TranspositionTableEntry>();

		public readonly ulong Key;
		public readonly int Value;
		public readonly byte Depth;
		public readonly Movement Movement;
		public readonly NodeType Type;

		public TranspositionTableEntry(ulong key, int value, byte depth, Movement movement, NodeType nodeType)
		{
			Key = key;
			Value = value;
			Depth = depth;
			Type = nodeType;
			Movement = movement;
		}
	}
}
