using Common.Contracts;
using Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Games.Contracts
{
    public class Request : BaseRequest
    {
        [JsonPropertyName("pathParameters")]
        public RequestPathParams PathParameters { get; set; }
        [JsonIgnore]
        public Game ParsedBody => JsonSerializer.Deserialize<Game>(Body);
    }

    public class RequestPathParams
    {
        [JsonPropertyName("gameId")]
        public string GameId{ get; set; }
    }
}
