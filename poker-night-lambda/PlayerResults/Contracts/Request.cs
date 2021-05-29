using Common.Contracts;
using Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Players.Contracts
{
    public class Request : BaseRequest
    {
        [JsonPropertyName("pathParameters")]
        public RequestPathParams PathParameters { get; set; }
        [JsonIgnore]
        public PlayerResult ParsedBody => JsonSerializer.Deserialize<PlayerResult>(Body);
    }

    public class RequestPathParams
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; }
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }
    }
}
