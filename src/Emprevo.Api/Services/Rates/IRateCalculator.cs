using Emprevo.Api.Models;

namespace Emprevo.Api.Services.Rates
{
    public interface IRateCalculator
    {
        string Name { get; }
        decimal GetTotalPrice(ParkingPeriod parkingPeriod);
    }
}