using Data.Models.Requests.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/links")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _linkService;
        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LinkViewModel>> GetLink([FromRoute] Guid id)
        {
            try
            {
                var link = await _linkService.GetLink(id);
                if (link == null)
                {
                    return NotFound();
                }
                return Ok(link);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LinkViewModel>> CreateLink([FromBody] CreateLinkRequestModel link)
        {
            if (link is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _linkService.CreateLink(link);
                return CreatedAtAction(nameof(GetLink), new { id = result.Id }, result);
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
        public async Task<ActionResult<bool>> DeleteLink([FromRoute] Guid id)
        {
            return await _linkService.DeleteLink(id) ? Ok(true) : BadRequest();
        }
    }
}
