using Application.Configurations.Middleware;
using Data.Models.Internal;
using Data.Models.Requests.Create;
using Data.Models.Requests.Get;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/statuses")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<StatusViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<StatusViewModel>>> GetStatuses([FromQuery] StatusRequest filter)
        {
            var statuses = await _statusService.GetStatuses(filter);
            if (statuses is null || statuses.Count == 0)
            {
                return NotFound();
            }
            return Ok(statuses);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(StatusViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StatusViewModel>> GetStatus([FromRoute] Guid id)
        {
            var status = await _statusService.GetStatus(id);
            if (status is null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpPost]
        [ProducesResponseType(typeof(StatusViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StatusViewModel>> CreateStatus([FromBody] CreateStatusRequestModel status)
        {
            if (status is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _statusService.CreateStatus(status);
                return CreatedAtAction(nameof(GetStatus), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ICollection<StatusViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ICollection<StatusViewModel>>>
         UpdatedeStatuss([FromRoute] Guid id ,[FromBody] UpdateStatusRequestModel status)
        {
            if (status is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _statusService.UpdateStatus(id, status);
                if (result is not null)
                {
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ICollection<StatusViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ICollection<StatusViewModel>>>
         UpdatedeStatuss([FromBody] ICollection<UpdateStatusRequestModel> statuses)
        {
            if (statuses is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _statusService.UpdateStatuses(statuses);
                if (result is not null)
                {
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                return BadRequest();
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
        public async Task<ActionResult> DeleteStatus([FromRoute] Guid id, Guid inheritanceId)
        {
            return await _statusService.DeleteStatus(id, inheritanceId) ? StatusCode(StatusCodes.Status204NoContent) : BadRequest();
        }
    }
}
