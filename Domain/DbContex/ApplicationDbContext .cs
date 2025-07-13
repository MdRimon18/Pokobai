using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Entity.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DbContex
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSet properties for each entity you want to include in the database.
        
        
        public DbSet<UserPhoneNumbers> UserPhoneNumbers { get; set; }
        public DbSet<InvoiceItemSerials> InvoiceItemSerials { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<User> Users { get; set; }// User can be Customer or Supplier or Admin or Employee or driver or any other user type
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<UserAddressBook> UserAddressBooks { get; set; }
        //  public DbSet<BillingPlans> BillingPlans { get; set; }
        // Optional: Override OnModelCreating to configure entity mappings.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships, constraints, etc., here.
            //modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(); // Example: Unique email constraint
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             

            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity &&
                 (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (entityEntry.State == EntityState.Added)
                {
                    entity.Status = "Active";
                    entity.key = Guid.NewGuid();
                    entity.EntryDateTime =DateTime.UtcNow;
                    //entity.CreatedBy = null;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.LastModifyDate = DateTime.UtcNow; 
                    //entity.UpdatedBy = null;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
