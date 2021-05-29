using Amazon.DynamoDBv2.DataModel;
using System;
using System.Text.Json.Serialization;

namespace Common.Models
{
    [DynamoDBTable("players")]
    public class Player
    {
        [DynamoDBHashKey("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [DynamoDBProperty("loginId")]
        [JsonPropertyName("loginId")]
        public string LoginId { get; set; }
        [DynamoDBProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [DynamoDBProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [DynamoDBProperty("lastLogIn")]
        [JsonPropertyName("lastLogIn")]
        public DateTime? LastLogIn { get; set; }
    }
}
