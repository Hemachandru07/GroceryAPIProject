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
    public class BillController : ControllerBase
    {
        private readonly GroceryContext _context;

        public BillController(GroceryContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("Payment")]
        public async Task<ActionResult<Payment>> Payment(Payment payment)
        {
            Customer customer = (from i in _context.customer where i.CustomerID == payment.CustomerID select i).SingleOrDefault();

            List<Cart> cart = (from i in _context.carts where i.CartTypeId == customer.CartTypeId select i).ToList();
            payment.TotalAmount = 0;
            foreach (Cart c in cart)
            {
                payment.TotalAmount += c.UnitPrice;
                Grocery g = (from i in _context.grocery where i.GroceryID == c.GroceryID select i).Single();
                g.Stock -= c.Quantity;
                await _context.SaveChangesAsync();
            }
            _context.payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        [HttpPut]
        [Route("UpdateTypeId")]
        public async Task<ActionResult<Customer>> UpdateTypeId(Customer c)
        {

            var update = await _context.customer.FindAsync(c.CustomerID);
            update.CartTypeId = null;
            await _context.SaveChangesAsync();
            return update;

        }

        [HttpGet]
        [Route("GetPayment")]
        public async Task<ActionResult<List<Payment>>> GetPayment(int cid)
        {
            try
            {
                var result = await (from i in _context.payments where i.CustomerID == cid select i).ToListAsync();

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Receipt>> Order(Receipt r)
        {
            List<Cart> cart = new List<Cart>();
            var customer = await (from i in _context.customer
                                  where i.CustomerID == r.CustomerID
                                  select i).SingleOrDefaultAsync();
            var c = await (from i in _context.carts
                           where i.CartTypeId == customer.CartTypeId
                           select i).ToListAsync();
            foreach (var item in c)
            {
                Receipt receipt = new Receipt();
                receipt.CustomerID = item.CustomerID;
                receipt.GroceryID = item.GroceryID;
                receipt.Quantity = item.Quantity;
                receipt.Amount = item.UnitPrice;
                receipt.ReceiptDate = DateTime.Now;
                _context.Add(receipt);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        [HttpGet]
        [Route("GetMyOrders"),Authorize]
        public async Task<ActionResult<List<Receipt>>> GetMyOrders(int cid)
        {
            try
            {
                var result = await _context.receipts.Include(x => x.grocery).Where(x => x.CustomerID == cid).ToListAsync();
                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
