using System;
using System.Collections.Generic;
using System.Linq;
using ModelData;
using DataAccess.Repositories;

namespace DataAccess.EntityFramework.Repositories
{
    public class ClientRepository : IClientRepository, IDisposable
    {

        private readonly RetroCollectNewContext _clientContext;

        public ClientRepository(RetroCollectNewContext clientContext)
        {
            _clientContext = clientContext;
        }

        public IEnumerable<ClientListModel> GetClients() => _clientContext.ClientListModel.ToList();
        public ClientListModel GetClientByID(int? gameId) => _clientContext.ClientListModel.SingleOrDefault(m => m.ClientId == gameId);
        public void InsertClient(ClientListModel game) => _clientContext.Add(game);
        public void UpdateClient(ClientListModel game) => _clientContext.Update(game);
        public void DeleteClient(ClientListModel game) => _clientContext.ClientListModel.Remove(game);
        public void Save() => _clientContext.SaveChanges();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _clientContext.Dispose();
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
