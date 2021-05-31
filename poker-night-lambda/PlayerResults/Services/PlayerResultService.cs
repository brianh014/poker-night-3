using Common.Contracts;
using Common.Data;
using Common.Services;
using Players.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace Games.Services
{
    public class PlayerResultService : BaseService
    {
        private Request Request { get; set; }
        private GamesDao _dao { get; set; }

        public PlayerResultService(Request request)
        {
            Request = request;
            _dao = new GamesDao();
        }

        public override async Task<IResponse> ExecutePost()
        {
            if (!string.IsNullOrEmpty(Request.Body))
            {
                if (!string.IsNullOrWhiteSpace(Request.PathParameters?.GameId))
                {
                    if (Request.CurrentUser != Request.ParsedBody.PlayerId && !Request.IsAdmin) return Forbidden();
                    return new BaseResponse(await _dao.AddPlayer(Request.PathParameters.GameId, Request.ParsedBody));
                }
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }

        public override async Task<IResponse> ExecutePut()
        {
            if (!string.IsNullOrEmpty(Request.Body))
            {
                if (!string.IsNullOrWhiteSpace(Request.PathParameters?.GameId) && !string.IsNullOrWhiteSpace(Request.PathParameters?.PlayerId))
                {
                    if (!Request.IsAdmin && Request.ParsedBody.PlayerId != Request.CurrentUser) return Forbidden();
                    return new BaseResponse(await _dao.UpdatePlayerResult(Request.PathParameters.GameId, Request.ParsedBody));
                }
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }

        public override async Task<IResponse> ExecuteDelete()
        {
            if (!Request.IsAdmin) return Forbidden();
            if (!string.IsNullOrWhiteSpace(Request.PathParameters?.GameId) && !string.IsNullOrWhiteSpace(Request.PathParameters?.PlayerId))
            {
                await _dao.DeletePlayerResult(Request.PathParameters.GameId, Request.PathParameters.PlayerId);
                return new BaseResponse("Player result deleted");
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }
    }
}
