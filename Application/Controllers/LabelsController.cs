using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/labels")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LabelViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LabelViewModel>> GetLabel([FromRoute] Guid id)
        {
            try
            {
                var label = await _labelService.GetLabel(id);
                if (label == null)
                {
                    return NotFound();
                }
                return Ok(label);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(LabelViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LabelViewModel>> CreateLabel([FromBody] CreateLabelRequestModel label)
        {
            if (label is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _labelService.CreateLabel(label);
                return CreatedAtAction(nameof(GetLabel), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ICollection<LabelViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ICollection<LabelViewModel>>> UpdateLabel(Guid id, [FromBody] UpdateLabelRequestModel label)
        {
            if (label is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _labelService.UpdateLabel(id, label);
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
        public async Task<ActionResult<bool>> DeleteStatus([FromRoute] Guid id)
        {
            return await _labelService.DeleteLabel(id) ? Ok(true) : BadRequest();
        }
    }
}
