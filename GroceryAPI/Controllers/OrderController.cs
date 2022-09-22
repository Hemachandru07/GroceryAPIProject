using GroceryAPI.Data;
using GroceryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GroceryContext _context;

        public OrderController(GroceryContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetCartById")]
        public async Task<Cart> GetCartById(int id)
        {
            var result = await _context.carts.Include(x => x.grocery).Where(x => x.CartID == id).Select(x => x).FirstOrDefaultAsync();
            //var result await _context.carts.Find
            //var result = await (from i in _context.carts.Include(x => x.grocery) where i.CartID == id select i).FirstOrDefaultAsync();
            return result;
        }
       
        [HttpGet("{id}")]
        public async Task<List<Cart>> GetCart(int id)
        {
            List<Cart> cart = new List<Cart>();
            var Id = await _context.customer.Where(x => x.CustomerID == id).Select(x => x.CartTypeId).FirstOrDefaultAsync();
            cart = await _context.carts.Include(x => x.grocery).Where(x => x.CartTypeId == Id).ToListAsync();
            return cart;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Cart>> PutCart(int id, Cart cart)
        {
            var c = await _context.carts.FindAsync(cart.CartID);
            c.Quantity = cart.Quantity;
            c.UnitPrice = c.Quantity * (from i in _context.grocery where i.GroceryID == c.GroceryID select i.Price).SingleOrDefault();
            _context.carts.Update(c);
            await _context.SaveChangesAsync();
            return c;
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var c = await _context.carts.FindAsync(id);
            _context.carts.Remove(c);
            await _context.SaveChangesAsync();
            return null;
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<ActionResult<Cart>> AddToCart(Cart cart)
        {
            var id = await (from i in _context.customer where i.CustomerID == cart.CustomerID select i.CartTypeId).FirstOrDefaultAsync();
            var check = await (from i in _context.carts where i.CartTypeId == id && i.GroceryID == cart.GroceryID select i).SingleOrDefaultAsync();
            if(check == null)
            {
                var customer = (from i in _context.customer where i.CustomerID == cart.CustomerID select i).SingleOrDefault();
                cart.CartTypeId = customer.CartTypeId;
                if (cart.CartTypeId == null)
                {
                    Guid obj = Guid.NewGuid();
                    cart.CartTypeId = obj.ToString();
                    customer.CartTypeId = obj.ToString();
                }
                cart.UnitPrice = cart.Quantity * (from i in _context.grocery where i.GroceryID == cart.GroceryID select i.Price).SingleOrDefault();
            }
            else
            {
                check.Quantity += cart.Quantity;
                check.UnitPrice = check.Quantity * (from i in _context.grocery where i.GroceryID == cart.GroceryID select i.Price).SingleOrDefault();
                await _context.SaveChangesAsync();
                return check;

            }
            _context.carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;

        }

    }
}

