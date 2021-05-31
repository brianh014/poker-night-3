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
        [JsonPropertyName("queryStringParameters")]
        public QueryParams QueryParameters { get; set; }
        [JsonIgnore]
        public Player ParsedBody => JsonSerializer.Deserialize<Player>(Body);
    }

    public class RequestPathParams
    {
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }
    }

    public class QueryParams
    {
        [JsonPropertyName("byLoginId")]
        public string ByLoginId { get; set; }
        [JsonPropertyName("stats")]
        public string Stats { get; set; }
    }
}
