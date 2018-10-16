using ApplicationLayer.Models.Request;
using ApplicationLayer.Models.Responses;
using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace ApplicationLayer.Helpers
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
            var pagedResults = gameList.ToPagedList(currentPage, resultsPerPage);

            return new GameListResponse(pagedResults,
                User.Identity.IsAuthenticated,
                gameList.Select(x => x.Format).Distinct(),
                User.IsInRole("Admin"),
                gameListRequestModel.SortingOptions,
                currentPage, pagedResults.PageCount,
                gameListRequestModel.Format,
                gameListRequestModel.SortingOptions,
                gameListRequestModel.ShowClientList)        }
    }
}
