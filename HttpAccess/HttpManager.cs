using ModelData.Request;
using ModelData;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Common.Dictionaries;
using System.Text;
using System.Linq;
using Common.Extensions;
using Microsoft.Extensions.Configuration;
using DataAccess.WorkUnits;
using System.Linq.Dynamic.Core;
using Caching;

namespace HttpAccess
{
    public class HttpManager : IHttpManager
    {
        private string _userId = null;

        private readonly IConfiguration _configuration;
        private readonly IUnitOFWork _unitOFWork;
        private readonly ICachingManager _cachingManager;

        HttpClient client = new HttpClient();

        public HttpManager(IConfiguration configuration, IUnitOFWork unitOFWork, ICachingManager cachingManager)
        {
            _configuration = configuration;
            _unitOFWork = unitOFWork;
            _cachingManager = cachingManager;
        }


        public async Task<IEnumerable<GameListModel>> GetSortedResults(GameListRequest gameListRequest, string userId = null)
        {
            _userId = userId;
            //TODO: Massive clean up on this method!

            #region Setup Http client
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");
            #endregion

            #region date filters
            var defaultFromDate = _configuration.GetSection("DefaultSearchDates:FromDate").Value.ToString();
            var defaultToDate = _configuration.GetSection("DefaultSearchDates:ToDate").Value.ToString();
            int resultsPerPage = int.Parse(_configuration.GetSection("Paging:ResultsPerPage").Value);
            var useTodaysDate = bool.Parse(_configuration.GetSection("DefaultSearchDates:UseTodaysDate").Value);

            string toDate = null;
            string fromDate = null;
            if (string.IsNullOrEmpty(gameListRequest.FromDate))
            {
                fromDate = "&filter[first_release_date][gt]=" + defaultFromDate;
            }
            else
            {
                var d = gameListRequest.FromDate.ToDateTime();
                fromDate = "&filter[first_release_date][gt]=" + d.ToUnix().ToString();
            }


            if (string.IsNullOrEmpty(gameListRequest.ToDate))
            {
                toDate = "&filter[first_release_date][lte]=" + (useTodaysDate ? DateTime.Now.ToString("dd/MM/yyyy").ToDateTime().ToUnix().ToString() : defaultToDate);
            }
            else
            {
                var d = gameListRequest.ToDate.ToDateTime();
                toDate = "&filter[first_release_date][lte]=" + d.ToUnix().ToString();
            }
            #endregion

            #region paging

            if (gameListRequest.Page != null && gameListRequest.Page > 0) gameListRequest.Page--;
            else gameListRequest.Page = 0;
            #endregion

            #region Searching
            string searchType = string.IsNullOrEmpty(gameListRequest.Platform) ? "any" : "in";
            string searchString = string.IsNullOrEmpty(gameListRequest.SearchText) ? "" : string.Format("search={0}&", gameListRequest.SearchText);
            searchString = searchString.Replace(" ", "%20");
            #endregion

            #region Sorting and filtering

            if (!string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                gameListRequest.SortingOptions = gameListRequest.SortingOptions.Replace(" ", "_").ToLower();
            }

            string orderOption = gameListRequest.Switchsort ? ":asc" : ":desc";
            string orderByOption = "";
            string filterText = "";

            if (!string.IsNullOrEmpty(gameListRequest.Platform) && gameListRequest.Platform != "Other")
            {
                filterText = string.Format("&filter[platforms][{0}]={1}", searchType, gameListRequest.Platform);
            }
            else if (gameListRequest.Platform == "Other")
            {
                filterText = string.Format("&filter[platforms][{0}]={1}", "not_in", GetConsoleListString());
            }
            else
            {
                filterText = "";
            }

            if (!string.IsNullOrEmpty(gameListRequest.SearchText) && string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                orderByOption = "";
                orderOption = "";
            }
            else if (string.IsNullOrEmpty(gameListRequest.SearchText) && string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                orderByOption = "&order=popularity";
            }
            else
            {
                orderByOption = "&order=" + gameListRequest.SortingOptions;
            }

            #endregion

            string queryString = string.Format("https://api-endpoint.igdb.com/games/{7}?{4}fields=cover,name,first_release_date,popularity,rating,platforms{0}{1}&offset={2}{3}&limit={8}{5}{6}",
                orderByOption,
                orderOption,
                gameListRequest.Page * resultsPerPage,
                filterText,
                searchString,
                fromDate ?? "",
                toDate ?? "",
                gameListRequest.ShowClientList ? GetUserGameList() : "",
                resultsPerPage
                );

            //string msg = await client.GetStringAsync(queryString);
            var msg = await GetResultsFromAPI(queryString);

            IEnumerable<GameListModel> result = JsonConvert.DeserializeObject<IEnumerable<GameListModel>>(msg);

            //Convert returned Unix time stamps to date strings
            result.ToList().ForEach(X => X.First_release_date = string.IsNullOrEmpty(X.First_release_date) ? "No Release Date Available" : DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(X.First_release_date)).ToString("dd/MM/yyyy"));

            return result;
        }

        public async Task<IEnumerable<GameListModel>> GetClientResults(GameListRequest gameListRequest, string userId = null)
        {
            _userId = userId;
            int resultsPerPage = int.Parse(_configuration.GetSection("Paging:ResultsPerPage").Value);

            int pageSkip = 1;
            if (gameListRequest.Page != null)
            {
                pageSkip = (int)gameListRequest.Page * resultsPerPage;
            }

            #region Setup Http client
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");
            #endregion

            string queryString = string.Format("https://api-endpoint.igdb.com/games/{0}?fields=cover,name,first_release_date,popularity,rating,platforms", GetUserGameList());
            var msg = await GetResultsFromAPI(queryString);

            IEnumerable<GameListModel> result = JsonConvert.DeserializeObject<IEnumerable<GameListModel>>(msg);

            result.ToList().ForEach(X => X.First_release_date = string.IsNullOrEmpty(X.First_release_date) ? "No Release Date Available" : DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(X.First_release_date)).ToString("dd/MM/yyyy"));

            if (gameListRequest.FromDate != null && gameListRequest.ToDate != null)
            {
                result = result.Where(x => x.First_release_date.ToDateTime().ToUnix() > gameListRequest.FromDate.ToDateTime().ToUnix() && x.First_release_date.ToDateTime().ToUnix() < gameListRequest.ToDate.ToDateTime().ToUnix());
            }

            if (!string.IsNullOrEmpty(gameListRequest.SearchText))
            {
                result = result.Where(x => x.Name.ToUpper().Contains(gameListRequest.SearchText.ToUpper()));
            }

            if (!string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                gameListRequest.SortingOptions = gameListRequest.SortingOptions.Replace(" ", "_").ToLower();
                var sortOption = gameListRequest.Switchsort ? " descending" : "";
                result = result.AsQueryable().OrderBy(gameListRequest.SortingOptions + sortOption);
            }

            if (!string.IsNullOrEmpty(gameListRequest.Platform))
            {
                result = result.Where(x => x.Platforms.Contains(gameListRequest.Platform));
            }

            return result.Skip(pageSkip).Take(resultsPerPage);
        }

        public async Task<GameListModel> GetByID(string id)
        {
            #region Setup Http client
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");
            #endregion

            string queryString = string.Format("https://api-endpoint.igdb.com/games/{0}?fields=*", id);

            var msg = await client.GetStringAsync(queryString);

            IEnumerable<GameListModel> mappedResult = JsonConvert.DeserializeObject<IEnumerable<GameListModel>>(msg);

            GameListModel result = mappedResult.FirstOrDefault();

            return result;
        }

        private string GetConsoleListString()
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in Dictionaries.ConsoleDictionary)
            {
                s.Append(item.Key + ",");
            }
            s.Length = s.Length - 1;
            var result = s.ToString();
            return s.ToString();
        }

        private string GetUserGameList()
        {
            var DB = _unitOFWork.ClientRepo.Get(filter: x => x.UserId == _userId);
            StringBuilder responseString = new StringBuilder();
            foreach (var item in DB)
            {
                responseString.Append(item.GameId.ToString() + ",");
            }

            responseString.Length = responseString.Length - 1;
            return responseString.ToString();
        }

        private async Task<string> GetResultsFromAPI(string queryString)
        {

            //Use the query string as a key so if query is the same we don't have to make an API call.
            return _cachingManager.GetCache<string>(queryString) ??
                _cachingManager.SetCache(await client.GetStringAsync(queryString), queryString);
        }
    }

}