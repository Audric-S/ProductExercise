using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly DBContext _context;

    public OrderController(DBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] int id)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order orderDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var order = new Order
        {
            Date = orderDto.Date,
            ClientId = orderDto.ClientId,
            OrderProducts = orderDto.OrderProducts.Select(op => new OrderProduct
            {
                ProductId = op.ProductId,
                Quantity = op.Quantity
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] Order orderDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null) return NotFound();

        order.Date = orderDto.Date;
        order.ClientId = orderDto.ClientId;

        // Update OrderProducts
        order.OrderProducts.Clear();
        foreach (var op in orderDto.OrderProducts)
        {
            order.OrderProducts.Add(new OrderProduct
            {
                OrderId = order.OrderId,
                ProductId = op.ProductId,
                Quantity = op.Quantity
            });
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder([FromRoute] int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}