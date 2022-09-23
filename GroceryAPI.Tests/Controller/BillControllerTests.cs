using FluentAssertions;
using GroceryAPI.Controllers;
using GroceryAPI.Data;
using GroceryAPI.Models;
using GroceryAPI.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Tests.Controller
{
    public class BillControllerTests
    {
        [Fact]
        public async Task UpdateTypeId_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var billController = new BillController(dbContext);
            var customer = new Customer()
            {
                CustomerID = 11,
                CartTypeId = null
            };

            //Act
            var result = await billController.UpdateTypeId(customer);

            //Assert
            result.Value.CartTypeId.Should().Be(customer.CartTypeId);
        }

        [Fact]
        public async Task GetPayment_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var billController = new BillController(dbContext);
            var payment = new Payment()
            {
                CustomerID = 11,
                PaymentID = 10,
                CardNumber = 12346,
                TotalAmount = 10
            };

            //Act
            var result = await billController.GetPayment(payment.CustomerID);

            //Assert
            result.Should().BeOfType<ActionResult<List<Payment>>>();
            result.Value[0].PaymentID.Should().Be(payment.PaymentID);
            result.Value[0].CardNumber.Should().Be(payment.CardNumber);
            result.Value[0].TotalAmount.Should().Be(payment.TotalAmount);
        }

        [Fact]
        public async Task Payment_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var billController = new BillController(dbContext);
            var payment = new Payment()
            {
               
                CustomerID = 11,
                CardNumber = 12346,
                TotalAmount = 10
            };

            //Act
            var result = await billController.Payment(payment);
            var expected = dbContext.grocery.FindAsync(11).Result?.Stock;

            //Assert
            result.Should().BeOfType<ActionResult<Payment>>();
            result.Value.TotalAmount.Should().Be(10);
            expected.Should().Be(11);
        }

        [Fact]
        public async Task Order_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var billController = new BillController(dbContext);
            var receipt = new Receipt()
            {
                CustomerID= 11,
            };

            //Act
            var result = await billController.Order(receipt);
            
            //Assert
            result.Should().BeOfType<ActionResult<Receipt>>();
            dbContext.receipts.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetMyOrders_Test()
        {
            //Arrange
            var inMemory = new InMemoryDbContext();
            var dbContext = await inMemory.GetDatabaseContext();
            var billController = new BillController(dbContext);
            var receipt = new Receipt()
            {
                CustomerID = 11,
            };

            //Act
            await billController.Order(receipt);
            var result = await billController.GetMyOrders(11);
            

            //Assert
            result.Should().BeOfType<ActionResult<List<Receipt>>>();
            result.Value.Should().HaveCount(1);
        }
    }
}
