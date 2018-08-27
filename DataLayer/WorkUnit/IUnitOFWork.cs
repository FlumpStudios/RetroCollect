using DataAccess.Repositories;
using ModelData;

namespace DataAccess.WorkUnits
{
    public interface IUnitOFWork
    {        
        IGenericRepository<ClientListModel> ClientRepo { get; }
        IGenericRepository<GameListModel> GameRepo { get; }

        void Commit();
        void RejectChanges();
        void Dispose();
    }
}