using Castle.Core.Resource;
using FluentAssertions;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Helper;
using GroceryAPI.Models;
using GroceryAPI.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace GroceryAPI.Tests.Controller
{
    public class AccountControllerTests
    {
        
        private readonly IConfiguration _configuration;

        
        
        [Fact]
        public async Task AdminLogin_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var accountController = new AccountController(dbContext, _configuration);
            Admin admin = new Admin()
            {
                AdminID = 12,
                AdminName = "Admin1",
                EmailID = "admin1@gmail.com",
                Password = "!Morning11"
            };

            //Act
            var result = await accountController.AdminLogin(admin);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Admin>>();
            result.Value.Should().BeEquivalentTo(admin);
            
        }

        [Fact]
        public async Task Register_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var accountController = new AccountController(dbContext, _configuration);
            Customer customer = new Customer()
                        {
                            //CustomerID = 15,
                            CustomerName = "Chandru",
                            CustomerEmail = "Chandru@gmail.com",
                            MobileNumber = 985632587,
                            Address = "Chennai",
                            CartTypeId = null,
                            Password = "!Morning1",
                        };
           
            //Act
            var result = await accountController.Register(customer);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Customer>>();
            //result.Value.Should().BeEquivalentTo(customer);
            dbContext.customer.Should().HaveCount(5);
        }

        //[Fact]
        public async Task CustomerLogin_Test()
        {

            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var accountController = new AccountController(dbContext, _configuration);
            Customer customer = new Customer()
            {
                //CustomerID = 15,
                CustomerName = "Chandru",
                CustomerEmail = "Chandru@gmail.com",
                MobileNumber = 985632587,
                Address = "Chennai",
                CartTypeId = null,
                Password = "!Morning1",
            };
            Customer customer1 = new Customer()
            {
                CustomerID = 15,
                CustomerName = "Chandru",
                CustomerEmail = "Chandru@gmail.com",
                MobileNumber = 985632587,
                Address = "Chennai",
                CartTypeId = null,
                Password = "!Morning1",
            };


            //Act
            await accountController.Register(customer);
            var result = await accountController.CustomerLogin(customer1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<JWTToken>>();
            //result.Value.Should().BeEquivalentTo(customer);
            dbContext.customer.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetAllCustomer_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var accountController = new AccountController(dbContext, _configuration);

            //Act
            var result = await accountController.GetAllCustomer();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<List<Customer>>>();
            result.Value.Should().HaveCount(4);
        }
    }
}
