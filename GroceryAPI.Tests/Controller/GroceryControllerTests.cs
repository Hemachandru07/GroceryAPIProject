using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Tests.Controller
{
    public class GroceryControllerTests
    {
        private async Task<GroceryContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<GroceryContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var databaseContext = new GroceryContext(options);
            databaseContext.Database.EnsureCreated();
            int id = 1000;
            if (await databaseContext.grocery.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
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
        public async Task GetAllGrocery_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            

            //Act
            var result = await groceryController.GetAllGrocery();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<List<Grocery>>>();
            int actualresult = result.Value.Count();
            int expectedresult = 10;
            Assert.Equal(expectedresult, actualresult);

        }
        [Fact]
        public async Task GetGroceryById_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            Grocery grocery = new Grocery()
            {
                GroceryID = 1000,
                GroceryName = "Oil0" ,
                Price = 1001,
                Stock = 1001
            };

            //Act
            var result = await groceryController.GetGroceryById(1000);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Grocery>>();
            result.Value.Should().BeEquivalentTo(grocery);
        }

        [Fact]
        public async Task CreateGrocery_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            Grocery grocery = new Grocery()
            {
                GroceryID = 1011,
                GroceryName = "Oil0",
                Price = 1001,
                Stock = 1001
            };

            //Act
            var result = await groceryController.CreateGrocery(grocery);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Grocery>>();
            result.Value.Should().BeEquivalentTo(grocery);
            dbContext.grocery.Should().HaveCount(11);
        }
        [Fact]
        public async Task EditGrocery_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            var id = 1000;
            Grocery grocery = new Grocery()
            {
                GroceryID = id,
                GroceryName = "Oil11",
                Price = 1001,
                Stock = 1001
            };

            //Act
            var oldGrocery = await dbContext.grocery.FindAsync(id);
            dbContext.Entry<Grocery>(oldGrocery).State = EntityState.Detached;
            var result = await groceryController.EditGrocery(grocery);

            //Assert
            result.Value.Should().BeEquivalentTo(grocery);
            dbContext.grocery.Should().HaveCount(10);
        }
        [Fact]
        public async Task DeleteGrocery_Test()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            
            //Act
            var result = await groceryController.DeleteGrocery(1000);

            //Assert
            result.Should().BeNull();
            dbContext.grocery.Should().HaveCount(9);
        }
    }
}
