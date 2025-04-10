using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Dto;

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
        public async Task<IActionResult> CreateClient([FromBody] ClientDto clientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (clientDto.DateOfBirth >= DateTime.Now)
                return BadRequest("Veuillez renseigner une date de naissance valide.");

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                DateOfBirth = clientDto.DateOfBirth
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.ClientId }, client);
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
                return NotFound("Client non trouvé.");

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
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
        {
            if (id <= 0)
                return BadRequest("ID invalide.");

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound("Client non trouvé.");

            if (clientDto.DateOfBirth >= DateTime.Now)
                return BadRequest("Date de naissance invalide.");

            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.DateOfBirth = clientDto.DateOfBirth;

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
                return NotFound("client non trouvé");

            if (client.Orders.Any())
                return BadRequest("Le client ne peut pas être supprimé car il a des commandes.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
