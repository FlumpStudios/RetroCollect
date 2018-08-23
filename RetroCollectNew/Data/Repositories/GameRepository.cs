using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetroCollectNew.Models.DataModel;
using RetroCollectNew.Models.Requests;
using RetroCollectNew.Models;
using Microsoft.EntityFrameworkCore;

namespace RetroCollectNew.Data.Repositories
{
    public class GameRepository : IGameRepository, IDisposable
    {

        private readonly RetroCollectNewContext _gameContext;
        public GameRepository(RetroCollectNewContext gameContext)
        {
            _gameContext = gameContext;
        }

        public IEnumerable<GameListModel> GetGames() => _gameContext.GameListModel.ToList();
        public GameListModel GetGameByID(int? gameId) => _gameContext.GameListModel.SingleOrDefault(m => m.Id == gameId);
        public void InsertGame(GameListModel game) => _gameContext.Add(game);
        public void UpdateGame(GameListModel game) => _gameContext.Update(game);
        public void DeleteGame(GameListModel game)
        {
            _gameContext.GameListModel.Remove(game);
        }

        public void Save() => _gameContext.SaveChanges();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _gameContext.Dispose();
                }              

                disposedValue = true;
            }
        }


        public void Dispose()
        {          
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
