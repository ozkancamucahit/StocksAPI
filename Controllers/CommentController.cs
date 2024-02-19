using api.Data;
using api.DTOs.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> userManager;
        private readonly IFMPService fMPService;

        #endregion

        #region CTOR
        public CommentController(ICommentRepository commentRepository,
        IStockRepository stockRepository, UserManager<AppUser> userManager, IFMPService fMPService)
        {
            this.commentRepository = commentRepository;
            this.stockRepository = stockRepository;
            this.userManager = userManager;
            this.fMPService = fMPService;
        }
        #endregion


        [HttpGet(Name = "GetAllComments")]
        [Authorize]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments([FromQuery]CommentQueryObject queryObject)
        {
            try
            {
                var comments = (await commentRepository.GetAllAsync(queryObject))
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
        
        
        [HttpPost("{symbol:alpha:required:length(2,10)}", Name = "Create")]
        [Authorize]
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Comment>> Create([FromRoute] string symbol, CreateCommentDTO commentDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stock = await stockRepository.GetBySymbolAsync(symbol);

                if (stock == null)
                {
                    stock = await fMPService.FindStockBySymbol(symbol);
                    if (stock == null)
                        return BadRequest("Stock does not exist");
                    else
                    {
                        await stockRepository.CreateAsync(stock);
                    }
                }

                var userName = User.GetUserName();
                var appUser = await userManager.FindByNameAsync(userName);
                var commentModel = commentDTO.ToCommentFromCreateDTO(stock.Id);
                commentModel.AppUserId = appUser.Id;

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
        
        
        [HttpPut("{id:int:required}", Name = "UpdateComment")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDTO comment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Comment? commentModel = await commentRepository.UpdateAsync(id, comment);

                if (commentModel is null)
                    return NotFound(comment);
                else
                    return Ok(commentModel.ToCommentDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }









    }
}