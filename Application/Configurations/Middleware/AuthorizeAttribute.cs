using Data.Models.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Configurations.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public ICollection<string> Roles { get; set; }

        public AuthorizeAttribute(params string[] roles)
        {
            Roles = roles.Select(x => x.ToLower()).ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var auth = (AuthModel?)context.HttpContext.Items["User"];
            if (auth == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                var roles = auth.Roles.ToList();
                var isValid = false;
                roles.ForEach(role =>
                {
                    if (Roles.Contains(role.ToLower()))
                    {
                        isValid = true;
                    }
                });
                if (!isValid)
                {
                    context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
        }
    }
}
