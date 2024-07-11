using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates
{
    public interface ICalculationEngine
    {
        Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod);
    }

    public class CalculationEngine : ICalculationEngine
    {
        private readonly IRateCalculationStrategy _rateCalculationStrategy;

        public CalculationEngine(IRateCalculationStrategy rateCalculationStrategy)
        {
            _rateCalculationStrategy = rateCalculationStrategy;
        }

        public Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod)
        {
            var rateCalculator = _rateCalculationStrategy.GetCalculator(parkingPeriod);
            var totalPrice = rateCalculator.GetTotalPrice(parkingPeriod);

            var result = new RateResult
            {
                RateName = rateCalculator.Name,
                TotalPrice = totalPrice,
            };

            return new Result<IRateResult>(result);
        }
    }
}