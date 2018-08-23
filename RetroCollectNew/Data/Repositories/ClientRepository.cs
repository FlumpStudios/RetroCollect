using RetroCollectNew.Models;
using RetroCollectNew.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetroCollectNew.Models.DataModel;

namespace RetroCollectNew.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {

        private readonly RetroCollectNewContext _clientContext;

        public ClientRepository(RetroCollectNewContext clientContext)
        {
            _clientContext = clientContext;
        }

        public void DeleteClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public ClientListModel GetClientByID(int clientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClientListModel> GetClients()
        {
            throw new NotImplementedException();
        }

        public void InsertClient(ClientListModel client)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateClient(ClientListModel Client)
        {
            throw new NotImplementedException();
        }
    }
}
