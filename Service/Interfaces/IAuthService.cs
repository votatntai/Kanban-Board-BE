using Data.Models.Internal;
using Data.Models.Requests.Get;
using Data.Models.View;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthViewModel> AuthenticatedUser(AuthRequest auth);
        Task<AuthModel?> GetUserById(Guid id);
    }
}
