namespace Emprevo.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekDay(this DateTime date)
        {
            var weekends = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return !weekends.Contains(date.DayOfWeek);
        }

        public static bool IsWeekend(this DateTime date)
        {
            var weekends = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weekends.Contains(date.DayOfWeek);
        }

        public static bool IsWithinTimeRange(this DateTime dateTime, TimeSpan start, TimeSpan end)
        {
            return dateTime.TimeOfDay >= start && dateTime.TimeOfDay <= end;
        }

        public static bool Before(this DateTime dateTime, TimeSpan time)
        {
            return dateTime.TimeOfDay <= time;
        }

        public static int TotalHoursBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (endDateTime - startDateTime).Hours;
        }

        public static int TotalDaysBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (endDateTime - startDateTime).Days;
        }

        public static bool IsOneDayGapBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (endDateTime - startDateTime).Days <= 1;
        }
    }
}