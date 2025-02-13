using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductExercise;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly DBContext _context;
        public ClientController(
            DBContext context
            )
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
    
            if (client.DateOfBirth >= DateTime.Now) return BadRequest("Veuillez renseigner une date de naissance valide.");

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.ClientId }, client);
        }


        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _context.Clients.Include(c => c.Addresses).ToListAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Invalid client ID.");

            var client = await _context.Clients.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null) return NotFound();

            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Invalid client ID.");

            var client = await _context.Clients.Include(c => c.Orders).FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null) return NotFound();

            if (client.Orders.Any()) return BadRequest("Client cannot be deleted because they have orders.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}