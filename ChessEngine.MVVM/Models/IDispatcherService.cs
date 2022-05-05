namespace ChessEngine.MVVM.Models
{
    public interface IDispatcherService
    {
        void InvokeAsync(Action action);
    }
}
