using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class NightRateCalculator() : IRateCalculator
    {
        public string Name => "Night Rate";
        public decimal Rate => 6.5m;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod) => Rate;

        public bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
                parkingPeriod.EntryDateTime.IsWeekDay() &&
                parkingPeriod.EntryDateTime.IsWithinTimeRange(new TimeSpan(18, 0, 0), TimeSpan.FromHours(24)) &&
                parkingPeriod.ExitDateTime.Before(new TimeSpan(6, 0, 0));
        }
    }
}