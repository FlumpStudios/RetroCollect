using RetroCollectNew.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroCollectNew.Data.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<GameListModel> GetGames();
        GameListModel GetGameByID(int? gameId);
        void InsertGame(GameListModel game);
        void DeleteGame(GameListModel gameId);
        void UpdateGame(GameListModel game);
        void Save();
    }
}
