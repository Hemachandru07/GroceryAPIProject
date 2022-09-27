using GroceryAPI.Data;
using GroceryAPI.Helper;
using GroceryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroceryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly GroceryContext db;
        private readonly IConfiguration _configuration;
        public AccountController(GroceryContext _db, IConfiguration configuration)
        {
            db = _db;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomer()
        {
            try
            {
                return await db.customer.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Customer>> Register([FromBody] Customer customer)
        {
            try
            {
                var HashedPassword = PasswordHash.GetMd5Hash(customer.Password);
                customer.Password = HashedPassword;
                
                if (customer == null)
                    return BadRequest();

                await db.customer.AddAsync(customer);
                await db.SaveChangesAsync();
                return customer;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<JWTToken>> CustomerLogin(Customer customer)
        {
            JWTToken jwt = new JWTToken();
            var HashedPassword = PasswordHash.GetMd5Hash(customer.Password);
            customer.Password = HashedPassword;
            var result = await (from i in db.customer where i.Password == customer.Password && i.CustomerEmail == customer.CustomerEmail select i).SingleOrDefaultAsync();
            if (result != null)
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.CustomerEmail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


                var token = GetToken(authClaims);
                String s = new JwtSecurityTokenHandler().WriteToken(token);
                jwt.customer = result;
                jwt.Token = s;
                return jwt;
            }
            else
            {
                return null;
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<ActionResult<Admin>> AdminLogin(Admin admin)
        {
            try
            {
                var adminLogin = (from i in db.admin
                                  where i.EmailID == admin.EmailID && i.Password == admin.Password
                                  select i).SingleOrDefault();
                if(adminLogin == null)
                {
                    return NotFound("No Admin Found");
                }
                return adminLogin;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Wrong Entry");
            }
        }
    }
}
