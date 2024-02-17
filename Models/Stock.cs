using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public sealed class Stock
    {
        public int Id { get; private set; }

        public string Symbol { get; private set; } = String.Empty;
        public string CompanyName { get; private set; } = String.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();

    }
}
