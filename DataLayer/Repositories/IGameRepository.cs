using ModelData;
using System.Collections.Generic;

namespace DataAccess.Repositories
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
