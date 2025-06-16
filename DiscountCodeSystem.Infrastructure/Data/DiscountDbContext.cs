using DiscountCodeSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Infrastructure.Data
{
    public class DiscountDbContext : DbContext, IDiscountCodeDbContext
    {
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }
        public DbSet<DiscountCode> DiscountCodes => Set<DiscountCode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<DiscountCode>()
                .HasIndex(dc => dc.Code)
                .IsUnique();
        }

    }
}
