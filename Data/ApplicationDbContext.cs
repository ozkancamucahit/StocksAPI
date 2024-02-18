using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        #endregion




    }
}
