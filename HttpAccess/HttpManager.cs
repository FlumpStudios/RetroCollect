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
using System.Globalization;
using Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace HttpAccess
{
    public class HttpManager : IHttpManager
    {
        private readonly IConfiguration _configuration;

        HttpClient client = new HttpClient();

        public HttpManager(IConfiguration configuration)
        {
            _configuration = configuration;          
        }

        private async Task<string> GetResultCount(GameListRequest gameListRequest)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");

            string queryString = string.Format("https://api-endpoint.igdb.com/count/?search={0}&filter[platforms][eq]={1}", gameListRequest.SearchText, gameListRequest.Platform);
            
            var stringTask = client.GetStringAsync(queryString);

            var msg = await stringTask;
            return msg;
        }

        public async Task<IEnumerable<GameListModel>> GetSortedResults(GameListRequest gameListRequest)
        {

            //TODO: Massive clean up on this method!

#region Setup Http client
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");
#endregion

#region date filters
            var defaultFromDate = _configuration.GetSection("DefaultSearchDates:FromDate").Value.ToString();
            var defaultToDate = _configuration.GetSection("DefaultSearchDates:ToDate").Value.ToString();

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
                toDate = "&filter[first_release_date][lte]=" + defaultToDate;
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
                orderByOption = "&order="+gameListRequest.SortingOptions;
            }

#endregion

            string queryString = string.Format("https://api-endpoint.igdb.com/games/?{4}fields=cover,name,first_release_date,popularity,rating{0}{1}&offset={2}{3}&limit=50{5}{6}",
                orderByOption,
                orderOption,
                gameListRequest.Page * 10 ?? 0,
                filterText,
                searchString,
                fromDate ?? "",
                toDate ?? ""
                );

            //string msg = await client.GetStringAsync(queryString);

            IEnumerable<GameListModel> result = JsonConvert.DeserializeObject<IEnumerable<GameListModel>>(await client.GetStringAsync(queryString));

            //Convert returned Unix time stamps to date strings
            result.ToList().ForEach(X => X.First_release_date = string.IsNullOrEmpty(X.First_release_date) ? "No Release Date Available" : DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(X.First_release_date)).ToString("dd/MM/yyyy"));

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
    }

}