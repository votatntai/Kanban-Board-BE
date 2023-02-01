using Data.Models.Requests.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommentViewModel>> GetComment([FromRoute] Guid id)
        {
            try
            {
                var comment = await _commentService.GetComment(id);
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommentViewModel>> CreateComment([FromBody] CreateCommentRequestModel comment)
        {
            if (comment is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _commentService.CreateComment(comment);
                return CreatedAtAction(nameof(GetComment), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteComment([FromRoute] Guid id)
        {
            return await _commentService.DeleteComment(id) ? Ok(true) : BadRequest();
        }
    }
}
