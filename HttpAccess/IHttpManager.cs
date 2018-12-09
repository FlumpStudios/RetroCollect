using System.Collections.Generic;
using System.Threading.Tasks;
using ModelData;
using ModelData.Request;

namespace HttpAccess
{
    public interface IHttpManager
    {
        Task<IEnumerable<GameListModel>> GetSortedResults(GameListRequest gameListRequest, string userId = null);
        Task<IEnumerable<GameListModel>> GetClientResults(GameListRequest gameListRequest, string userId = null);
    }
}