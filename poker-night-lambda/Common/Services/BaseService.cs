using Common.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IService
    {
        Task<IResponse> ExecuteGet();
        Task<IResponse> ExecutePost();
        Task<IResponse> ExecutePut();
        Task<IResponse> ExecuteDelete();
    }

    public class BaseService : IService
    {
        public virtual Task<IResponse> ExecuteDelete()
        {
            return Task.FromResult((IResponse)new ErrorResponse("Method not supported", (int)HttpStatusCode.NotImplemented));
        }

        public virtual Task<IResponse> ExecuteGet()
        {
            return Task.FromResult((IResponse)new ErrorResponse("Method not supported", (int)HttpStatusCode.NotImplemented));
        }

        public virtual Task<IResponse> ExecutePost()
        {
            return Task.FromResult((IResponse)new ErrorResponse("Method not supported", (int)HttpStatusCode.NotImplemented));
        }

        public virtual Task<IResponse> ExecutePut()
        {
            return Task.FromResult((IResponse)new ErrorResponse("Method not supported", (int)HttpStatusCode.NotImplemented));
        }

        protected ErrorResponse Forbidden()
        {
            return new ErrorResponse("Forbidden", (int)HttpStatusCode.Forbidden);
        }
    }
}
