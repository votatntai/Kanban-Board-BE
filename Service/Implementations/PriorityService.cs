using Data;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class PriorityService : BaseService, IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        public PriorityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _priorityRepository = unitOfWork.Priority;
        }

        public async Task<PriorityViewModel> GetPriority(Guid id)
        {
            return await _priorityRepository.GetMany(priority => priority.Id.Equals(id)).Select(priority => new PriorityViewModel
            {
                Id = priority.Id,
                Description = priority.Description,
                Name = priority.Name,
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<PriorityViewModel>> GetPriorities(string? name)
        {
            return await _priorityRepository.GetMany(priority => priority.Name.Contains(name ?? "")).Select(priority => new PriorityViewModel
            {
                Id = priority.Id,
                Description = priority.Description,
                Name = priority.Name,
            }).ToListAsync();
        }
    }
}
