namespace Emprevo.Api.Models
{
    public class ParkingPeriod
    {
        public DateTime EntryDateTime { get; }
        public DateTime ExitDateTime { get; }

        public ParkingPeriod(DateTime entryDateTime, DateTime exitDateTime)
        {
            if (entryDateTime >= exitDateTime)
            {
                throw new ArgumentException("Entry date and time must be before exit date and time");
            }

            EntryDateTime = entryDateTime;
            ExitDateTime = exitDateTime;
        }
    }
}