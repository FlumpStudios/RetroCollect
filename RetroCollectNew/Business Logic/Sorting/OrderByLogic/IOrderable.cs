using System;
using System.Linq;
using ModelData;

namespace ApplicationLayer.Business_Logic.Sorting.OrderByLogic
{
    public interface IOrderable
    {
        Func<IQueryable<GameListModel>, IOrderedQueryable<GameListModel>> GetOrder(bool switchSort);
    }
}