using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class StandardRateCalculator : IRateCalculator
    {
        public string Name => RateNameConstants.StandardRateName;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod)
        {
            var totalHoursBetween = parkingPeriod.EntryDateTime.TotalHoursBetween(parkingPeriod.ExitDateTime);
            var totalDaysBetween = parkingPeriod.EntryDateTime.TotalDaysBetween(parkingPeriod.ExitDateTime);

            return totalHoursBetween switch
            {
                0 => RateConstants.StandardFirstHourRate,
                1 => RateConstants.StandardSecondHourRate,
                2 => RateConstants.StandardThirdHourRate,
                _ => RateConstants.StandardFlatRate * totalDaysBetween
            };
        }
    }
}