using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Business_Logic.Sorting.OrderByLogic
{
    public class OrderByName : IOrderable
    {
        public Func<IQueryable<GameListModel>, IOrderedQueryable<GameListModel>> GetOrder(bool switchSort)
        {
            if (switchSort) return x => x.OrderByDescending(y => y.Name);
            return x => x.OrderBy(y => y.Name);
        }
    }
}
