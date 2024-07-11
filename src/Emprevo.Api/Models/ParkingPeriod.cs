using System.Globalization;

namespace Emprevo.Api.Models
{
    public class ParkingPeriod
    {
        public DateTime EntryDateTime { get; }
        public DateTime ExitDateTime { get; }

        public ParkingPeriod(string entryDate, string exitDate, string entryTime, string exitTime)
        {
            if (!DateTime.TryParseExact(entryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Invalid entry date format");
            }

            if (!DateTime.TryParseExact(exitDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Invalid exit date format");
            }

            if (!DateTime.TryParseExact(entryTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Invalid entry time format");
            }

            if (!DateTime.TryParseExact(exitTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Invalid exit time format");
            }

            var entryDateTime = Convert.ToDateTime(entryDate).Add(TimeSpan.Parse(entryTime, CultureInfo.InvariantCulture));
            var exitDateTime = Convert.ToDateTime(exitDate).Add(TimeSpan.Parse(exitTime, CultureInfo.InvariantCulture));

            if (entryDateTime >= exitDateTime)
            {
                throw new ArgumentException("Entry date and time must be before exit date and time");
            }


            EntryDateTime = entryDateTime;
            ExitDateTime = exitDateTime;
        }
    }
}