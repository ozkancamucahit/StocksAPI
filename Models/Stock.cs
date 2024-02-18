using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Stocks")]
    public sealed class Stock
    {
        #region FIELDS
        public int Id { get; set; }

        public string Symbol { get; set; } = String.Empty;
        public string CompanyName { get; set; } = String.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
        public List<Portfolio> Portfolios { get; set; } = new();

        #endregion

    }
}
