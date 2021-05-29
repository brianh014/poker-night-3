namespace Common.Contracts
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(string message, int errorCode)
        {
            Body = $"<html><body><p>{message}</p></body></html>";
            StatusCode = errorCode;
            Headers = new Headers
            {
                ContentType = "text/html"
            };
            MultiValueHeaders = new MultiValueHeaders();
        }
    }
}
