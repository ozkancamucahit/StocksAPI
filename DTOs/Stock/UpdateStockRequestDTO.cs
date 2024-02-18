using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock
{
    public sealed class UpdateStockRequestDTO
    {
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = String.Empty;
        
        [Required]
        [MaxLength(10)]
        public string CompanyName { get; set; } = String.Empty;
        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }
    }
}