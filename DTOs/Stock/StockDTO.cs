

using api.DTOs.Comment;

namespace api.DTOs.Stock
{
    public sealed class StockDTO
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = String.Empty;
        public string CompanyName { get; set; } = String.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = String.Empty;

        public long MarketCap { get; set; }

        public IEnumerable<CommentDTO> Comments{ get; set; } = Enumerable.Empty<CommentDTO>();

    }
}
