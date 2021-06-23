using Common.Contracts;
using Common.Services;
using Players.Contracts;
using Common.Data;
using System.Net;
using System.Threading.Tasks;

namespace Players.Services
{
    public class PlayerService : BaseService
    {
        private Request Request { get; set; }
        private PlayersDao _dao { get; set; } 

        public PlayerService(Request request)
        {
            Request = request;
            _dao = new PlayersDao();
        }

        public override async Task<IResponse> ExecuteGet()
        {
            if (Request.PathParameters == null && !string.IsNullOrEmpty(Request.QueryParameters?.Stats))
            {
                var stats = await _dao.GetPlayerStats();
                return new BaseResponse(stats);
            }
            else if (Request.PathParameters == null)
            {
                var players = await _dao.GetPlayers();
                return new BaseResponse(players);
            }
            else if (!string.IsNullOrWhiteSpace(Request.PathParameters.PlayerId))
            {
                if (Request.QueryParameters?.ByLoginId == "true")
                {
                    return new BaseResponse(await _dao.GetPlayerByLogin(Request.PathParameters.PlayerId, null));
                }

                return new BaseResponse(await _dao.GetPlayer(Request.PathParameters.PlayerId));
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }

        public override async Task<IResponse> ExecutePost()
        {
            if (!Request.IsAdmin) return Forbidden();
            if (!string.IsNullOrWhiteSpace(Request.Body) && Request.PathParameters == null)
            {
                var player = await _dao.CreatePlayer(Request.ParsedBody);
                return new BaseResponse(player);
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }

        public override async Task<IResponse> ExecutePut()
        {
            if (!Request.IsAdmin) return Forbidden();
            if (!string.IsNullOrEmpty(Request.Body) && !string.IsNullOrWhiteSpace(Request.PathParameters?.PlayerId))
            {
                var player = await _dao.UpdatePlayer(Request.ParsedBody);
                return new BaseResponse(player);
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }
    }
}
