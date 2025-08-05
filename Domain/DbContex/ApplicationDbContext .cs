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


        public DbSet<ProductVariants> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<Attributte> Attributtes { get; set; }
        public DbSet<AttributteValue> AttributteValues { get; set; }
        public DbSet<OrderStage> OrderStages { get; set; }
        public DbSet<ProductCategoryTypes> ProductCategoryTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.Entity<ProductVariants>().HasMany(v => v.ProductVariantAttributes).WithOne(va => va.ProductVariant).HasForeignKey(va => va.ProductVariantId);
            modelBuilder.Entity<Attributte>().HasMany(a => a.AttributeValues).WithOne(av => av.Attributte).HasForeignKey(av => av.AttributeId);
            modelBuilder.Entity<ProductVariantAttribute>().HasOne(va => va.AttributeValue).WithMany().HasForeignKey(va => va.AttributeValueId);
            modelBuilder.Entity<ProductVariantAttribute>().HasOne(va => va.Attributte).WithMany().HasForeignKey(va => va.AttributeId);
       
        //  modelBuilder.Entity<Products>().HasMany(p => p.Variants).WithOne(v => v.Product).HasForeignKey(v => v.ProductId);
      
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
