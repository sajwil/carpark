using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    internal class StandardRateCalculator() : IRateCalculator
    {
        public string Name => "Standard Rate";

        public decimal Rate => 6.5m;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod)
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

        public bool IsElligible(ParkingPeriod parkingPeriod)
        {
            throw new NotImplementedException();
        }
    }
}