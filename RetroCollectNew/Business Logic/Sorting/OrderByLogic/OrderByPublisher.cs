using ModelData;
using System;
using System.Linq;

namespace ApplicationLayer.Business_Logic.Sorting.OrderByLogic
{
    public class OrderByPublisher : IOrderable
    {
        public Func<IQueryable<GameListModel>, IOrderedQueryable<GameListModel>> GetOrder(bool switchSort)
        {
            if (switchSort) return x => x.OrderByDescending(y => y.Publisher);
            return x => x.OrderBy(y => y.Publisher);
        }
    }
}
