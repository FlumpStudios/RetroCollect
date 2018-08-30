using System.Security.Claims;
using ApplicationLayer.Models.Request;
using ApplicationLayer.Models.Responses;

namespace ApplicationLayer.Business_Logic.Builders
{
    public interface IGameListResponseBuilder
    {
        GameListResponse GetResponse(GameListRequest gameListRequestModel, ClaimsPrincipal User);
    }
}