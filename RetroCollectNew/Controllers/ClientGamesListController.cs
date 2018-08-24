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


        #region Actions
        /// <summary>
        /// Create a new record in client DB
        /// </summary>
        /// <param name="ClientListModel"></param>
        /// <returns></returns>
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