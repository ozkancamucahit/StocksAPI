using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        #region CTOR
        public StockController(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        [HttpGet(Name = "Stocks")]
        public async Task<IActionResult> Stocks()
        {
            try
            {
                var stocks = context.Stocks
                                .AsNoTracking()
                                .AsEnumerable()
                                .Select(s => s.ToStockDTO());

                if(stocks.Any())
                    return Ok(stocks);
                else
                    return NoContent();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{id:int:min(1):required}", Name = "StockById")]


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StockById([FromRoute] int id)
        {
            try
            {
                var stock = context.Stocks
                    .Find(id);

                if(stock is null)
                    return NoContent();
                else
                    return Ok(stock.ToStockDTO());

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }





    }
}
