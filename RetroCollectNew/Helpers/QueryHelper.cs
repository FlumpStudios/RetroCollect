using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Helpers
{
    public static class QueryHelper
    {
        /// <summary>
        /// Inner join client list of game ids with gamelist, to return a list of games in client DB
        /// </summary>
        /// <param name="gameList"></param>
        /// <param name="clientList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<GameListModel> InnerJoinClientListWithGameList(IEnumerable<GameListModel> gameList, 
                      IEnumerable< ClientListModel> clientList, 
                      string userId)
        {
            return    (from s in gameList
                       join c in clientList on s.Id equals c.GameId
                       where c.GameId == s.Id && userId == c.UserId
                       select s).ToList();
        }
    }
}
