using System.Text.Json.Serialization;

namespace IntegrationService.DTO
{
    public class CurrencyRates
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }


        [JsonPropertyName("eur")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
