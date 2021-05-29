using Amazon.Lambda.Core;
using Common.Contracts;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.Services
{
    public class LambdaHandler
    {
        public IRequest Request { get; set; }
        public IService Service { get; set; }

        public LambdaHandler(IRequest request, IService service)
        {
            Request = request;
            Service = service;
        }

        public async Task<IResponse> GetResponse()
        {
            LambdaLogger.Log($"REQUEST: {System.Text.Json.JsonSerializer.Serialize(Request)}");

            try
            {
                if (Request?.RequestContext?.Http != null)
                {
                    switch (Request?.RequestContext.Http.Method)
                    {
                        case "GET":
                            return await Service.ExecuteGet();
                        case "POST":
                            return await Service.ExecutePost();
                        case "PUT":
                            return await Service.ExecutePut();
                        case "DELETE":
                            return await Service.ExecuteDelete();
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"ERROR: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return new ErrorResponse(ex.Message, (int)HttpStatusCode.InternalServerError);
            }

            return new ErrorResponse("Method not supported", (int)HttpStatusCode.NotImplemented);
        }
    }
}
