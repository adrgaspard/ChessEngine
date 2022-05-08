using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI.Transposition
{
    public enum NodeType : byte
    {
        None = 0,
        Exact = 1,
        LowerBound = 2,
        UpperBound = 3
    }
}
