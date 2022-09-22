using FluentAssertions;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace GroceryAPI.Tests.Controller
{
    public class AccountControllerTests
    {
        private readonly IConfiguration _configuration;
        public AccountControllerTests(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private async Task<GroceryContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<GroceryContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var databaseContext = new GroceryContext(options);
            databaseContext.Database.EnsureCreated();
            int id = 10;
            int cid = 100;
            if (await databaseContext.customer.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.customer.Add(
                        new Customer()
                        {
                            CustomerID = cid++,
                            CustomerName = "Chandru" + i,
                            CustomerEmail = "Chandru" + i + "@gmail.com",
                            MobileNumber = 985632587 + i,
                            Address = "Chennai" + i,
                            CartTypeId = "null",
                            Password = "!Morning1" + i,
                            CPassword = "!Morning1" + i
                        });
                    databaseContext.admin.Add(
                        new Admin()
                        {
                            AdminID = id++,
                            AdminName = "Admin"+i,
                            EmailID = "admin"+i+"@gmail.com",
                            Password = "!Morning1"+i
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        //[Fact]
        public async Task AdminLogin_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var accountController = new AccountController(dbContext, _configuration);
            Admin admin = new Admin()
            {
                AdminID = 10,
                AdminName = "Admin0",
                EmailID = "admin0@gmail.com",
                Password = "!Morning10"
            };

            //Act
            var result = await accountController.AdminLogin(admin);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Admin>>();
            result.Value.Should().BeEquivalentTo(admin);
            
        }
    }
}
