using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ModelData;
using DataAccess.WorkUnits;
using ApplicationLayer.Business_Logic.Builders;
using Microsoft.AspNetCore.Hosting;
using Common.Enumerations;
using ApplicationLayer.Business_Logic.FileHandling;
using HttpAccess;
using ModelData.Request;
using ModelData.Responses;

namespace ApplicationLayer.Controllers
{    
    public class GameListController : Controller
    {
        private readonly IUnitOFWork _unitOFWork;     

        private readonly IGameListResponseBuilder _gameListResponseBuilder;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IFileHandler _fileHandler;

        public GameListController(IUnitOFWork unitOFWork, 
            IGameListResponseBuilder gameListResponseBuilder, 
            IHostingEnvironment hostingEnvironment,
            IFileHandler fileHandler)            
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOFWork = unitOFWork;        
            _gameListResponseBuilder = gameListResponseBuilder;
            _fileHandler = fileHandler;
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
            HttpManager goo = new HttpManager();
            goo.GetAll();

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
            gameListModel.ScreenShotURL = _fileHandler.LoadFiles(id.ToString());

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

            gameListModel.ScreenShotURL = _fileHandler.LoadFiles(id.ToString());
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
        public IActionResult Create([Bind("Id,Name,Developer,Genre,Publisher,ReleaseDateNA,ReleaseDateEU,ReleaseDateJP,Format, ScreenShot")] GameListModel gameListModel)
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
        public IActionResult Edit(int id, [Bind("Id,Name,Developer,Genre,Publisher,ReleaseDateNA,ReleaseDateEU,ReleaseDateJP, Format, ScreenShot")] GameListModel gameListModel)
        {
            //Save screenshots
            if (gameListModel.ScreenShot != null && gameListModel.ScreenShot.Count() > 0)
            { 
                _fileHandler.SaveFile(id.ToString(), gameListModel.ScreenShot);
            }

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
            }
            return RedirectToAction(nameof(Edit) + "/" + id);
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
        /// Delete file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]        
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(string fileLocation, int id)
        {
            switch (_fileHandler.DeleteFile(fileLocation))
            {
                case FileResponse.FileNotFound:
                    return StatusCode(400, "Could not find file to delete");
                case FileResponse.Exception:
                    return StatusCode(500, "Could not delete file, please see logs for details.");            
            }
            return RedirectToAction(nameof(Edit) + "/" + id );
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
