using ChessEngine.Core.Match;

namespace ChessEngine.Core.Serialization.Contracts
{
    public interface IGameLoader<TSerialized>
    {
        Game Load(TSerialized data);
    }
}
