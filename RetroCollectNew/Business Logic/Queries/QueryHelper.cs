using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLayer.Business_Logic.Queries
{

    //TODO: move from static class to standard class and inject in throug IOC

    public static class QueryHelper
    {

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
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
            IEnumerable<GameListModel> query = null;

            try
            {
                //query = (from s in gameList
                //            join c in clientList on s.Id equals c.GameId
                //            where c.GameId == s.Id && userId == c.UserId
                //            select s).ToList();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error joining Client list with game list");
            }
        
            return query;
        }
    }
}
