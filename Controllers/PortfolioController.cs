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


    }
}
