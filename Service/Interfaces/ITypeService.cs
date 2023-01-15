using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ITypeService
    {
        Task<ICollection<TypeViewModel>> GetTypes(string? name);
        Task<TypeViewModel> GetType(Guid id);
    }
}
