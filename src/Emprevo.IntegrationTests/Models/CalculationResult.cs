using System.Text.Json.Serialization;

namespace Emprevo.IntegrationTests.Models
{
    public class CalculationResult
    {
        [JsonPropertyName("rateName")]
        public string RateName { get; set; } = string.Empty;
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}