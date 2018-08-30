using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ApplicationLayer.Models.Responses;
using System.Security.Claims;
using ApplicationLayer.Models.Request;
using ApplicationLayer.Business_Logic.Sorting;
using ModelData;
using DataAccess.WorkUnits;
using ApplicationLayer.Helpers;
using X.PagedList;
using Microsoft.Extensions.Configuration;
using ApplicationLayer.Business_Logic.Builders;

namespace ApplicationLayer.Controllers
{
    
    public class GameListController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;     

        private readonly IGameListResponseBuilder _gameListResponseBuilder;

        public GameListController(IUnitOFWork unitOFWork, IGameListResponseBuilder gameListResponseBuilder)            
        {
            _unitOFWork = unitOFWork;        
            _gameListResponseBuilder = gameListResponseBuilder;
        }

        /// <summary>
        /// Sort and filted games from DB and return with view
        /// </summary>
        /// <param name="gameListRequestModel"></param>
        /// <returns></returns>
        #region Get Request
        [HttpGet]
        public IActionResult Index(GameListRequest gameListRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GameListResponse response = _gameListResponseBuilder.GetResponse(gameListRequestModel, User);

            return View (response);
        }


        /// <summary>
        /// Get game by ID and return with Details view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();

            return View(gameListModel);
        }
        #endregion


        #region Post request
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
    #endregion
}
