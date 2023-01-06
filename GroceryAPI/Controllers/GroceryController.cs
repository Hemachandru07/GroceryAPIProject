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
    public class GroceryController : ControllerBase
    {
        private readonly GroceryContext db;

        public GroceryController(GroceryContext db)
        {
            this.db = db;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Grocery>>> GetAllGrocery()
        {
            try
            {
                
                return await db.grocery.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grocery>> GetGroceryById(int id)
        {
            try
            {
                var result = await db.grocery.FindAsync(id);

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
       public async Task<ActionResult<Grocery>> CreateGrocery(Grocery grocery)
        {
            try
            {
                if (grocery == null)
                    return BadRequest();

                db.grocery.Add(grocery);
                await db.SaveChangesAsync();
                return grocery;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Grocery>> EditGrocery(Grocery grocery)
        {
            try
            {
                Thread.Sleep(10000);
                db.grocery.Update(grocery);
                await db.SaveChangesAsync();
                return grocery;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Grocery record");
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult<Grocery>> DeleteGrocery(int id)
        {
            try
            {
                var deleteGrocery = await db.grocery.FindAsync(id);

                if (deleteGrocery == null)
                {
                    return NotFound($"Grocery with Id = {id} not found");
                }

                db.grocery.Remove(deleteGrocery);
                await db.SaveChangesAsync();
                return null;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
