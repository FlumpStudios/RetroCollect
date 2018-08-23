using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RetroCollectNew.Models;
using Microsoft.AspNetCore.Authorization;
using RetroCollectNew.Models.DataModel;
using System.Security.Claims;
using RetroCollectNew.Data.Repositories;

namespace RetroCollectNew.Controllers
{
    [Authorize]
    public class ClientGamesListController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientGamesListController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }


        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion


        #region Actions
        [HttpPost]       
        public bool Create([Bind("GameId")] ClientListModel ClientListModel)
        {
            if (ModelState.IsValid)
            {
                ClientListModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _clientRepository.InsertClient(ClientListModel);
                _clientRepository.Save();
                return true;
            }            
            return false;
        }
        #endregion
    }
}