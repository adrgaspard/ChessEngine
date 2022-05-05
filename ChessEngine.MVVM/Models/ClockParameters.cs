namespace ChessEngine.MVVM.Models
{
    public struct ClockParameters : IEquatable<ClockParameters>
    {
        public readonly TimeSpan BaseTime;
        public readonly TimeSpan IncrementTime;

        public ClockParameters(TimeSpan baseTime, TimeSpan incrementTime)
        {
            BaseTime = baseTime;
            IncrementTime = incrementTime;
        }

        public override bool Equals(object? obj)
        {
            return obj is ClockParameters parameters && Equals(parameters);
        }

        public bool Equals(ClockParameters other)
        {
            return BaseTime.Equals(other.BaseTime) && IncrementTime.Equals(other.IncrementTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseTime, IncrementTime);
        }

        public override string ToString()
        {
            return $"{BaseTime.TotalMinutes} + {IncrementTime.TotalSeconds}";
        }

        public static bool operator ==(ClockParameters left, ClockParameters right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ClockParameters left, ClockParameters right)
        {
            return !(left == right);
        }
    }
}
