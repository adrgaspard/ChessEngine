using ChessEngine.MVVM.Models;
using System.Collections.ObjectModel;

namespace ChessEngine.MVVM.Utils
{
    public static class ClockParametersConsts
    {
        public static readonly ClockParameters InfiniteTime = new(TimeSpan.MaxValue, TimeSpan.MaxValue);
        public static readonly ClockParameters Bullet1Plus0 = new(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(0));
        public static readonly ClockParameters Bullet2Plus1 = new(TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(1));
        public static readonly ClockParameters Blitz3Plus0 = new(TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(0));
        public static readonly ClockParameters Blitz3Plus2 = new(TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(2));
        public static readonly ClockParameters Blitz5Plus0 = new(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(0));
        public static readonly ClockParameters Blitz5Plus3 = new(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(3));
        public static readonly ClockParameters Rapid10Plus0 = new(TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(0));
        public static readonly ClockParameters Rapid10Plus5 = new(TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(5));
        public static readonly ClockParameters Rapid15Plus10 = new(TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(10));
        public static readonly ClockParameters Classical30Plus0 = new(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(0));
        public static readonly ClockParameters Classical30Plus20 = new(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(20));


        public static readonly IReadOnlyList<ClockParameters> PredefinedClockParameters = new ReadOnlyCollection<ClockParameters>(new List<ClockParameters>()
        {
            Bullet1Plus0,
            Bullet2Plus1,
            Blitz3Plus0,
            Blitz3Plus2,
            Blitz5Plus0,
            Blitz5Plus3,
            Rapid10Plus0,
            Rapid10Plus5,
            Rapid15Plus10,
            Classical30Plus0,
            Classical30Plus20,
            InfiniteTime
        });
    }
}
