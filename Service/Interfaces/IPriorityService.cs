using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IPriorityService
    {
        Task<ICollection<PriorityViewModel>> GetPriorities(string? name);
        Task<PriorityViewModel> GetPriority(Guid id);
    }
}
