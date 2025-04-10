using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Dto;
using System.Linq;

namespace WebApi.Controllers
{
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
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                Date = o.Date,
                ClientId = o.ClientId,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailsDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
                
            if (order == null) return NotFound("Commande non trouvée.");

            var orderDto = new OrderDto
            {
                Date = order.Date,
                ClientId = order.ClientId,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailsDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = new Order
            {
                Date = orderDto.Date,
                ClientId = orderDto.ClientId,
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetails
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, new
            {
                order.Date,
                order.ClientId,
                OrderDetails = order.OrderDetails.Select(od => new
                {
                    od.ProductId,
                    od.Quantity
                })
            });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound("Commande non trouvée.");

            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == orderDto.ClientId);
            if (!clientExists)
            {
                return BadRequest($"Client avec l'ID {orderDto.ClientId} n'existe pas.");
            }

            var invalidProductIds = new List<int>();
            foreach (var odDto in orderDto.OrderDetails)
            {
                var productExists = await _context.Products.AnyAsync(p => p.ProductId == odDto.ProductId);
                if (!productExists)
                {
                    invalidProductIds.Add(odDto.ProductId);
                }
            }

            if (invalidProductIds.Any())
            {
                return BadRequest($"Les produits suivants n'existent pas : {string.Join(", ", invalidProductIds)}");
            }

            order.Date = orderDto.Date;
            order.ClientId = orderDto.ClientId;

            order.OrderDetails.Clear();
            foreach (var odDto in orderDto.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetails
                {
                    ProductId = odDto.ProductId,
                    Quantity = odDto.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return Ok(orderDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound("Commande non trouvée.");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
