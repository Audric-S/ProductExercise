using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly DBContext _context;

        public AddressController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _context.Addresses.ToListAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress([FromRoute] int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound("Adresse non trouvée.");
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDto addressDto)
        {
            var client = await _context.Clients.FindAsync(addressDto.ClientId);
            if (client == null)
            {
                return BadRequest("Le ClientId spécifié n'existe pas.");
            }

            var address = new Address
            {
                Street = addressDto.Street,
                ZipCode = addressDto.ZipCode,
                City = addressDto.City,
                Country = addressDto.Country,
                ClientId = addressDto.ClientId
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAddress), new { id = address.AddressId }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] int id, [FromBody] AddressDto addressDto)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound("Adresse non trouvée.");

            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == addressDto.ClientId);
            if (!clientExists)
                return BadRequest("Le ClientId spécifié n'existe pas.");

            address.Street = addressDto.Street;
            address.ZipCode = addressDto.ZipCode;
            address.City = addressDto.City;
            address.Country = addressDto.Country;
            address.ClientId = addressDto.ClientId;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound("Adresse non trouvée.");

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("gestion")]
        public IActionResult GetAddressPage()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "address.html");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Le fichier HTML n'existe pas.");
            }

            return PhysicalFile(filePath, "text/html");
        }
    }
}
