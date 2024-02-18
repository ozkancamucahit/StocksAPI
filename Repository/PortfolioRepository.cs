using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public sealed class PortfolioRepository : IPortfolioRepository
    {
        #region FIELDS
        private readonly ApplicationDbContext context;
        #endregion


        #region CTOR
        public PortfolioRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await context.Portfolios.AddAsync(portfolio);

            int rows = await context.SaveChangesAsync();

            if (rows != 1)
                throw new InvalidOperationException("Could not create portfolio");
            else
                return portfolio;
        }
        #endregion


        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await context.Portfolios
                .Where(p => p.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap

                }).ToListAsync();
        }
    }
}
