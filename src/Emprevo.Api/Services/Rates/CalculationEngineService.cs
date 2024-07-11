using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates.Calculators;

namespace Emprevo.Api.Services.Rates
{
    public interface ICalculationEngineService
    {
        Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod);
    }

    public class CalculationEngineService(IServiceProvider serviceProvider) : ICalculationEngineService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Result<IRateResult> CalculateRate(ParkingPeriod parkingPeriod)
        {
            var rateCalculator = GetRateCalculator(parkingPeriod);
            var totalPrice = rateCalculator.GetTotalPrice(parkingPeriod);

            return new Result<IRateResult>(new RateResult
            {
                RateName = rateCalculator.Name,
                TotalPrice = totalPrice,
            });
        }

        private IRateCalculator GetRateCalculator(ParkingPeriod parkingPeriod)
        {
            var rateCalculatorType = parkingPeriod switch
            {
                _ when IsEarlyBirdCalculation(parkingPeriod) => typeof(EarlybirdRateCalculator),
                _ when IsNightRateCalculation(parkingPeriod) => typeof(NightRateCalculator),
                _ when IsWeekendRateCalculation(parkingPeriod) => typeof(WeekendRateCalculator),
                _ => typeof(StandardRateCalculator)
            };

            return _serviceProvider.GetService(rateCalculatorType) is not IRateCalculator rateCalculator
                ? throw new InvalidOperationException("The requested rate calculator could not be found.")
                : rateCalculator;
        }

        private static bool IsEarlyBirdCalculation(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
               parkingPeriod.EntryDateTime.IsWeekDay() &&
               parkingPeriod.EntryDateTime.IsWithinTimeRange(TimeConstants.SixAm, TimeConstants.NineAm) &&
               parkingPeriod.ExitDateTime.IsWithinTimeRange(TimeConstants.ThreeThirtyPm, TimeConstants.ElevnThirtyPm);
        }

        private static bool IsNightRateCalculation(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsOneDayGapBetween(parkingPeriod.ExitDateTime) &&
               parkingPeriod.EntryDateTime.IsWeekDay() &&
               parkingPeriod.EntryDateTime.IsWithinTimeRange(TimeConstants.SixPM, TimeConstants.Midnight) &&
               parkingPeriod.ExitDateTime.Before(TimeConstants.SixAm);
        }

        private static bool IsWeekendRateCalculation(ParkingPeriod parkingPeriod)
        {
            return parkingPeriod.EntryDateTime.IsWeekend() && parkingPeriod.ExitDateTime.IsWeekend();
        }
    }
}