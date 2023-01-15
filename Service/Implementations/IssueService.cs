using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class IssueService : BaseService, IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPriorityRepository _priorityRepository;

        public IssueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _issueRepository = unitOfWork.Issue;
            _projectRepository = unitOfWork.Project;
            _priorityRepository = unitOfWork.Priority;
        }

        public async Task<IssueViewModel> CreateIssue(CreateIssueRequestModel model)
        {
            var id = Guid.NewGuid();
            var project = await _projectRepository.GetMany(project => project.Id.Equals(model.ProjectId))
                .Include(project => project.Types)
                .FirstOrDefaultAsync();
            var priority = await _priorityRepository.GetAll().OrderBy(priority => priority.Value).FirstOrDefaultAsync();
            var issue = new Issue
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CreateAt = DateTime.Now,
                IsClose = false,
                AssigneeId = model.AssigneeId != null ? model.AssigneeId : null!,
                DueDate = model.DueDate,
                Position = model.Position,
                ProjectId = model.ProjectId,
                PriorityId = priority!.Id, 
                TypeId = project!.Types.Select(type => type.Id).FirstOrDefault(),
                StatusId = model.StatusId,
                EstimateTime = model.EstimateTime,
                ReporterId = project!.LeaderId
            };
            _issueRepository.Add(issue);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetIssue(id);
            }
            return null!;
        }

        public async Task<bool> DeleteIssue(Guid id)
        {
            var issue = await _issueRepository.FirstOrDefaultAsync(issue => issue.Id.Equals(id));
            if (issue != null)
            {
                _issueRepository.Remove(issue);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IssueViewModel> GetIssue(Guid id)
        {
            return await _issueRepository.GetMany(issue => issue.Id.Equals(id)).Select(issue => new IssueViewModel
            {
                Id = issue.Id,
                CreateAt = issue.CreateAt,
                UpdateAt = issue.UpdateAt,
                Description = issue.Description,
                IsClose = issue.IsClose,
                Name = issue.Name,
                EstimateTime = issue.EstimateTime,
                DueDate = issue.DueDate,
                Assignee = issue.Assignee != null ? new UserViewModel
                {
                    Id = issue.Assignee.Id,
                    Email = issue.Assignee.Email,
                    Name = issue.Assignee.Name,
                    Username = issue.Assignee.Username
                } : null!,
                PriorityId = issue.PriorityId,
                ProjectId = issue.Project.Id,
                Reporter = new UserViewModel
                {
                    Id = issue.Reporter.Id,
                    Email = issue.Reporter.Email,
                    Name = issue.Reporter.Name,
                    Username = issue.Reporter.Username
                },
                StatusId = issue.StatusId,
                TypeId = issue.TypeId,
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<IssueViewModel>> GetIssues(string? name)
        {
            return await _issueRepository.GetMany(issue => issue.Name.Contains(name ?? "")).Select(issue => new IssueViewModel
            {
                Id = issue.Id,
                CreateAt = issue.CreateAt,
                UpdateAt = issue.UpdateAt,
                Description = issue.Description,
                IsClose = issue.IsClose,
                Name = issue.Name,
                EstimateTime = issue.EstimateTime,
                DueDate = issue.DueDate,
                Assignee = issue.Assignee != null ? new UserViewModel
                {
                    Id = issue.Assignee.Id,
                    Email = issue.Assignee.Email,
                    Name = issue.Assignee.Name,
                    Username = issue.Assignee.Username
                } : null!,
                PriorityId = issue.PriorityId,
                ProjectId = issue.Project.Id,
                Reporter = new UserViewModel
                {
                    Id = issue.Reporter.Id,
                    Email = issue.Reporter.Email,
                    Name = issue.Reporter.Name,
                    Username = issue.Reporter.Username
                },
                StatusId = issue.StatusId,
                TypeId = issue.TypeId,
            }).ToListAsync();
        }

        public async Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueRequestModel model)
        {
            var issue = await _issueRepository.GetMany(issue => issue.Id.Equals(id)).FirstOrDefaultAsync();
            if (issue != null)
            {
                if (model.Name != null) issue.Name = model.Name;
                if (model.Description != null) issue.Description = model.Description;
                if (model.DueDate != null) issue.DueDate = (DateTime)model.DueDate;
                if (model.StatusId != null) issue.StatusId = (Guid)model.StatusId;
                if (model.PriorityId != null) issue.PriorityId = (Guid)model.PriorityId;
                if (model.TypeId != null) issue.TypeId = (Guid)model.TypeId;
                if (model.AssigneeId != null) issue.AssigneeId = model.AssigneeId;
                if (model.EstimateTime != null) issue.EstimateTime = (int)model.EstimateTime;
                if (model.ReporterId != null) issue.ReporterId = (Guid)model.ReporterId;
                if (model.IsClose != null) issue.IsClose = (bool)model.IsClose;
                issue.UpdateAt = DateTime.UtcNow;
                _issueRepository.Update(issue);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetIssue(id);
                }
            }
            return null!;
        }

        public async Task<ICollection<IssueViewModel>> UpdateIssues(ICollection<UpdateIssueRequestModel> models)
        {
            if (models.Count > 0)
            {
                var issues = new List<Issue>();
                foreach (var model in models)
                {
                    var issue = await _issueRepository.GetMany(issue => issue.Id.Equals(model.Id))
                        .Include(issue => issue.Project)
                        .FirstOrDefaultAsync();
                    if (issue != null)
                    {
                        if (model.Name != null) issue.Name = model.Name;
                        if (model.Description != null) issue.Description = model.Description;
                        if (model.DueDate != null) issue.DueDate = (DateTime)model.DueDate;
                        if (model.StatusId != null) issue.StatusId = (Guid)model.StatusId;
                        if (model.PriorityId != null) issue.PriorityId = (Guid)model.PriorityId;
                        if (model.TypeId != null) issue.TypeId = (Guid)model.TypeId;
                        if (model.AssigneeId != null) issue.AssigneeId = model.AssigneeId;
                        if (model.EstimateTime != null) issue.EstimateTime = (int)model.EstimateTime;
                        if (model.ReporterId != null) issue.ReporterId = (Guid)model.ReporterId;
                        if (model.Position != null) issue.Position = (int)model.Position;
                        if (model.IsClose != null) issue.IsClose = (bool)model.IsClose;
                        issue.UpdateAt = DateTime.UtcNow;
                        issues.Add(issue);
                    }
                }
                _issueRepository.UpdateRange(issues);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return issues.OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
                    {
                        Id = issue.Id,
                        CreateAt = issue.CreateAt,
                        UpdateAt = issue.UpdateAt,
                        Description = issue.Description,
                        IsClose = issue.IsClose,
                        Name = issue.Name,
                        EstimateTime = issue.EstimateTime,
                        DueDate = issue.DueDate,
                        Assignee = issue.Assignee != null ? new UserViewModel
                        {
                            Id = issue.Assignee.Id,
                            Email = issue.Assignee.Email,
                            Name = issue.Assignee.Name,
                            Username = issue.Assignee.Username
                        } : null!,
                        Position = issue.Position,
                        PriorityId = issue.PriorityId,
                        ProjectId = issue.Project.Id,
                        Reporter = issue.Reporter != null ? new UserViewModel
                        {
                            Id = issue.Reporter.Id,
                            Email = issue.Reporter.Email,
                            Name = issue.Reporter.Name,
                            Username = issue.Reporter.Username
                        } : null!,
                        StatusId = issue.StatusId,
                        TypeId = issue.TypeId,
                    }).ToList();
                }
            }
            return null!;
        }
    }
}
