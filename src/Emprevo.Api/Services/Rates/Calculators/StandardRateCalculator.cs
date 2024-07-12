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
            DateTime currentStart = parkingPeriod.EntryDateTime;

            var totalPrice = 0m;
            var totalHours = parkingPeriod.EntryDateTime.GetFullHoursBetweenDates(parkingPeriod.ExitDateTime);

            while (totalHours > 0)
            {
                if (totalHours <= 24)
                {
                    totalPrice += GetHourlyRateBasedOnRemainingHours(totalHours);
                    break;
                }

                DateTime segmentEnd = GetEndTimeWithinParkingPeriod(parkingPeriod, currentStart);
                var remainingHours = currentStart.GetFullHoursBetweenDates(segmentEnd);
                totalPrice += GetHourlyRateBasedOnRemainingHours(remainingHours);
                totalHours -= remainingHours;
                currentStart = segmentEnd;
            }

            return totalPrice;
        }

        /// <summary>
        /// Gets the end time of a segment within the parking period, ensuring it does not exceed 24 hours or the exit time.
        /// </summary>
        /// <param name="parkingPeriod">The parking period to consider.</param>
        /// <param name="currentStart">The current start time within the parking period.</param>
        /// <returns>The end time of the segment.</returns>
        private static DateTime GetEndTimeWithinParkingPeriod(ParkingPeriod parkingPeriod, DateTime currentStart)
        {
            return parkingPeriod.EntryDateTime.AddHours(24) <= parkingPeriod.ExitDateTime
                ? currentStart.AddHours(24)
                : parkingPeriod.ExitDateTime;
        }

        private static decimal GetHourlyRateBasedOnRemainingHours(decimal remainingHours)
        {
            if (remainingHours >= 0 && remainingHours < 1) return RateConstants.StandardFirstHourRate;
            if (remainingHours >= 1 && remainingHours < 2) return RateConstants.StandardSecondHourRate;
            if (remainingHours >= 2 && remainingHours < 3) return RateConstants.StandardThirdHourRate;
            return RateConstants.StandardFlatRate;
        }
    }
}