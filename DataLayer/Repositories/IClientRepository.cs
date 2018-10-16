using ModelData;
using System.Collections.Generic;


namespace DataAccess.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<ClientListModel> GetClients();
        ClientListModel GetClientByID(int? clientId);
        void InsertClient(ClientListModel client);
        void DeleteClient(ClientListModel clientId);
        void UpdateClient(ClientListModel Client);
        void Save();

    }
}
