using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Models
{
    [DynamoDBTable("games")]
    public class Game
    {
        [DynamoDBHashKey("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [DynamoDBProperty("date")]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [DynamoDBProperty("closed")]
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        [DynamoDBProperty("buyIn")]
        [JsonPropertyName("buyIn")]
        public decimal BuyIn { get; set; }
        [DynamoDBProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [DynamoDBProperty("players")]
        [JsonPropertyName("players")]
        public List<PlayerResult> Players { get; set; }
    }
}
