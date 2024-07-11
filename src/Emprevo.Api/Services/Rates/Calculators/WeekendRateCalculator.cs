using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class WeekendRateCalculator() : BaseRateCalculator
    {
        public override string Name => RateNameConstants.WeekendRateName;

        public override decimal Rate => RateConstants.WeekendRate;

        public override decimal GetTotalPrice(ParkingPeriod parkingPeriod)
        {
            var totalDaysBetween = parkingPeriod.EntryDateTime.TotalDaysBetween(parkingPeriod.ExitDateTime);

            return Rate * totalDaysBetween;
        }

        public override bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsWeekend() && parkingPeriod.ExitDateTime.IsWeekend();
        }
    }
}