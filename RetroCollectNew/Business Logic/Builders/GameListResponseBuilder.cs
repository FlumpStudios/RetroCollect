using ApplicationLayer.Business_Logic.Sorting;
using ModelData.Request;
using ModelData.Responses;
using DataAccess.WorkUnits;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using ApplicationLayer.Business_Logic.Queries;
using Common.Dictionaries;
using Common.Extensions;
using HttpAccess;
using System.Collections.Generic;
using ModelData;

namespace ApplicationLayer.Business_Logic.Builders
{
    public class GameListResponseBuilder : IGameListResponseBuilder
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ISortingManager _sortingManager;

        private readonly IConfiguration _configuration;

        private readonly IUnitOFWork _unitOFWork;

        private readonly IHttpManager _httpManager;

        public GameListResponseBuilder(ISortingManager sortingManager, IConfiguration configuration, IUnitOFWork unitOFWork, IHttpManager httpManager)
        {          
            _sortingManager = sortingManager;
            _configuration = configuration;
            _unitOFWork = unitOFWork;
            _httpManager = httpManager;
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

            var defaultFromDate = _configuration.GetValue<string>("DefaultSearchDates:FromDate");

            var defaultToDate = _configuration.GetValue<string>("DefaultSearchDates:ToDate");
            

            //Retrieve sorted values for view
            var consoleList = Dictionaries.ConsoleDictionary;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<GameListModel> gameList = null;

            if (gameListRequestModel.ShowClientList)
            {
                gameList = _httpManager.GetClientResults(gameListRequestModel, userId).Result;
            }
            else
            {
                gameList= _httpManager.GetSortedResults(gameListRequestModel, userId).Result;
            }


            var currentPage = gameListRequestModel.Page ?? 1;         

            //If requset is to display only the clients games, check client DB for game ID's and only return games from game DB that exist in clients DB.
            if (gameListRequestModel.ShowClientList && !string.IsNullOrEmpty(userId))
            {
                //gameList = QueryHelper.InnerJoinClientListWithGameList(gameList, _unitOFWork.ClientRepo.Get(), userId);
            }
            
            


            const int PAGE_COUNT = 1000;
           // var pagedResults = gameList.ToPagedList(currentPage, resultsPerPage);

            return new GameListResponse(gameList,
                User.Identity.IsAuthenticated,
                consoleList,
                User.IsInRole(ADMIN_ROLE),
                gameListRequestModel.SortingOptions,
                currentPage, PAGE_COUNT,
                gameListRequestModel.Platform,
                gameListRequestModel.SortingOptions,
                gameListRequestModel.ShowClientList,
                 gameListRequestModel.ToDate ?? defaultToDate.FromUnix(),
                gameListRequestModel.FromDate ?? defaultFromDate.FromUnix()               
                );
        }
    }
}
