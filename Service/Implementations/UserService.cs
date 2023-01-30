using Data;
using Data.Models.View;
using Data.Models.Views;
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

        public async Task<ICollection<UserViewModel>> GetUsers(string search)
        {
            return await _userRepository.GetMany(user => user.Name.Contains(search) || user.Email.Contains(search))
                .Select(user => new UserViewModel
                {
                    Email = user.Email,
                    Name = user.Name,
                    Id = user.Id,
                    Username = user.Name,
                }).ToListAsync();
        }

        public async Task<AuthViewModel> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).Select(user => new AuthViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
            }).FirstOrDefaultAsync();
            return user ?? null!;
        }
    }
}
