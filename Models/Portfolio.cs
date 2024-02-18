using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Portfolios")]
    public sealed class Portfolio
    {
        public string AppUserId { get; set; } = String.Empty;

        public int StockId { get; set; }

        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }

    }
}
