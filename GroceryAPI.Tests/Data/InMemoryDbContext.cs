using GroceryAPI.Data;
using GroceryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Tests.Data
{
    public class InMemoryDbContext
    {
        public async Task<GroceryContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<GroceryContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var databaseContext = new GroceryContext(options);
            databaseContext.Database.EnsureCreated();
            int id = 10;
            for (int i = 1; i < 5; i++)
            {
                databaseContext.payments.Add(
                    new Payment()
                    {
                        PaymentID = id++,
                        CustomerID = id,
                        CardNumber = 12345 + i,
                        TotalAmount = i * 10
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
                databaseContext.carts.Add(
                       new Cart()
                       {
                           CartID = id,
                           CartTypeId = "77777" + i,
                           GroceryID = id,
                           CustomerID = id,
                           Quantity = 1,
                           UnitPrice = i * 10
                       });
                databaseContext.grocery.Add(
                    new Grocery()
                    {
                        GroceryID = id++,
                        GroceryName = "Oil" + i,
                        Price = i * 10,
                        Stock = id

                    });
                databaseContext.admin.Add(
                        new Admin()
                        {
                            AdminID = id,
                            AdminName = "Admin" + i,
                            EmailID = "admin" + i + "@gmail.com",
                            Password = "!Morning1" + i
                        });

                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }
    }
}
