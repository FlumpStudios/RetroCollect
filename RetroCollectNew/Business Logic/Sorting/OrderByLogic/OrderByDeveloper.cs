using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Business_Logic.Sorting.OrderByLogic
{
    public class OrderByDeveloper : IOrderable
    {
        public Func<IQueryable<GameListModel>, IOrderedQueryable<GameListModel>> GetOrder(bool switchSort)
        {
            if (switchSort) return x => x.OrderByDescending(y => y.Developer);
            return x => x.OrderBy(y => y.Developer);
        }
    }
}
