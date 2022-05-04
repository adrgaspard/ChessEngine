using ChessEngine.Core.Castling;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.Contracts;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;

namespace ChessEngine.Core.Serialization.FEN
{
    public class FENGameLoader : IGameLoader<string>
    {
        protected readonly IGameHashing<ulong> GameHashing;

        public FENGameLoader(IGameHashing<ulong> gameHashing)
        {
            GameHashing = gameHashing;
        }

        public Game Load(string data)
        {
            string[] informations = data.Split(' ');
            if (informations?.Length != 6)
            {
                throw new ArgumentException($"The data '{data}' is incorrect.");
            }
            Board board = GenerateBoard(informations[0]);
            Colour currentPlayer = GenerateCurrentPlayer(informations[1]);
            CastlingState castlingLegality = GenerateCastlingLegality(informations[2]);
            Position enPassantTarget = GenerateEnPassantTarget(informations[3]);
            byte fiftyMoveCounter = (byte)GenerateFiftyMoveCounter(informations[4]);
            uint totalMoveCounter = GenerateTotalMoveCounter(informations[5]);
            Stack<GameState> stack = new();
            GameState state = new(castlingLegality, enPassantTarget, PieceConsts.NoPiece, fiftyMoveCounter);
            stack.Push(state);
            return new(board, stack, currentPlayer, totalMoveCounter, GameHashing.GenerateHashValue(board, state, currentPlayer));
        }

        protected static Board GenerateBoard(string configPart)
        {
            Board board = new();
            sbyte file = 0, rank = BoardConsts.BoardSize - 1;
            try
            {
                foreach (char symbol in configPart)
                {
                    if (symbol == '/')
                    {
                        file = 0;
                        rank--;
                    }
                    else
                    {
                        if (char.IsDigit(symbol))
                        {
                            file += (sbyte)char.GetNumericValue(symbol);
                        }
                        else
                        {
                            board[new Position(rank, file)] = new(FENConsts.SymbolsToPieceType[symbol.ToString().ToLower()[0]], char.IsUpper(symbol) ? Colour.White : Colour.Black);
                            file++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"The configuration part '{configPart}' is incorrect.", exception);
            }
            return board;
        }

        protected static Colour GenerateCurrentPlayer(string configPart)
        {
            return configPart == "w" ? Colour.White : (configPart == "b" ? Colour.Black : throw new ArgumentException($"The configuration part '{configPart}' is incorrect."));
        }

        protected static CastlingState GenerateCastlingLegality(string configPart)
        {
            CastlingState result = CastlingState.None;
            if (configPart != "-")
            {
                foreach (char symbol in configPart)
                {
                    result |= symbol switch
                    {
                        'K' => CastlingState.WhiteCanCastleOnKingSide,
                        'k' => CastlingState.BlackCanCastleOnKingSide,
                        'Q' => CastlingState.WhiteCanCastleOnQueenSide,
                        'q' => CastlingState.BlackCanCastleOnQueenSide,
                        _ => throw new ArgumentException($"The configuration part '{configPart}' is incorrect."),
                    };
                }
            }
            return result;
        }

        protected static Position GenerateEnPassantTarget(string configPart)
        {
            if (configPart == "-")
            {
                return BoardConsts.NoPosition;
            }
            if (configPart.Length == 2)
            {
                if (configPart[0] >= 'a' && configPart[0] < 'a' + BoardConsts.BoardSize)
                {
                    if (sbyte.TryParse(configPart[1].ToString(), out sbyte result))
                    {
                        if (result >= 1 && result <= BoardConsts.BoardSize)
                        {
                            return new((sbyte)(result - 1), (sbyte)(configPart[0] - 'a'));
                        }
                    }
                }
            }
            throw new ArgumentException($"The configuration part '{configPart}' is incorrect.");
        }

        protected static uint GenerateFiftyMoveCounter(string configPart)
        {
            return ParseStringToUintOrThrow(configPart);
        }

        protected static uint GenerateTotalMoveCounter(string configPart)
        {
            return ParseStringToUintOrThrow(configPart);
        }

        protected static uint ParseStringToUintOrThrow(string str)
        {
            if (uint.TryParse(str, out uint result))
            {
                return result;
            }
            throw new ArgumentException($"The configuration part '{str}' is incorrect.");
        }
    }
}
