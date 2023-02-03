using Data.Models.Internal;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<ICollection<ProjectViewModel>> GetProjects(AuthModel user, string? name);
        Task <ProjectViewModel> GetProject(Guid id);
        Task<MemberViewModel> AddMember(Guid projectId, Guid memberId);
        Task<bool> RemoveMember(Guid memberId, Guid projectId);
        Task<ProjectViewModel> CreateProject(CreateProjectRequestModel model, Guid leaderId);
        Task<ProjectViewModel> UpdateProject(Guid id, UpdateProjectRequestModel model);
        Task<bool> DeleteProject(Guid id);
    }
}
