using FluentAssertions;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Tests.Controller
{
    public class OrderControllerTests
    {
        private async Task<GroceryContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<GroceryContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var databaseContext = new GroceryContext(options);
            databaseContext.Database.EnsureCreated();
            int id = 10;
            if (await databaseContext.carts.CountAsync() <= 0)
            {
                for (int i = 1; i < 5; i++)
                {
                    databaseContext.carts.Add(
                        new Cart()
                        {
                            CartID = id++,
                            CartTypeId = "77777"+i,
                            GroceryID = i,
                            CustomerID = id,
                            Quantity = i,
                            UnitPrice = i*10
                        });
                    databaseContext.customer.Add(
                       new Customer()
                       {
                           CustomerID = id,
                           CustomerName = "Chandru" + i,
                           CustomerEmail = "Chandru" + i + "@gmail.com",
                           MobileNumber = 985632587 + i,
                           Address = "Chennai" + i,
                           CartTypeId = "77777" + i,
                           Password = "!Morning1" + i,
                       });
                    databaseContext.grocery.Add(
                        new Grocery()
                        {
                            GroceryID = id++,
                            GroceryName = "Oil" + i,
                            Price = id,
                            Stock = id

                        });
                   
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async Task GetCartById_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            Cart cart = new Cart()
            {
                CartID = 10,
                CartTypeId= "777771",
                GroceryID = 1,
                CustomerID = 11,
                Quantity = 1,
                UnitPrice = 10
            };

            //Act
            var result = await orderController.GetCartById(10);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Cart>();
            //result.Should().BeEquivalentTo(cart);
        }

        [Fact]
        public async Task DeleteGrocery_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);

            //Act
            var result = await orderController.DeleteCart(10);

            //Assert
            result.Should().BeNull();
            dbContext.carts.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetCart_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            //List<Cart> result = new List<Cart>();

            //Act
            var result = await orderController.GetCart(11);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Cart>>();
        }
    }
}
