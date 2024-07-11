using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class EarlybirdRateCalculator() : BaseRateCalculator
    {
        public override string Name => RateNameConstants.EarlybirdRateName;

        public override decimal Rate => RateConstants.EarlybirdRate;

        public override bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
                parkingPeriod.EntryDateTime.IsWeekDay() &&
                parkingPeriod.EntryDateTime.IsWithinTimeRange(TimeConstants.SixAm, TimeConstants.NineAm) &&
                parkingPeriod.ExitDateTime.IsWithinTimeRange(TimeConstants.ThreeThirtyPm, TimeConstants.ElevnThirtyPm);
        }
    }
}