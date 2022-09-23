using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Models;
using GroceryAPI.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Tests.Controller
{
    public class GroceryControllerTests
    {
        [Fact]
        public async Task GetAllGrocery_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);

            //Act
            var result = await groceryController.GetAllGrocery();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<List<Grocery>>>();
            result.Value.Should().HaveCount(4);
            
        }
        [Fact]
        public async Task GetGroceryById_ReturnGrocery()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            Grocery grocery = new Grocery()
            {
                GroceryID = 11,
                GroceryName = "Oil1" ,
                Price = 10,
                Stock = 12
            };

            //Act
            var result = await groceryController.GetGroceryById(11);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Grocery>>();
            
        }

        [Fact]
        public async Task GetGroceryById_ReturnNull()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);

            //Act
            var result = await groceryController.GetGroceryById(20);

            //Assert
            result.Value.Should().BeNull();
            result.Should().BeOfType<ActionResult<Grocery>>();

        }

        [Fact]
        public async Task CreateGrocery_GroceryAdded()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            Grocery grocery = new Grocery()
            {
                GroceryID = 18,
                GroceryName = "Oil5",
                Price = 50,
                Stock = 19
            };

            //Act
            var result = await groceryController.CreateGrocery(grocery);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Grocery>>();
            result.Value.Should().BeEquivalentTo(grocery);
            dbContext.grocery.Should().HaveCount(5);
        }

        [Fact]
        public async Task EditGrocery_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            var id = 11;
            Grocery grocery = new Grocery()
            {
                GroceryID = id,
                GroceryName = "Oil11",
                Price = 60,
                Stock = 13
            };

            //Act
            var oldGrocery = await dbContext.grocery.FindAsync(id);
            dbContext.Entry<Grocery>(oldGrocery).State = EntityState.Detached;
            var result = await groceryController.EditGrocery(grocery);

            //Assert
            result.Value.Should().BeEquivalentTo(grocery);
            dbContext.grocery.Should().HaveCount(4);
        }

        [Fact]
        public async Task DeleteGrocery_DeletedSuccessfully()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);
            
            //Act
            var result = await groceryController.DeleteGrocery(11);

            //Assert
            result.Should().BeNull();
            dbContext.grocery.Should().HaveCount(3);
        }

        [Fact]
        public async Task DeleteGrocery_NotDeleted()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var groceryController = new GroceryController(dbContext);

            //Act
            var result = await groceryController.DeleteGrocery(20);

            //Assert
            result.Result.Should().NotBeNull();
            dbContext.grocery.Should().HaveCount(4);
        }
    }
}
