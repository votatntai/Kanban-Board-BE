using Data;
using Data.Models.Internal;
using Data.Models.Requests.Get;
using Data.Models.View;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Settings;

namespace Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSetting _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<AppSetting> appSettings) : base(unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
        }

        public async Task<AuthViewModel> AuthenticatedUser(AuthRequest auth)
        {
            var user = await _userRepository.GetMany(user => user.Email.Equals(auth.Email) && user.Password.Equals(auth.Password))
                .Include(user => user.Roles).FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Status = "Activated",
                    Roles = user.Roles.Select(role => role.Name).ToArray()
                });
                return new AuthViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Name = user.Name,
                    Roles = user.Roles.Select(role => role.Name).ToArray(),
                    Token = token
                };
            }
            return null!;
        }

        public async Task<AuthModel?> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id))
                .Include(user => user.Roles).FirstOrDefaultAsync();
            if (user != null)
            {
                return new AuthModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Roles = user.Roles.Select(role => role.Name).ToArray(),
                    Status = "Activated",
                };
            }
            return null;
        }

        private string GenerateJwtToken(AuthModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("roles", String.Join(",", user.Roles)),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
