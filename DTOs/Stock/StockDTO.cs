

namespace api.DTOs.Stock
{
    public sealed class StockDTO
    {
        public int Id { get; private set; }

        public string Symbol { get; set; } = String.Empty;
        public string CompanyName { get; set; } = String.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }

        public StockDTO(int id)
        {
            Id = id;
        }
    }
}
