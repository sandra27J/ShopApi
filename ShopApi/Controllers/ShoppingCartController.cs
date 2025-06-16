using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Models;

namespace ShopApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ShoppingCartController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetCart()
        {
            var userEmail = User.Identity.Name;
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.User == userEmail);

            if (cart == null)
            {
                return Ok(new List<Product>());
            }

            return Ok(cart.Products);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userEmail = User.Identity.Name;
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.User == userEmail);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    User = userEmail,
                    Products = new List<Product> { product }
                };
                _context.ShoppingCarts.Add(cart);
            }
            else
            {
                if (!cart.Products.Any(p => p.Id == productId))
                {
                    cart.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userEmail = User.Identity.Name;
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.User == userEmail);

            if (cart == null)
            {
                return NotFound("Shopping cart not found");
            }

            var product = cart.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("Product not found in cart");
            }

            cart.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}