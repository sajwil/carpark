using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates
{
    public interface IRateCalculator
    {
        string Name { get; }
        decimal Rate { get; }
        decimal GetTotalPrice(ParkingPeriod parkingPeriod);
        bool IsElligible(ParkingPeriod parkingPeriod);
    }
}