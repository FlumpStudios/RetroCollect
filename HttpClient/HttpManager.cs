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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("user-key", "e71b082e22dee3f92e0ccd22c7b2fc4c");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var stringTask = client.GetAsync("https://api-endpoint.igdb.com/games/doom");

            var msg = stringTask;

            return null;
        }

        public IEnumerable<GameListModel>  GetName(string gameName)
        {
            throw new NotImplementedException();
        }
            
    }
}
