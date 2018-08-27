using System.Collections.Generic;
using ApplicationLayer.Models.Request;
using ModelData;

namespace ApplicationLayer.Business_Logic.Sorting
{
    public interface ISortingManager
    {
        IEnumerable<GameListModel> GetFilteredResults(GameListRequest gameListRequestModel);
    }
}