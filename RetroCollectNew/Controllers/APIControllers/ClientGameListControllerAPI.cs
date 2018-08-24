using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroCollectNew.Models;
using RetroCollectNew.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RetroCollectNew.APIcontrollers.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/ClientGameList")]
    public class ClientGameListControllerAPI : Controller
    {
        private readonly RetroCollectNewContext _context;

        public ClientGameListControllerAPI(RetroCollectNewContext context)
        {
            _context = context;
        }

        // GET: api/ClientGameList
        [HttpGet]
        public IEnumerable<ClientListModel> GetClientListModel()
        {
            return _context.ClientListModel;
        }

        // GET: api/ClientGameList/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientListModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientListModel = await _context.ClientListModel.SingleOrDefaultAsync(m => m.Id == id);

            if (clientListModel == null)
            {
                return NotFound();
            }

            return Ok(clientListModel);
        }

        // PUT: api/ClientGameList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientListModel([FromRoute] int id, [FromBody] ClientListModel clientListModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientListModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientListModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientListModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientGameList
        [HttpPost]
        public async Task<IActionResult> PostClientListModel([FromBody] ClientListModel clientListModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }          

            clientListModel.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.ClientListModel.Add(clientListModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientListModel", new { id = clientListModel.Id }, clientListModel);
        }

        // DELETE: api/ClientGameList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientListModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientListModel = await _context.ClientListModel.SingleOrDefaultAsync(m => m.Id == id);
            if (clientListModel == null)
            {
                return NotFound();
            }

            _context.ClientListModel.Remove(clientListModel);
            await _context.SaveChangesAsync();

            return Ok(clientListModel);
        }

        private bool ClientListModelExists(int id)
        {
            return _context.ClientListModel.Any(e => e.Id == id);
        }
    }
}