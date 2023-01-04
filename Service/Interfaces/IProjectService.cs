using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<ICollection<ProjectViewModel>> GetProjects(string? name);
        Task <ProjectViewModel> GetProject(Guid id);
        Task<ProjectViewModel> CreateProject(CreateProjectRequestModel model, Guid leaderId);
        Task<ProjectViewModel> UpdateProject(Guid id, UpdateProjectRequestModel model);
        Task<bool> DeleteProject(Guid id);
    }
}
