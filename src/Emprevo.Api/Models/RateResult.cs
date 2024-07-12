namespace Emprevo.Api.Models
{
    public interface IRateResult
    {
        string RateName { get; }
        decimal TotalPrice { get; }
    }

    public struct RateResult : IRateResult
    {
        public string RateName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}