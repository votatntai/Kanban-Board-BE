using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/attachments")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost]
        public async Task<ActionResult<AttachmentViewModel>> SaveFile(Guid issueId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }
            var result = await _attachmentService.SaveFile(issueId, file);
            return CreatedAtAction(nameof(GetFile), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttachmentViewModel>> GetFile(Guid id)
        {
            var result = await _attachmentService.GetFile(id);
            return Ok(result);
        }
    }
} 
