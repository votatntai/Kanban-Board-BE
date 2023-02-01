using Data.Models.Requests.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/log-works")]
    [ApiController]
    public class LogWorksController : ControllerBase
    {
        private readonly ILogWorkService _logWorkService;
        public LogWorksController(ILogWorkService logWorkService)
        {
            _logWorkService = logWorkService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LogWorkViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LogWorkViewModel>> GetLogWork([FromRoute] Guid id)
        {
            try
            {
                var logWork = await _logWorkService.GetLogWork(id);
                if (logWork == null)
                {
                    return NotFound();
                }
                return Ok(logWork);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(LogWorkViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LogWorkViewModel>> CreateLogWork([FromBody] CreateLogWorkRequestModel logWork)
        {
            if (logWork is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _logWorkService.CreateLogWork(logWork);
                return CreatedAtAction(nameof(GetLogWork), new { id = result.Id }, result);
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
        public async Task<ActionResult<bool>> DeleteLogWork([FromRoute] Guid id)
        {
            return await _logWorkService.DeleteLogWork(id) ? Ok(true) : BadRequest();
        }
    }
}
