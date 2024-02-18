
using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllAsync(Helpers.QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockRequestDTO);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}

