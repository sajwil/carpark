using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates
{
    public interface IRateCalculationStrategy
    {
        IRateCalculator GetCalculator(ParkingPeriod parkingPeriod);
    }

    public class RateCalculationStrategy(IServiceProvider serviceProvider) : IRateCalculationStrategy
    {
        private readonly IEnumerable<IRateCalculator> _rateCalculators = serviceProvider.GetServices<IRateCalculator>();

        public IRateCalculator GetCalculator(ParkingPeriod parkingPeriod)
        {
            var rateCalculator = _rateCalculators
                .First(rateCalculator => rateCalculator.IsElligible(parkingPeriod));

            return rateCalculator;
        }
    }
}