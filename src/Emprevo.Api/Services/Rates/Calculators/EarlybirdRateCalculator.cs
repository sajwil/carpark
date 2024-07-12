using Emprevo.Api.Constants;
using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public class EarlybirdRateCalculator : IRateCalculator
    {
        public string Name => RateNameConstants.EarlybirdRateName;

        public decimal GetTotalPrice(ParkingPeriod parkingPeriod) => RateConstants.EarlybirdRate;
    }
}