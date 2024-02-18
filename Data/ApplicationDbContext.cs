using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace api.Data
{
    public sealed class ApplicationDbContext :IdentityDbContext<AppUser>
    {
        #region CTOR
        public ApplicationDbContext( DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        #endregion

        #region FIELDS
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>()
                .HasKey(p => new { p.AppUserId, p.StockId });

            builder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(p => p.Portfolios)
                .HasForeignKey(p => p.StockId);
            
            builder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(p => p.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            List<IdentityRole> roles = new()
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            builder
                .Entity<Comment>()
                .HasOne(p => p.Stock)
                .WithMany(p => p.Comments!)
                .HasForeignKey(p => p.StockId);
            
            builder
                .Entity<Stock>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.Stock!)
                .HasForeignKey(p => p.StockId);

        }


    }
}
