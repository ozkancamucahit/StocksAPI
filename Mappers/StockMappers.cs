using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase
            };
        }
        public static Stock ToStockDTO(this CreateStockRequestDTO model)
        {
            return new Stock()
            {
                Symbol = model.Symbol,
                CompanyName = model.CompanyName,
                Industry = model.Industry,
                LastDiv = model.LastDiv,
                MarketCap = model.MarketCap,
                Purchase = model.Purchase
            };
        }


    }
}
