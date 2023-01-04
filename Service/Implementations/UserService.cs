using Data;
using Data.Models.View;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.User;
        }

        public async Task<AuthViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).Select(user => new AuthViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Name= user.Name,
                Email = user.Email,
            }).FirstOrDefaultAsync();
            return user ?? null!;
        }
    }
}
