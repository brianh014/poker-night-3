using Common.Contracts;
using Common.Data;
using Common.Models;
using Common.Services;
using Games.Contracts;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Games.Services
{
    public class GameService : BaseService
    {
        private Request Request { get; set; }
        private GamesDao _dao { get; set; }

        public GameService(Request request)
        {
            Request = request;
            _dao = new GamesDao();
        }

        public override async Task<IResponse> ExecuteGet()
        {
            if (Request.PathParameters == null)
            {
                return new BaseResponse(await _dao.GetGames());
            }
            else if (!string.IsNullOrWhiteSpace(Request.PathParameters.GameId))
            {
                return new BaseResponse(await _dao.GetGame(Request.PathParameters.GameId));
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }

        public override async Task<IResponse> ExecutePost()
        {
            if (!Request.IsAdmin) return Forbidden();
            if (!string.IsNullOrEmpty(Request.Body))
            {
                if (Request.PathParameters == null)
                {
                    return new BaseResponse(await _dao.CreateGame(Request.ParsedBody));
                }
            }
            else if (Request.RawPath.Contains("toggle-lock") && !string.IsNullOrWhiteSpace(Request.PathParameters.GameId))
            {
                return new BaseResponse(await _dao.ToggleLock(Request.PathParameters.GameId));
            }

            return new ErrorResponse("Bad request", (int)HttpStatusCode.BadRequest);
        }
    }
}
