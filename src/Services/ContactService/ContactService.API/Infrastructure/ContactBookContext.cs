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
        public ContactBookContext(DbContextOptions<ContactBookContext> options)
           : base(options)
        { }

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
        }

        public class ContactBookContextDesignFactory : IDesignTimeDbContextFactory<ContactBookContext>
        {
            public ContactBookContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ContactBookContext>()
                    .UseSqlServer("Server=tcp:10.0.75.1,5433;Initial Catalog=Services.ContactsDb;User Id=sa;Password=Qweasd123@");

                return new ContactBookContext(optionsBuilder.Options);
            }
        }
    }
}
