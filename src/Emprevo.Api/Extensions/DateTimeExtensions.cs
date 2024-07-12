using System.Globalization;

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

        public static bool IsWithinGivenRange(this DateTime entryDateTime, DateTime exitDateTime, DateTime lowerBound, DateTime upperBound)
        {
            return entryDateTime >= lowerBound && exitDateTime <= upperBound;
        }

        public static bool Before(this DateTime dateTime, TimeSpan time)
        {
            return dateTime.TimeOfDay <= time;
        }

        public static int TotalHoursBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (int)endDateTime.Subtract(startDateTime).TotalHours;
        }

        public static int TotalDaysBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (int)Math.Ceiling((endDateTime - startDateTime).TotalHours / 24);
        }

        public static bool IsOneDayGapBetween(this DateTime startDateTime, DateTime endDateTime)
        {
            return (endDateTime - startDateTime).TotalDays <= 1;
        }

        public static DateTime ToDate(this string dateString)
        {
            _ = DateTime.TryParse(dateString, new CultureInfo(CultureInfo.CurrentCulture.Name), out DateTime dateValue);
            return dateValue;
        }

        public static decimal GetFullHoursBetweenDates(this DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan timeSpan = endDateTime - startDateTime;
            var temp = Math.Ceiling((decimal)timeSpan.TotalMinutes) / 60;

            return temp;
        }
    }
}