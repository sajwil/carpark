using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class NightRateCalculator() : BaseRateCalculator
    {
        public override string Name => RateNameConstants.NightRateName;
        public override decimal Rate => 6.5m;

        public override bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
                parkingPeriod.EntryDateTime.IsWeekDay() &&
                parkingPeriod.EntryDateTime.IsWithinTimeRange(TimeConstants.SixPM, TimeConstants.Midnight) &&
                parkingPeriod.ExitDateTime.Before(TimeConstants.SixAm);
        }
    }
}