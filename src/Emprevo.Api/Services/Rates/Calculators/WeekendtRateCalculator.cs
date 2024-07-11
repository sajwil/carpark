using Emprevo.Api.Extensions;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class WeekendtRateCalculator() : IRateCalculator
    {
        public string Name => "Weekend Rate";

        public decimal Rate => 10;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod) => Rate;

        public bool IsElligible(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsWeekend() && parkingPeriod.ExitDateTime.IsWeekend();
        }
    }
}