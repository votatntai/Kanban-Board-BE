using Application.Configurations.Middleware;
using Data.Models.Internal;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/projects")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ProjectViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<ProjectViewModel>>>
                              GetProjects([FromQuery] string? name)
        {
            var projects = await _projectService.GetProjects(name);
            if (projects.Any())
            {
                return Ok(projects);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectViewModel>> GetProject([FromRoute] Guid id)
        {
            var project = await _projectService.GetProject(id);
            if (project is null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectViewModel>> CreateProject([FromBody] CreateProjectRequestModel project)
        {
            if (project is null)
            {
                return BadRequest(new());
            }
            try
            {
                var user = (AuthModel?)HttpContext.Items["User"];
                var result = await _projectService.CreateProject(project, user!.Id);
                return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPost]
        [Route("member")]
        //[Authorize]
        [ProducesResponseType(typeof(MemberViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MemberViewModel>> AddMember(Guid projectId, Guid memberId)
        {
            try
            {
                var member = await _projectService.AddMember(projectId, memberId);
                return member != null ? Ok(member) : BadRequest();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectViewModel>>
         UpdatedeProject([FromRoute] Guid id, [FromBody] UpdateProjectRequestModel project)
        {
            if (project is null)
            {
                return BadRequest(new());
            }
            try
            {
                var result = await _projectService.UpdateProject(id, project);
                if (result is not null)
                {
                    return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
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
