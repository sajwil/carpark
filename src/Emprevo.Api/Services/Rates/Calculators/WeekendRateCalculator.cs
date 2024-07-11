using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class WeekendRateCalculator : IRateCalculator
    {
        public string Name => RateNameConstants.WeekendRateName;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod)
        {
            var totalDaysBetween = parkingPeriod.EntryDateTime.TotalDaysBetween(parkingPeriod.ExitDateTime);

            return RateConstants.WeekendRate * totalDaysBetween;
        }
    }
}