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
                for (int i = 1; i < 5; i++)
                {
                    databaseContext.carts.Add(
                        new Cart()
                        {
                            CartID = id++,
                            CartTypeId = "77777"+i,
                            GroceryID = id,
                            CustomerID = id,
                            Quantity = 1,
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
                            Price = i*10,
                            Stock = id

                        });
                   
                    await databaseContext.SaveChangesAsync();
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

        [Fact]
        public async Task PutCart_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            var id = 10;
            Cart cart = new Cart()
            {
                CartID = id,
                CartTypeId = "777771",
                GroceryID = 11,
                CustomerID = 11,
                Quantity = 2,
                UnitPrice = 20
            };

            //Act
            var updateCart = await dbContext.carts.FindAsync(id);
            dbContext.Entry<Cart>(updateCart).State = EntityState.Detached;
            var result = await orderController.PutCart(id,cart);

            //Assert
            //result.Value.Should().BeEquivalentTo(cart);
            dbContext.carts.Should().HaveCount(4);
            result.Value.UnitPrice.Should().Be(cart.UnitPrice);
            result.Value.Quantity.Should().Be(cart.Quantity);
        }

        [Fact]
        public async Task AddToCart_UpdateQuantity()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            Cart cart = new Cart()
            {
                GroceryID = 11,
                CustomerID = 11,
                Quantity = 2,
            };

            //Act
            var result = await orderController.AddToCart(cart);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Cart>>();
        }

        [Fact]
        public async Task AddToCart_ExistingCustomer_NewGrocery()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            Cart cart = new Cart()
            {
                GroceryID = 13,
                CustomerID = 11,
                Quantity = 2,
                UnitPrice = 40
            };

            //Act
            var result = await orderController.AddToCart(cart);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Cart>>();
            result.Value.UnitPrice.Should().Be(cart.UnitPrice);
            dbContext.carts.Should().HaveCount(5);
        }

        [Fact]
        public async Task AddToCart_NewCustomer()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);
            Cart cart = new Cart()
            {
                GroceryID = 13,
                CustomerID = 18,
                Quantity = 2,
                UnitPrice = 40
            };
            dbContext.customer.Add(
                       new Customer()
                       {
                           CustomerID = 18,
                           CustomerName = "Chandru",
                           CustomerEmail = "Chandru@gmail.com",
                           MobileNumber = 985632587,
                           Address = "Chennai",
                           CartTypeId = null,
                           Password = "!Morning1",

                       });
            await dbContext.SaveChangesAsync();

            //Act
            var result = await orderController.AddToCart(cart);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Cart>>();
            result.Value.UnitPrice.Should().Be(cart.UnitPrice);
            dbContext.carts.Should().HaveCount(5);
        }
    }
}
