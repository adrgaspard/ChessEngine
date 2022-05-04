using ChessEngine.Core.Castling;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Match;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Utils;
using System.Collections.ObjectModel;

namespace ChessEngine.Core.Transposition.Zobrist
{
    public class ZobristHashing : IGameHashing<ulong>
    {
        protected const int Seed = 20315509;
        protected static readonly Random Random = new(Seed);

        protected static readonly IReadOnlyDictionary<Position, IReadOnlyDictionary<Piece, ulong>> BoardHashs;
        protected static readonly IReadOnlyDictionary<CastlingState, ulong> CastlingHashs;
        protected static readonly IReadOnlyDictionary<sbyte, ulong> EnPassantFileHashs;
        protected static readonly ulong CurrentPlayerHash;

        static ZobristHashing()
        {
            List<PieceType> pieceTypes = new(6) { PieceType.King, PieceType.Pawn, PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Queen };
            List<Colour> pieceColours = new(2) { Colour.White, Colour.Black };
            Dictionary<Position, IReadOnlyDictionary<Piece, ulong>> boardHashs = new();
            Dictionary<CastlingState, ulong> castlingHashs = new();
            Dictionary<sbyte, ulong> enPassantFileHashs = new();
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Dictionary<Piece, ulong> subDictionary = new();
                    foreach (PieceType pieceType in pieceTypes)
                    {
                        foreach (Colour pieceColour in pieceColours)
                        {
                            subDictionary.Add(new(pieceType, pieceColour), Random.NextUnsignedInt64());
                        }
                    }
                    boardHashs.Add(new(i, j), new ReadOnlyDictionary<Piece, ulong>(subDictionary));
                }
            }
            for (byte i = 0; i < 16; i++)
            {
                castlingHashs.Add((CastlingState)i, Random.NextUnsignedInt64());
            }
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                enPassantFileHashs.Add(i, Random.NextUnsignedInt64());
            }
            CurrentPlayerHash = Random.NextUnsignedInt64();
            BoardHashs = new ReadOnlyDictionary<Position, IReadOnlyDictionary<Piece, ulong>>(boardHashs);
            CastlingHashs = new ReadOnlyDictionary<CastlingState, ulong>(castlingHashs);
            EnPassantFileHashs = new ReadOnlyDictionary<sbyte, ulong>(enPassantFileHashs);
        }

        public ulong GenerateHashValue(Board board, GameState state, Colour currentPlayer)
        {
            ulong zobristValue = 0;
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    Position position = new(i, j);
                    Piece piece = board[position];
                    if (piece != PieceConsts.NoPiece)
                    {
                        zobristValue ^= BoardHashs[position][piece];
                    }
                }
            }
            if (state.EnPassantTarget != BoardConsts.NoPosition)
            {
                zobristValue ^= EnPassantFileHashs[state.EnPassantTarget.File];
            }
            if (currentPlayer == Colour.Black)
            {
                zobristValue ^= CurrentPlayerHash;
            }
            zobristValue ^= CastlingHashs[state.CastlingState];
            return zobristValue;
        }

        public ulong IncrementHashOnCurrentPlayerUpdate(ulong hash)
        {
            return hash ^ CurrentPlayerHash;
        }

        public ulong IncrementHashOnBoardUpdate(ulong hash, Position oldOrNewposition, Piece oldOrNewPiece)
        {
            return hash ^ BoardHashs[oldOrNewposition][oldOrNewPiece];
        }

        public ulong IncrementHashOnPawnPushUpdate(ulong hash, Position oldOrNewEnPassantTarget)
        {
            return hash ^ EnPassantFileHashs[oldOrNewEnPassantTarget.File];
        }

        public ulong IncrementHashOnCastlingStateUpdate(ulong hash, CastlingState oldOrNewCastlingState)
        {
            return hash ^ CastlingHashs[oldOrNewCastlingState];
        }
    }
}
