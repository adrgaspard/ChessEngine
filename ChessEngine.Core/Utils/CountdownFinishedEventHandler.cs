namespace ChessEngine.Core.Utils
{
    public delegate void CountdownFinishedEventHandler(object? sender, CountdownFinishedEventArgs eventArgs);

    public class CountdownFinishedEventArgs : EventArgs
    {
        public CountdownFinishedEventArgs()
        {
        }
    }
}
