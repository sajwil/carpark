using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class StandardRateCalculator() : BaseRateCalculator
    {
        public override string Name => RateNameConstants.StandardRateName;

        public override decimal GetTotalPrice(ParkingPeriod parkingPeriod)
        {
            var totalHoursBetween = parkingPeriod.EntryDateTime.TotalHoursBetween(parkingPeriod.ExitDateTime);
            var totalDaysBetween = parkingPeriod.EntryDateTime.TotalDaysBetween(parkingPeriod.ExitDateTime);

            return totalHoursBetween switch
            {
                0 => 5,
                1 => 10,
                2 => 15,
                _ => 20 * totalDaysBetween
            };
        }
    }
}