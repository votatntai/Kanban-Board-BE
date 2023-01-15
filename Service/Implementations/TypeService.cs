using Data;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class TypeService : BaseService, ITypeService
    {
        private readonly ITypeRepository _typeRepository;
        public TypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _typeRepository = unitOfWork.Type;
        }

        public async Task<TypeViewModel> GetType(Guid id)
        {
            return await _typeRepository.GetMany(type => type.Id.Equals(id)).Select(type => new TypeViewModel
            {
                Id = type.Id,
                Description = type.Description,
                Name = type.Name,
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<TypeViewModel>> GetTypes(string? name)
        {
            return await _typeRepository.GetMany(type => type.Name.Contains(name ?? "")).Select(type => new TypeViewModel
            {
                Id = type.Id,
                Description = type.Description,
                Name = type.Name,
            }).ToListAsync();
        }
    }
}
