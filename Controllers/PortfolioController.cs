using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public sealed class PortfolioController : ControllerBase
    {
        #region FIELDS
        private readonly UserManager<AppUser> userManager;
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        #endregion

        #region CTOR
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            this.userManager = userManager;
            this.stockRepository = stockRepository;
            this.portfolioRepository = portfolioRepository;
        }
        #endregion

        [HttpGet(Name = "UserPortfolio")]
        [Authorize]
        public async Task<IActionResult> UserPortfolio()
        {
            try
            {
                var userName = User.GetUserName();
                var appUser = await userManager.FindByNameAsync(userName);

                var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser);
                return Ok(userPortfolio);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPost("{symbol:alpha:length(4,6):required}",Name = "AddPortfolio")]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            try
            {
                var userName = User.GetUserName();
                var appUser = await userManager.FindByNameAsync(userName);
                var stock = await stockRepository.GetBySymbolAsync(symbol);

                if (stock is null)
                {
                    return BadRequest("Stock not found");
                }

                var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser!);

                if(userPortfolio.Any(e =>  e.Symbol.ToLower().Trim() == symbol.ToLower().Trim()))
                {
                    return BadRequest("Cannot add same stock to portfolio");
                }

                var portfolioModel = new Portfolio
                {
                    StockId = stock.Id,
                    AppUserId = appUser.Id
                };

                await portfolioRepository.CreateAsync(portfolioModel);
                return Created();


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{symbol:alpha:length(4,6):required}", Name = "")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            try
            {
                var userName = User.GetUserName();
                var appUser = await userManager.FindByNameAsync(userName);

                var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser);
                var filteredStock = userPortfolio.Where(p => p.Symbol.ToLower() == symbol.ToLower().Trim()).ToList();

                if(filteredStock.Count == 1)
                {
                    await portfolioRepository.DeletePortfolio(appUser, symbol);
                    return NoContent();
                }
                else
                {
                    return BadRequest("Stock is not in your portfolio");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
