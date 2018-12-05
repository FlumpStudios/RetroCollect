using System.Security.Claims;
using ModelData.Request;
using ModelData.Responses;

namespace ApplicationLayer.Business_Logic.Builders
{
    public interface IGameListResponseBuilder
    {
        GameListResponse GetResponse(GameListRequest gameListRequestModel, ClaimsPrincipal User);
    }
}