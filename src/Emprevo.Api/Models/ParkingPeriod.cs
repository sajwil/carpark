namespace Emprevo.Api.Models
{
    public class ParkingPeriod
    {
        public DateTime EntryDateTime { get; }
        public DateTime ExitDateTime { get; }

        public ParkingPeriod(string entryDate, string exitDate, string entryTime, string exitTime)
        {
            // validation here

            var entryDateTime = Convert.ToDateTime(entryDate).Add(TimeSpan.Parse(entryTime));
            var exitDateTime = Convert.ToDateTime(exitDate).Add(TimeSpan.Parse(exitTime));

            EntryDateTime = entryDateTime;
            ExitDateTime = exitDateTime;
        }
    }
}