using Data.Models.View;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<AuthViewModel> GetUserById(Guid id);
        Task<ICollection<UserViewModel>> GetUsers(string search);
    }
}
