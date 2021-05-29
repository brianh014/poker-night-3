using System.Text.Json.Serialization;

namespace Common.Contracts
{
    public interface IRequest
    {
        [JsonPropertyName("rawPath")]
        public string RawPath { get; set; }
        [JsonPropertyName("requestContext")]
        RequestContext RequestContext { get; set; }
        [JsonPropertyName("body")]
        string Body { get; set; }
        [JsonPropertyName("headers")]
        RequestHeaders Headers { get; set; }
        [JsonIgnore]
        bool IsAdmin { get; }
    }

    public class BaseRequest : IRequest
    {
        [JsonPropertyName("rawPath")]
        public string RawPath { get; set; }
        [JsonPropertyName("requestContext")]
        public RequestContext RequestContext { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("headers")]
        public RequestHeaders Headers { get; set; }
        [JsonIgnore]
        public bool IsAdmin
        {
            get {
                var groups = RequestContext?.Authorizer?.Jwt?.Claims?.Groups;
                if (groups != null && groups.Contains("Admin")) return true;
                return false;
            }
        }
        [JsonIgnore]
        public string CurrentUser => RequestContext?.Authorizer?.Jwt?.Claims?.Sub;
    }

    public class RequestContext
    {
        [JsonPropertyName("http")]
        public HttpRequest Http { get; set; }
        [JsonPropertyName("authorizer")]
        public Authorizer Authorizer { get; set; }
    }

    public class HttpRequest
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }
    }

    public class RequestHeaders
    {
        [JsonPropertyName("authorization")]
        public string Authorization { get; set; }
    }

    public class Authorizer
    {
        [JsonPropertyName("jwt")]
        public Jwt Jwt { get; set; }
    }

    public class Jwt
    {
        [JsonPropertyName("claims")]
        public Claims Claims { get; set; }
    }

    public class Claims
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
        [JsonPropertyName("cognito:groups")]
        public string Groups { get; set; }
    }
}
