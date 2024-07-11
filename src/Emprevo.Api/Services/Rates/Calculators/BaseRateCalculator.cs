using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates.Calculators
{
    public abstract class BaseRateCalculator : IRateCalculator
    {
        public abstract string Name { get; }
        public virtual decimal Rate { get; }
        public virtual decimal GetTotalPrice(ParkingPeriod parkingPeriod) => Rate;
        public virtual bool IsElligible(ParkingPeriod parkingPeriod) => true;
    }
}
