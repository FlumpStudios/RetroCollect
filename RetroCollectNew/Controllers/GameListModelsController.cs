using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ApplicationLayer.Models.Responses;
using System.Security.Claims;
using ApplicationLayer.Models.Requests;
using ApplicationLayer.Business_Logic;
using ModelData;
using DataAccess.WorkUnits;

namespace ApplicationLayer.Controllers
{
    
    public class GameListController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;

        private readonly ISortingManager _sortingManager;

        public GameListController(IUnitOFWork unitOFWork, ISortingManager sortingManager)            
        {
            _unitOFWork = unitOFWork;
            _sortingManager = sortingManager;
        }


        /// <summary>
        /// Sort and filted games from DB and return with view
        /// </summary>
        /// <param name="gameListRequestModel"></param>
        /// <returns></returns>
        #region views
        public IActionResult Index(GameListRequest gameListRequestModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gameList = _sortingManager.GetFilteredResults(gameListRequestModel);                                   

            if (gameListRequestModel.ShowClientList && !string.IsNullOrEmpty(userId))
            {               
                    gameList = QueryHelper.InnerJoin(gameList, _unitOFWork.ClientRepo.Get(), userId);
            }

            return View (new GameListResponse(gameList,
                User.Identity.IsAuthenticated,
                _unitOFWork.GameRepo.GetDistinct(x => x.Format),
                gameListRequestModel.Switchsort,
                User.IsInRole("Admin")));            
            }



        /// <summary>
        /// Get game by ID and return with Details view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();

            return View(gameListModel);
        }


        /// <summary>
        /// Return the create view
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Return the edit view with along with retrieved record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();
            return View(gameListModel);
        }


        /// <summary>
        /// Return the edit view with along with retrieved record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();

            return View(gameListModel);
        }
        #endregion



        /// <summary>
        /// Create new game in game list DB
        /// </summary>
        /// <param name="gameListModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Id,Name,Developer,Genre,Publisher,ReleaseDateNA,ReleaseDateEU,ReleaseDateJP")] GameListModel gameListModel)
        {
            if (ModelState.IsValid)
            {
                _unitOFWork.GameRepo.Insert(gameListModel);
                _unitOFWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(gameListModel);
        }

        /// <summary>
        /// Edit game in game list DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gameListModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, [Bind("Id,Name,Developer,Genre,Publisher,ReleaseDateNA,ReleaseDateEU,ReleaseDateJP")] GameListModel gameListModel)
        {
            if (id != gameListModel.Id) return NotFound();            

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOFWork.GameRepo.Update(gameListModel);
                    _unitOFWork.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameListModelExists(gameListModel.Id)) return NotFound();                    
                    else throw;                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameListModel);
        }


        /// <summary>
        /// Delete game in game list DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOFWork.GameRepo.Delete(id);
            _unitOFWork.Commit();
            
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Ensure game list model exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool GameListModelExists(int id) =>  _unitOFWork.GameRepo.Get().Any(e => e.Id == id);
    }
}
