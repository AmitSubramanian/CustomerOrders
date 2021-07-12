using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerOrdersService.Models
{
    public class CustomerOrderContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public CustomerOrderContext()
        { }

        public CustomerOrderContext(DbContextOptions<CustomerOrderContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=customerorders.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Customer Seed
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerId = 1, Name = "John Doe", Email = "john.doe@a.com" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerId = 2, Name = "Isaac Newton", Email = "isaac.newton@b.com" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerId = 3, Name = "Lady Ada", Email = "lady.ada@c.com" });
            #endregion

            #region Order Seed
            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 1, CustomerId = 1, Price = 500.21M, CreatedDate = new DateTime(2019, 6, 9) });
            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 2, CustomerId = 1, Price = 1000.32M, CreatedDate = new DateTime(2019, 6, 9) });
            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 3, CustomerId = 1, Price = 800.65M, CreatedDate = new DateTime(2019, 6, 9) });

            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 4, CustomerId = 2, Price = 100.43M, CreatedDate = new DateTime(2019, 6, 10) });
            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 5, CustomerId = 2, Price = 300.56M, CreatedDate = new DateTime(2019, 6, 10) });
            #endregion
        }
    }
}
