using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Contracts
{
    public interface IResponse
    {
        [JsonPropertyName("body")]
        string Body { get; set; }
        [JsonPropertyName("statusCode")]
        int StatusCode { get; set; }
        [JsonPropertyName("isBase64Encoded")]
        public bool IsBase64Encoded => false;
        [JsonPropertyName("headers")]
        Headers Headers { get; set; }
        [JsonPropertyName("multiValueHeaders")]
        MultiValueHeaders MultiValueHeaders { get; set; }
    }

    public class BaseResponse : IResponse
    {
        public BaseResponse(object body, int? statusCode = null, string contentType = null)
        {
            Body = JsonSerializer.Serialize(body);
            StatusCode = statusCode ?? 200;
            Headers = new Headers
            {
                ContentType = contentType ?? "application/json"
            };
        }

        public BaseResponse() { }

        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("isBase64Encoded")]
        public bool IsBase64Encoded => false;
        [JsonPropertyName("headers")]
        public Headers Headers { get; set; }
        [JsonPropertyName("multiValueHeaders")]
        public MultiValueHeaders MultiValueHeaders { get; set; }
    }

    public class Headers
    {
        [JsonPropertyName("content-type")]
        public string ContentType { get; set; }
    }

    public class MultiValueHeaders
    {

    }
}
