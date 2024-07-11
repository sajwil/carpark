using Emprevo.Api.Constants;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class NightRateCalculator : IRateCalculator
    {
        public string Name => RateNameConstants.NightRateName;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod) => RateConstants.NightRate;
    }
}