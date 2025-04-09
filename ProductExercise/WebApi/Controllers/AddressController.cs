using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly DBContext _context;
    private readonly ILogger<AddressController> logger;


    public AddressController(DBContext context, ILogger<AddressController> logger)
    {
        _context = context;
            this.logger = logger;
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
        if (address == null) return NotFound();
        return Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] Address addressDto)
    {

        _context.Addresses.Add(addressDto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAddress), new { id = addressDto.AddressId }, addressDto);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAddress(
        [FromRoute] int id,
        [FromBody] Address addressDto
    )
    {

        var address = await _context.Addresses.FindAsync(id);
        if (address == null) return NotFound();

        address.Street = addressDto.Street;
        address.ZipCode = addressDto.ZipCode;
        address.City = addressDto.City;
        address.Country = addressDto.Country;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress([FromRoute] int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null) return NotFound();

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}