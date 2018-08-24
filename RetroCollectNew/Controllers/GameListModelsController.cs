using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroCollectNew.Models;
using RetroCollectNew.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using RetroCollectNew.Models.ViewModels;
using System.Security.Claims;
using RetroCollectNew.Models.Requests;
using System.Collections.Generic;
using RetroCollectNew.Data.Repositories;
using RetroCollectNew.Data.WorkUnits;

namespace RetroCollectNew.Controllers
{
    
    public class GameListModelsController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;

        public GameListModelsController(IUnitOFWork unitOFWork)            
        {
            _unitOFWork = unitOFWork;  
        }

        #region views
        public IActionResult Index(GameListRequestModel gameListRequestModel)
        {
            var gameList = _unitOFWork.GameRepo.Get();
            var clientList = _unitOFWork.ClientRepo.Get();

            if (gameListRequestModel.ShowClientList)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //Check client DB against Game DB to see which games to return.
                if (!string.IsNullOrEmpty(userId))
                {
                    gameList = (from s in gameList
                                join c in clientList on s.Id equals c.GameId
                                where c.GameId == s.Id && userId == c.UserId
                                select s).ToList();
                }

                //Get a list of available consoles in the game list for game options tabs

            }
            var availableConsoles = gameList.Select(m => m.Format).Distinct().ToList();
            
            //filters and searching
            //TODO: Move into seperate class
            if (!string.IsNullOrEmpty(gameListRequestModel.SearchText)) gameList = gameList.Where(y => y.Name.ToUpper().Contains(gameListRequestModel.SearchText.ToUpper())).ToList();
            if (!string.IsNullOrEmpty(gameListRequestModel.Format)) gameList = gameList.Where(t => t.Format.Equals(gameListRequestModel.Format)).ToList();

            //TODO: Sort this shit out!
            if (!string.IsNullOrEmpty(gameListRequestModel.SortingOptions))
            {
                if (gameListRequestModel.SortingOptions == "Name") gameList = gameListRequestModel.Switchsort ? gameList.OrderByDescending(h => h.Name).ToList() : gameList.OrderBy(h => h.Name).ToList();
                if (gameListRequestModel.SortingOptions == "Developer") gameList = gameList.OrderBy(h => h.Developer).ToList();
                if (gameListRequestModel.SortingOptions == "Genre") gameList = gameList.OrderBy(h => h.Genre).ToList();
            }


            ListView listView = new ListView()
            {
                GameListModel = gameList,
                IsAdmin = User.IsInRole("Admin"),
                IsLoggedIn = User.Identity.IsAuthenticated,
                ConsoleList = availableConsoles,
                ReversedList = gameListRequestModel.Switchsort
            };

            return View(listView);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();

            return View(gameListModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


       
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();
            return View(gameListModel);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var gameListModel = _unitOFWork.GameRepo.GetByID(id);
            if (gameListModel == null) return NotFound();

            return View(gameListModel);
        }
        #endregion


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
  

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOFWork.GameRepo.Delete(id);
            _unitOFWork.Commit();
            
            return RedirectToAction(nameof(Index));
        }

        private bool GameListModelExists(int id) =>  _unitOFWork.GameRepo.Get().Any(e => e.Id == id);
    }
}
