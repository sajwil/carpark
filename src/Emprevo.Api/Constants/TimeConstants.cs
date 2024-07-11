namespace Emprevo.Api.Constants
{
    public static class TimeConstants
    {
        public static TimeSpan SixAm => new(6, 0, 0);
        public static TimeSpan NineAm => new(9, 0, 0);
        public static TimeSpan ThreeThirtyPm => new(15, 30, 0);
        public static TimeSpan SixPM => new(18, 0, 0);
        public static TimeSpan ElevnThirtyPm => new(23, 30, 0);
        public static TimeSpan Midnight => TimeSpan.FromHours(24);
    }
}
