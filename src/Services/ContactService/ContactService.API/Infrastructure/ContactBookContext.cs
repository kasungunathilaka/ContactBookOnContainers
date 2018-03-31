using ContactService.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.Infrastructure
{
    public class ContactBookContext : DbContext
    {
        public ContactBookContext(DbContextOptions<ContactBookContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(a => a.ContactDetails)
                .WithOne(b => b.Customer)
                .HasForeignKey<ContactDetails>(b => b.CustomerId);

            modelBuilder.Entity<ContactDetails>()
                .HasOne(a => a.Address)
                .WithOne(b => b.ContactDetails)
                .HasForeignKey<Address>(b => b.ContactDetailsId);

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

        public class ContactBookContextDesignFactory : IDesignTimeDbContextFactory<ContactBookContext>
        {
            public ContactBookContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ContactBookContext>()
                    .UseSqlServer("Server=tcp:10.0.75.1,5434;Initial Catalog=ContactsDb;User Id=sa;Password=Qweasd123");

                return new ContactBookContext(optionsBuilder.Options);
            }
        }
    }
}
