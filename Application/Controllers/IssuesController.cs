using Application.Configurations.Middleware;
using Data.Models.Internal;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/issues")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<IssueViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<IssueViewModel>>>
                              GetIssues([FromQuery] string? name)
        {
            var issues = await _issueService.GetIssues(name);
            if (issues.Any())
            {
                return Ok(issues);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IssueViewModel>> GetIssue([FromRoute] Guid id)
        {
            var issue = await _issueService.GetIssue(id);
            if (issue is null)
            {
                return NotFound();
            }
            return Ok(issue);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IssueViewModel>> CreateIssue([FromBody] CreateIssueRequestModel issue)
        {
            if (issue is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _issueService.CreateIssue(issue);
                return CreatedAtAction(nameof(GetIssue), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [Route("child")]
        [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IssueViewModel>> CreateChildIssue([FromBody] CreateIssueRequestModel issue)
        {
            if (issue is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _issueService.CreateChildIssue(issue);
                return CreatedAtAction(nameof(GetIssue), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ICollection<IssueViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ICollection<IssueViewModel>>>
         UpdatedeIssues([FromBody] ICollection<UpdateIssueRequestModel> issues)
        {
            if (issues is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _issueService.UpdateIssues(issues);
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
        [Route("{id}")]
        [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IssueViewModel>>
         UpdatedeIssue([FromRoute] Guid id, [FromBody] UpdateIssueRequestModel issue)
        {
            if (issue is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _issueService.UpdateIssue(id, issue);
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
    }
}
