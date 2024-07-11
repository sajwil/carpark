using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class EarlybirdRateCalculator() : IRateCalculator
    {
        public string Name => "Early Bird";

        public decimal Rate => 13;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod) => Rate;

        public bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
                parkingPeriod.EntryDateTime.IsWeekDay() &&
                parkingPeriod.EntryDateTime.IsWithinTimeRange(new TimeSpan(6, 0, 0), new TimeSpan(9, 0, 0)) &&
                parkingPeriod.ExitDateTime.IsWithinTimeRange(new TimeSpan(15, 30, 0), new TimeSpan(23, 30, 0));
        }
    }
}