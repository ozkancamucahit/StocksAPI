using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
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
    public sealed class StockController : ControllerBase
    {
        private readonly IStockRepository repository;

        #region CTOR
        public StockController(IStockRepository repository)
        {
            this.repository = repository;
        }
        #endregion

        [HttpGet(Name = "Stocks")]
        [Authorize]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Stocks([FromQuery] QueryObject query)
        {
            try
            {
                var stocks = (await repository.GetAllAsync(query))
                                .Select(s => s.ToStockDTO()).ToList();

                if (stocks.Any())
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
                var stock = await repository.GetByIdAsync(id);

                if (stock is null)
                    return NoContent();
                else
                    return Ok(stock.ToStockDTO());

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost(Name = "CreateStock")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDTO stock)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Stock stockModel = stock.ToStockDTO();
                var res = await repository.CreateAsync(stockModel);
                return CreatedAtRoute("StockById", new { stockModel.Id }, stockModel.ToStockDTO());

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id:int:required}", Name = "UpdateStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDTO stock)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Stock? stockModel = await repository.UpdateAsync(id, stock);

                if (stockModel is null)
                    return NotFound(stock);
                else
                    return Ok(stockModel.ToStockDTO());

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int:required}", Name = "DeleteStock")]
        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            try
            {
                var res = await repository.DeleteAsync(id);

                if (res is null)
                    return NotFound();
                else
                    return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
