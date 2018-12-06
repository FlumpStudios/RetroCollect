using ModelData.Request;
using ModelData;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HttpAccess
{
    public class HttpManager
    {
        static HttpClient client = new HttpClient();


        public GameListModel GetAll()
        {
           var foo = ProcessRepositories(null);

            return null;
        }
        private static async Task GetConsleList()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");

            var stringTask = client.GetStringAsync("https://api-endpoint.igdb.com/games/fields=name,release_dates");

            var msg = await stringTask;
            Console.Write(msg);

        }

        private static async Task ProcessRepositories(GameListRequest gameListRequest)
        {


            client.DefaultRequestHeaders.Accept.Clear();   
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));          
            client.DefaultRequestHeaders.Add("user-key", "b0d327604a696f914cafb0a23d782e6f");

            var stringTask = client.GetStringAsync("https://api-endpoint.igdb.com/games/?fields=cover,name,first_release_date,rating&order=name&offset=100&filter[platform][any]=99,48,49");

            var msg = await stringTask;
                Console.Write(msg);
        }

        public IEnumerable<GameListModel> GetName(string gameName)
        {
            throw new NotImplementedException();
        }

    }
}
