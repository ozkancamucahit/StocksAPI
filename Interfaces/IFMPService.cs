using api.Models;

namespace api.Interfaces
{
    public interface IFMPService
    {
        Task<Stock?> FindStockBySymbol(string symbol);
    }
}
