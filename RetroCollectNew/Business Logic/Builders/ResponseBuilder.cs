using ModelData.Request;
using ModelData.Responses;
using ModelData;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using X.PagedList;

namespace ApplicationLayer.Business_Logic.Builders
{
    public class ResponseBuilder
    {
        public GameListResponse GetResponse(GameListRequest gameListRequestModel,
            ClaimsPrincipal User,
            IEnumerable<GameListModel> gameList
            )
        {

            var currentPage = gameListRequestModel.Page ?? 1;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pagedResults = gameList.ToPagedList(currentPage, 10);

            return new GameListResponse(pagedResults,
                User.Identity.IsAuthenticated,
                gameList.Select(x => x.Format).Distinct(),
                User.IsInRole("Admin"),
                gameListRequestModel.SortingOptions,
                currentPage, pagedResults.PageCount,
                gameListRequestModel.Format,
                gameListRequestModel.SortingOptions,
                gameListRequestModel.ShowClientList);
            }
    }
}
