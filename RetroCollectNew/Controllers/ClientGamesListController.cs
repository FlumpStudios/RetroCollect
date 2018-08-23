using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RetroCollectNew.Models;
using Microsoft.AspNetCore.Authorization;
using RetroCollectNew.Models.DataModel;
using System.Security.Claims;

namespace RetroCollectNew.Controllers
{
    [Authorize]
    public class ClientGamesListController : Controller
    {
        private readonly RetroCollectNewContext _context;

        public ClientGamesListController(RetroCollectNewContext context)
        {
            _context = context;
        }

        [HttpPost]       
        public async Task<bool> Create([Bind("GameId")] ClientListModel ClientListModel)
        {
            if (ModelState.IsValid)
            {
                ClientListModel.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(ClientListModel);
                await _context.SaveChangesAsync();
                return true;
            }
            //If model state invalid return false
            return false;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}