using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.DTOs.Stock
{
    public sealed class CreateStockRequestDTO
    {
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = String.Empty;
        [Required]
        [MaxLength(20)]
        public string CompanyName { get; set; } = String.Empty;
        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }
    }
}
