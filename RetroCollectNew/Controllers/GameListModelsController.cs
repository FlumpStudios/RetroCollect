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
using System.Linq.Expressions;
using System;
using RetroCollectNew.Business_Logic;

namespace RetroCollectNew.Controllers
{
    
    public class GameListModelsController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;

        private readonly ISortingManager _sortingManager;

        public GameListModelsController(IUnitOFWork unitOFWork, ISortingManager sortingManager)            
        {
            _unitOFWork = unitOFWork;
            _sortingManager = sortingManager;


        }

        #region views
        public IActionResult Index(GameListRequestModel gameListRequestModel)
        {
            var gameList = _sortingManager.GetFilteredResults(gameListRequestModel); 
            var availableConsoles = _unitOFWork.GameRepo.GetDistinct(x => x.Format);           
        

            if (gameListRequestModel.ShowClientList)
            {
                var clientList = _unitOFWork.ClientRepo.Get();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Check client DB against Game DB to see which games to return.
                if (!string.IsNullOrEmpty(userId))
                {
                    gameList = (from s in gameList
                                join c in clientList on s.Id equals c.GameId
                                where c.GameId == s.Id && userId == c.UserId
                                select s).ToList();
                }
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

        private Func<IQueryable<GameListModel>, IOrderedQueryable<GameListModel>> SelectOrderBy(string sortOption, bool decend = false)
        {
            //TODO: Replace string compare with enum
            switch (sortOption)
            {
                case "Name":
                    if (decend) return x => x.OrderByDescending(y => y.Name);
                    return x => x.OrderBy(y => y.Name);
                case "Developer":
                    if (decend) return x => x.OrderByDescending(y => y.Developer);
                    return x => x.OrderBy(y => y.Developer);
                case "Genre":
                    if (decend) return x => x.OrderByDescending(y => y.Genre);
                    return x => x.OrderBy(y => y.Genre);
                default:
                    return x => x.OrderBy(y => y.Name);
            }
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
