using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DBContext _context;

        public ProductController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Produit introuvable.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productDto.Price <= 0)
                return BadRequest("Le prix du produit doit être supérieur à 0.");

            if (productDto.UnitInStock <= 0)
                return BadRequest("La quantité en stock doit être supérieure à 0.");

            if (productDto.Weight <= 0)
                return BadRequest("Le poids doit être supérieure à 0.");

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                UnitInStock = productDto.UnitInStock,
                Weight = productDto.Weight
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productDto.Price <= 0)
                return BadRequest("Le prix du produit doit être supérieur à 0.");

            if (productDto.UnitInStock <= 0)
                return BadRequest("La quantité en stock doit être supérieure à 0.");
            
            if (productDto.Weight <= 0)
                return BadRequest("Le poids doit être supérieure à 0.");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Produit introuvable");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.UnitInStock = productDto.UnitInStock;
            product.Weight = productDto.Weight;

            await _context.SaveChangesAsync();
            return Ok(productDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Produit introuvable");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
