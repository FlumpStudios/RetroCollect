using DataAccess.EntityFramework.Repositories;
using DataAccess.Repositories;
using DataAccess.WorkUnits;
using Microsoft.EntityFrameworkCore;
using ModelData;
using System;
using System.Linq;


namespace ApplicationLayer.ApplicationData.WorkUnits
{
    public class UnitOFWork : IDisposable, IUnitOFWork
    {

        private readonly RetroCollectNewContext _dbContext;

        #region Repositories
        public IGenericRepository<ClientListModel> ClientRepo =>
           new GenericRepository<ClientListModel>(_dbContext);

        public IGenericRepository<GameListModel> GameRepo =>
           new GenericRepository<GameListModel>(_dbContext);
        #endregion


        public UnitOFWork(RetroCollectNewContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
