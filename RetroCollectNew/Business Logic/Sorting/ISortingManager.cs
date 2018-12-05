using System.Collections.Generic;
using ModelData;
using ModelData.Request;
using ModelData.Responses;

namespace ApplicationLayer.Business_Logic.Sorting
{
    public interface ISortingManager
    {
        IEnumerable<GameListModel> GetFilteredResults(GameListRequest gameListRequestModel);
    }
}