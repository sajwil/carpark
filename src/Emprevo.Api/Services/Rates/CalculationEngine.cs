using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates.Calculators;

namespace Emprevo.Api.Services.Rates
{
    public interface ICalculationEngine
    {
        Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod);
    }

    public class CalculationEngine : ICalculationEngine
    {
        private readonly IEnumerable<IRateCalculator> _rateCalculators;

        public CalculationEngine()
        {
            _rateCalculators =
            [
                new EarlybirdRateCalculator(),
                new NightRateCalculator(),
                new WeekendtRateCalculator(),
                new StandardRateCalculator(),
            ];
        }

        public Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod)
        {
            var rateCalculator = _rateCalculators.First(rateCalculator => rateCalculator.IsElligible(parkingPeriod));
            var totalPrice = rateCalculator.GetTotalPrice(parkingPeriod);

            return new Result<IRateResult>(new RateResult
            {
                RateName = rateCalculator.Name,
                TotalPrice = totalPrice,
            });
        }
    }
}