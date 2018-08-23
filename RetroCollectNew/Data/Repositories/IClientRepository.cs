using RetroCollectNew.Models.DataModel;
using System.Collections.Generic;


namespace RetroCollectNew.Data.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<ClientListModel> GetClients();
        ClientListModel GetClientByID(int clientId);
        void InsertClient(ClientListModel client);
        void DeleteClient(int clientId);
        void UpdateClient(ClientListModel Client);
        void Save();
    }
}
