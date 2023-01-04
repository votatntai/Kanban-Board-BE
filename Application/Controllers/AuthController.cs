using Data.Models.Requests.Get;
using Data.Models.View;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthViewModel>> AuthenticatedUser([FromBody][Required] AuthRequest auth)
        {
            var user = await _authService.AuthenticatedUser(auth);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
