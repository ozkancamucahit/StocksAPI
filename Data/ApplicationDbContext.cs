using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public sealed class ApplicationDbContext :DbContext
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
