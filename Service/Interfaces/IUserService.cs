using Data.Models.View;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<AuthViewModel> GetUserById(Guid id);
    }
}
