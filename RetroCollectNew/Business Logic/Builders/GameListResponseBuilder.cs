using ApplicationLayer.Business_Logic.Sorting;
using ModelData.Request;
using ModelData.Responses;
using DataAccess.WorkUnits;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using X.PagedList;
using Common.Helpers;
using ApplicationLayer.Business_Logic.Queries;

namespace ApplicationLayer.Business_Logic.Builders
{
    public class GameListResponseBuilder : IGameListResponseBuilder
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ISortingManager _sortingManager;

        private readonly IConfiguration _configuration;

        private readonly IUnitOFWork _unitOFWork;

        public GameListResponseBuilder(ISortingManager sortingManager, IConfiguration configuration, IUnitOFWork unitOFWork)
        {          
            _sortingManager = sortingManager;
            _configuration = configuration;
            _unitOFWork = unitOFWork;
        }

        /// <summary>
        /// Build Response for GameListController
        /// </summary>
        /// <param name="gameListRequestModel"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public GameListResponse GetResponse(GameListRequest gameListRequestModel,
                ClaimsPrincipal User)
        {
            const string ADMIN_ROLE = "Admin";

            //Get the amount of items to return to page for pagination
            var resultsPerPage = _configuration.GetValue<int>("Paging:ResultsPerPage");

            //Retrieve sorted values for view
            var gameList = _sortingManager.GetFilteredResults(gameListRequestModel);

            //Get the list of all avaiable console in DB to return as selectable options in view
            var consoleList = _unitOFWork.GameRepo.GetDistinct(x => x.Format);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentPage = gameListRequestModel.Page ?? 1;         

            //If requset is to display only the clients games, check client DB for game ID's and only return games from game DB that exist in clients DB.
            if (gameListRequestModel.ShowClientList && !string.IsNullOrEmpty(userId))
            {
                gameList = QueryHelper.InnerJoinClientListWithGameList(gameList, _unitOFWork.ClientRepo.Get(), userId);
            }

            var pagedResults = gameList.ToPagedList(currentPage, resultsPerPage);

            return new GameListResponse(pagedResults,
                User.Identity.IsAuthenticated,
                consoleList,
                User.IsInRole(ADMIN_ROLE),
                gameListRequestModel.SortingOptions,
                currentPage, pagedResults.PageCount,
                gameListRequestModel.Format,
                gameListRequestModel.SortingOptions,
                gameListRequestModel.ShowClientList);
        }
    }
}
