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
        private readonly IStatusRepository _statusRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IIssueLabelRepository _issueLabelRepository;

        public IssueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _issueRepository = unitOfWork.Issue;
            _projectRepository = unitOfWork.Project;
            _priorityRepository = unitOfWork.Priority;
            _issueLabelRepository = unitOfWork.IssueLabel;
            _statusRepository = unitOfWork.Status;
        }

        public async Task<IssueViewModel> CreateChildIssue(CreateIssueRequestModel model)
        {
            var id = Guid.NewGuid();
            var project = await _projectRepository.GetMany(project => project.Id.Equals(model.ProjectId))
                .Include(project => project.Types)
                .FirstOrDefaultAsync();
            var priority = await _priorityRepository.GetMany(priority => priority.Name.Equals("Medium")).FirstOrDefaultAsync();
            var issue = new Issue
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CreateAt = DateTime.Now,
                IsChild = true,
                IsClose = false,
                AssigneeId = model.AssigneeId != null ? model.AssigneeId : null!,
                Position = model.Position,
                ProjectId = model.ProjectId,
                PriorityId = priority!.Id,
                TypeId = project!.Types.Where(type => type.Name.Equals("SubTask")).Select(type => type.Id).FirstOrDefault(),
                StatusId = model.StatusId,
                ReporterId = project!.LeaderId
            };
            _issueRepository.Add(issue);
            project.LastActivity = DateTime.Now;
            _projectRepository.Update(project);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetIssue(id);
            }
            return null!;
        }


        public async Task<IssueViewModel> CreateIssue(CreateIssueRequestModel model)
        {
            var id = Guid.NewGuid();
            var project = await _projectRepository.GetMany(project => project.Id.Equals(model.ProjectId))
                .Include(project => project.Types)
                .FirstOrDefaultAsync();
            var priority = await _priorityRepository.GetMany(priority => priority.Name.Equals("Medium")).FirstOrDefaultAsync();
            var issue = new Issue
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CreateAt = DateTime.Now,
                IsChild = false,
                IsClose = false,
                AssigneeId = model.AssigneeId != null ? model.AssigneeId : null!,
                DueDate = model.DueDate,
                Position = model.Position,
                ProjectId = model.ProjectId,
                PriorityId = priority!.Id,
                TypeId = project!.Types.Where(type => type.Name.Equals("Task")).Select(type => type.Id).FirstOrDefault(),
                StatusId = model.StatusId,
                EstimateTime = model.EstimateTime,
                ReporterId = project!.LeaderId
            };
            _issueRepository.Add(issue);
            project.LastActivity = DateTime.Now;
            _projectRepository.Update(project);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetIssue(id);
            }
            return null!;
        }

        public async Task<IssueViewModel> GetIssue(Guid id)
        {
            return await _issueRepository.GetMany(issue => issue.Id.Equals(id))
                .Include(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                .Select(issue => new IssueViewModel
                {
                    Id = issue.Id,
                    CreateAt = issue.CreateAt,
                    UpdateAt = issue.UpdateAt,
                    Position = issue.Position,
                    Description = issue.Description,
                    IsClose = issue.IsClose,
                    IsChild = issue.IsChild,
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
                    ResolveAt = issue.ResolveAt,
                    Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                    {
                        Id = issueLabel.Label.Id,
                        Name = issueLabel.Label.Name,
                        ProjectId = issueLabel.Label.ProjectId,
                    }).ToList()
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<IssueViewModel>> GetIssues(string? name)
        {
            return await _issueRepository.GetMany(issue => issue.Name.Contains(name ?? "") && !issue.IsChild)
                .Include(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                .Select(issue => new IssueViewModel
                {
                    Id = issue.Id,
                    CreateAt = issue.CreateAt,
                    UpdateAt = issue.UpdateAt,
                    Description = issue.Description,
                    IsChild = issue.IsChild,
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
                    Position = issue.Position,
                    ResolveAt = issue.ResolveAt,
                    Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                    {
                        Id = issueLabel.Label.Id,
                        Name = issueLabel.Label.Name,
                        ProjectId = issueLabel.Label.ProjectId,
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<ICollection<IssueViewModel>> GetChildIssues(string? name)
        {
            return await _issueRepository.GetMany(issue => issue.Name.Contains(name ?? "") && issue.IsChild)
                .Include(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                .Select(issue => new IssueViewModel
                {
                    Id = issue.Id,
                    CreateAt = issue.CreateAt,
                    IsChild = issue.IsChild,
                    UpdateAt = issue.UpdateAt,
                    Description = issue.Description,
                    IsClose = issue.IsClose,
                    Name = issue.Name,
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
                    Position = issue.Position,
                    ResolveAt = issue.ResolveAt,
                    Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                    {
                        Id = issueLabel.Label.Id,
                        Name = issueLabel.Label.Name,
                        ProjectId = issueLabel.Label.ProjectId,
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueRequestModel model)
        {
            var issue = await _issueRepository.GetMany(issue => issue.Id.Equals(id))
                .Include(issue => issue.Project)
                .FirstOrDefaultAsync();
            if (issue != null)
            {
                if (model.Name != null) issue.Name = model.Name;
                if (model.Description != null) issue.Description = model.Description;
                issue.DueDate = model.DueDate;
                if (model.StatusId != null)
                {
                    issue.StatusId = (Guid)model.StatusId;
                    var status = await _statusRepository.GetMany(status => status.Id.Equals(model.StatusId)).FirstOrDefaultAsync();
                    if (status != null && status.IsLast)
                    {
                        issue.IsClose = true;
                        issue.ResolveAt = DateTime.Now;
                    }
                    else
                    {
                        issue.IsClose = false;
                        issue.ResolveAt = null;
                    }
                }
                if (model.PriorityId != null) issue.PriorityId = (Guid)model.PriorityId;
                if (model.TypeId != null) issue.TypeId = (Guid)model.TypeId;
                issue.AssigneeId = model.AssigneeId;
                if (model.EstimateTime != null) issue.EstimateTime = (int)model.EstimateTime;
                if (model.ReporterId != null) issue.ReporterId = (Guid)model.ReporterId;
                if (model.Labels != null || model.Labels != null && model.Labels.Count > 0)
                {
                    var issueLabels = new List<IssueLabel>();
                    var oldIssueLabels = await _issueLabelRepository.GetMany(issueLabel => issueLabel.IssueId.Equals(issue.Id)).ToListAsync();
                    _issueLabelRepository.RemoveRange(oldIssueLabels);
                    await _unitOfWork.SaveChanges();
                    foreach (var label in model.Labels)
                    {
                        issueLabels.Add(new IssueLabel
                        {
                            IssueId = issue.Id,
                            LabelId = label.Id,
                            UpdateAt = DateTime.Now
                        });
                    }
                    _issueLabelRepository.AddRange(issueLabels);
                }
                issue.UpdateAt = DateTime.Now;
                issue.Project.LastActivity = DateTime.Now;
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
                        .Include(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                        .FirstOrDefaultAsync();
                    if (issue != null)
                    {
                        if (model.Name != null) issue.Name = model.Name;
                        if (model.Description != null) issue.Description = model.Description;
                        if (model.DueDate != null) issue.DueDate = (DateTime)model.DueDate;
                        if (model.PriorityId != null) issue.PriorityId = (Guid)model.PriorityId;
                        if (model.TypeId != null) issue.TypeId = (Guid)model.TypeId;
                        if (model.AssigneeId != null) issue.AssigneeId = model.AssigneeId;
                        if (model.EstimateTime != null) issue.EstimateTime = (int)model.EstimateTime;
                        if (model.ReporterId != null) issue.ReporterId = (Guid)model.ReporterId;
                        if (model.Position != null) issue.Position = (int)model.Position;
                        if (model.StatusId != null)
                        {
                            issue.StatusId = (Guid)model.StatusId;
                            var status = await _statusRepository.GetMany(status => status.Id.Equals(model.StatusId)).FirstOrDefaultAsync();
                            if (status != null && status.IsLast)
                            {
                                issue.IsClose = true;
                                issue.ResolveAt = DateTime.Now;
                            }
                            else
                            {
                                issue.IsClose = false;
                                issue.ResolveAt = null;
                            }
                        }
                        issue.UpdateAt = DateTime.Now;
                        issue.Project.LastActivity = DateTime.Now;
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
                        IsChild = issue.IsChild,
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
                        ResolveAt = issue.ResolveAt,
                        Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                        {
                            Id = issueLabel.Label.Id,
                            Name = issueLabel.Label.Name,
                            ProjectId = issueLabel.Label.ProjectId,
                        }).ToList(),
                    }).ToList();
                }
            }
            return null!;
        }

        public async Task<bool> DeleteIssue(Guid id)
        {
            var result = false;
            var issue = await _issueRepository.GetMany(issue => issue.Id.Equals(id)).Include(issue => issue.Project)
                .Include(issue => issue.IssueLabels).FirstOrDefaultAsync();
            if (issue != null)
            {
                var childIssues = await _issueRepository.GetMany(childIssue => childIssue.ParentId.Equals(issue.Id))
                    .Include(childIssue => childIssue.IssueLabels).ToListAsync();

                issue.Project.LastActivity = DateTime.Now;
                foreach(var childIssue in childIssues)
                {
                    _issueLabelRepository.RemoveRange(childIssue.IssueLabels);
                }
                _issueLabelRepository.RemoveRange(issue.IssueLabels);
                _issueRepository.Update(issue);
                _issueRepository.RemoveRange(childIssues);
                _issueRepository.Remove(issue);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }

    }
}
