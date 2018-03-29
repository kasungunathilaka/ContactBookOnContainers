using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrderService.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.API.Infrastructure
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }       
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
               .HasMany(a => a.Addresses)
               .WithOne(b => b.Customer)
               .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Customer>()
               .HasOne(a => a.ContactDetails)
               .WithOne(b => b.Customer)
               .HasForeignKey<ContactDetails>(b => b.CustomerId);

            modelBuilder.Entity<Customer>()
               .HasMany(a => a.Orders)
               .WithOne(b => b.Customer)
               .HasForeignKey(b => b.CustomerId);               

            modelBuilder.Entity<ProductCategory>()
               .HasMany(a => a.Product)
               .WithOne(b => b.ProductCategory)
               .HasForeignKey(b => b.ProductCategoryId);

            modelBuilder.Entity<Order>()
               .HasMany(a => a.OrderItems)
               .WithOne(b => b.Order)
               .HasForeignKey(b => b.OrderId);

            modelBuilder.Entity<OrderItem>()
               .HasOne(a => a.Product)
               .WithOne(b => b.OrderItem)
               .HasForeignKey<OrderItem>(b => b.ProductId);

            // Create shadow properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
                    {
                        modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedDate");
                        modelBuilder.Entity(entityType.ClrType).Property<DateTime>("ModifiedDate");
                    }
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        private void ApplyAuditInformation()
        {
            var modifiedEntities = ChangeTracker.Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entity in modifiedEntities)
            {
                entity.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    entity.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                }
            }
        }

        public class OrderContextDesignFactory : IDesignTimeDbContextFactory<OrderContext>
        {
            public OrderContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<OrderContext>()
                    .UseSqlServer("Server=tcp:10.0.75.1,5434;Initial Catalog=OrdersDb;User Id=sa;Password=Qweasd123");

                //var optionsBuilder = new DbContextOptionsBuilder<OrderContext>()
                  //  .UseSqlServer("Server=KASUNG-LAP\\SQLEXPRESS;Initial Catalog=OrdersDb;;Integrated Security=True");

                return new OrderContext(optionsBuilder.Options);
            }
        }
    }
}
