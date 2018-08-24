using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RetroCollectNew.Models.DataModel;
using System.Security.Claims;
using RetroCollectNew.Data.Repositories;
using RetroCollectNew.Data.WorkUnits;

namespace RetroCollectNew.Controllers
{
    [Authorize]
    public class ClientGamesListController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;

        public ClientGamesListController(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;            
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
                _unitOFWork.ClientRepo.Insert(ClientListModel);
                _unitOFWork.Commit();
            
                return true;
            }            
            return false;
        }
        #endregion
    }
}