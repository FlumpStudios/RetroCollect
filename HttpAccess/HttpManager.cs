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

namespace HttpAccess
{
    public class HttpManager : IHttpManager
    {
        HttpClient client = new HttpClient();

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
            if (gameListRequest.Page != null && gameListRequest.Page > 0) gameListRequest.Page--;
            else gameListRequest.Page = 0;

       
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");

            string searchType = string.IsNullOrEmpty(gameListRequest.Platform) ? "any" : "in";

            if(!string.IsNullOrEmpty(gameListRequest.SortingOptions))
            { 
                gameListRequest.SortingOptions = gameListRequest.SortingOptions.Replace(" ", "_").ToLower();
            }
            string orderOption = gameListRequest.Switchsort ? ":desc" : ":asc";
            string orderByOption = "";

            if (!string.IsNullOrEmpty(gameListRequest.SearchText) && string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                orderByOption = "";
                orderOption = "";
            }
            else if (string.IsNullOrEmpty(gameListRequest.SearchText) && string.IsNullOrEmpty(gameListRequest.SortingOptions))
            {
                orderByOption = "&order=name";
            }
            else
            {
                orderByOption = "&order="+gameListRequest.SortingOptions;
            }


            string searchString = string.IsNullOrEmpty(gameListRequest.SearchText) ? "" : string.Format("search={0}&max=5&", gameListRequest.SearchText);

            string queryString = string.Format("https://api-endpoint.igdb.com/games/?{4}fields=cover,name,first_release_date,rating{0}{1}&offset={2}{3}&limit=50",
                orderByOption,
                orderOption,
                gameListRequest.Page * 10 ?? 0,
                string.Format("&filter[platforms][{0}]={1}",searchType, gameListRequest.Platform ?? GetConsoleListString()),
                searchString);

            string msg = await client.GetStringAsync(queryString);

            IEnumerable<GameListModel> result = JsonConvert.DeserializeObject<IEnumerable<GameListModel>>(msg);

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