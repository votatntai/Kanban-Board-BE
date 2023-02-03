using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IIssueService
    {
        Task<ICollection<IssueViewModel>> GetIssues(string? name);
        Task<IssueViewModel> GetIssue(Guid id);
        Task<IssueViewModel> CreateIssue(CreateIssueRequestModel model);
        Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueRequestModel model);
        Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueRequestModel model);
        Task<ChildIssueViewModel> UpdateChildIssue(Guid id, UpdateChildIssueRequestModel model);
        Task<ICollection<IssueViewModel>> UpdateIssues(ICollection<UpdateIssueRequestModel> models);
        Task<bool> DeleteIssue(Guid id);
    }
}
