

using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public sealed class StockRepository : IStockRepository
    {
        #region FIELDS
        private readonly ApplicationDbContext context;

        #endregion

        #region CTOR
        public StockRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {

            await context.Stocks.AddAsync(stockModel);
            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not save data");
            else
                return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            Stock? stockModel = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel is null)
                return null;

            else
                context.Stocks.Remove(stockModel);

            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not delete data");
            return stockModel;
        }
        #endregion


        public async Task<IEnumerable<Stock>> GetAllAsync(Helpers.QueryObject query)
        {
            var stocks = context.Stocks.AsNoTracking();

            if(!String.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName.Trim()));
            }

            if(!String.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol.Trim()));
            }

            if(!String.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol): stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PaneNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync(); 

        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await context.Stocks
                    .FindAsync(id);

            if (stock is null)
                return null;
            else
                return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stock)
        {
            Stock? stockModel = await context.Stocks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel is null)
                return null;

            #region UPDATE
            stockModel.Symbol = stock.Symbol;
            stockModel.CompanyName = stock.CompanyName;
            stockModel.MarketCap = stock.MarketCap;
            stockModel.Purchase = stock.Purchase;
            stockModel.LastDiv = stock.LastDiv;
            stockModel.Industry = stock.Industry;
            #endregion

            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not update data");
            else 
                return stockModel;
        }
    }
}

