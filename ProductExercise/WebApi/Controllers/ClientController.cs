using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MySqlX.XDevAPI;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly DBContext _context;

        public ClientController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Models.Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (client.DateOfBirth >= DateTime.Now)
                return BadRequest("Veuillez renseigner une date de naissance valide.");

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok(client);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            if (id <= 0)
                return BadRequest("ID invalide.");

            var client = await _context.Clients
                .Include(c => c.Addresses)
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.Clients.Include(c => c.Addresses).ToListAsync();
            return Ok(clients);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClients([FromQuery] string? name, [FromQuery] string? country)
        {
            var query = _context.Clients.Include(c => c.Addresses).AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name));

            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(c => c.Addresses.Any(a => a.Country.Contains(country)));

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Models.Client updatedClient)
        {
            if (id != updatedClient.ClientId)
                return BadRequest("L'identifiant ne correspond pas.");

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            if (updatedClient.DateOfBirth >= DateTime.Now)
                return BadRequest("Date de naissance invalide.");

            client.FirstName = updatedClient.FirstName;
            client.LastName = updatedClient.LastName;
            client.DateOfBirth = updatedClient.DateOfBirth;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id <= 0)
                return BadRequest("ID invalide.");

            var client = await _context.Clients.Include(c => c.Orders).FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null)
                return NotFound();

            if (client.Orders.Any())
                return BadRequest("Le client ne peut pas être supprimé car il a des commandes.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}