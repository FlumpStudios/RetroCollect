using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ModelData;
using DataAccess.WorkUnits;
using System;
using System.Linq;

namespace ApplicationLayer.Controllers
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
        public ActionResult Create([Bind("GameId")] ClientListModel ClientListModel)
        {
            if (ModelState.IsValid)
            {
                ClientListModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bool alreadyExists = _unitOFWork.ClientRepo.Get(filter: x => x.GameId == ClientListModel.GameId && x.UserId == ClientListModel.UserId).Any();

                if (alreadyExists) return StatusCode(403, "Game Already Exists in Database");

                _unitOFWork.ClientRepo.Insert(ClientListModel);
                _unitOFWork.Commit();
            
                return StatusCode(200, "Game successfully added to database");
            }

            string modalStateErrors = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            return StatusCode(500, modalStateErrors);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int recordId = _unitOFWork.ClientRepo.Get(filter: x => x.GameId == id && x.UserId == userId).FirstOrDefault().Id;

            try
            {
                _unitOFWork.ClientRepo.Delete(recordId);
                _unitOFWork.Commit();
            }
            catch (Exception e)
            {
                //TODO: Add logging
                return StatusCode(403, string.Format("Could not delete record with id {0}. Exception : {1}",id,e.ToString()));
            }
            return StatusCode(200, string.Format("String with id of {0} successfully deleted", id));
        }
        #endregion
    }
}