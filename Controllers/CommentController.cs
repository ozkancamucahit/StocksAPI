using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
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
    public sealed class CommentController : ControllerBase
    {
        #region FIELDS
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;

        #endregion

        #region CTOR
        public CommentController(ICommentRepository commentRepository,
        IStockRepository stockRepository)
        {
            this.commentRepository = commentRepository;
            this.stockRepository = stockRepository;
        }
        #endregion


        [HttpGet(Name = "GetAllComments")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            try
            {
                var comments = (await commentRepository.GetAllAsync())
                                .Select(s => s.ToCommentDTO());

                if (comments.Any())
                    return Ok(comments);
                else
                    return NoContent();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int:required}", Name = "GetById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Comment>> GetById([FromRoute]int id)
        {
            try
            {
                var comment = await commentRepository.GetByIdAsync(id);

                if (comment is null)
                    return NoContent();
                else
                    return Ok(comment);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        
        
        [HttpPost("{stockId:int:required}", Name = "Create")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Comment>> Create([FromRoute] int stockId, CreateCommentDTO commentDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await stockRepository.StockExists(stockId))
                {
                    return BadRequest("Stock does not exist");
                }

                var commentModel = commentDTO.ToCommentFromCreateDTO(stockId);
                await commentRepository.CreateAsync(commentModel);
                return CreatedAtRoute("GetById", new { commentModel.Id }, commentModel.ToCommentDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int:required}", Name = "DeleteComment")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            try
            {
                var res = await commentRepository.DeleteAsync(id);

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