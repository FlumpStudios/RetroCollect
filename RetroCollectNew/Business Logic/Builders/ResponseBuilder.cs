using ModelData.Request;
using ModelData.Responses;
using ModelData;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Common.Dictionaries;
using System.Threading.Tasks;

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

            return new GameListResponse(gameList,
                User.Identity.IsAuthenticated,
                Dictionaries.ConsoleDictionary,
                User.IsInRole("Admin"),
                gameListRequestModel.SortingOptions,
                currentPage, 1000,
                gameListRequestModel.Platform,
                gameListRequestModel.SortingOptions,
                gameListRequestModel.ShowClientList,
                gameListRequestModel.FromDate,
                gameListRequestModel.ToDate,
                5
                );
            }
    }
}
