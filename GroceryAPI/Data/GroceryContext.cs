using GroceryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Data
{
    public class GroceryContext : DbContext
    {
        public GroceryContext() { }
        public GroceryContext(DbContextOptions options) : base(options) { }

        public DbSet<Admin> admin { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Grocery> grocery { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Receipt> receipts { get; set; }
    }
}
