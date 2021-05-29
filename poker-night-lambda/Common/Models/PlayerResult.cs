using Amazon.DynamoDBv2.DataModel;
using System.Text.Json.Serialization;

namespace Common.Models
{
    public class PlayerResult
    {
        [DynamoDBProperty("playerId")]
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }
        [DynamoDBProperty("buyIn")]
        [JsonPropertyName("buyIn")]
        public decimal? BuyIn { get; set; }
        [DynamoDBProperty("cashOut")]
        [JsonPropertyName("cashOut")]
        public decimal? CashOut { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("loginId")]
        public string LoginId { get; set; }
    }
}
