using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Requests.Get;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class StatusService : BaseService, IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IIssueRepository _issueRepository;

        public StatusService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _statusRepository = unitOfWork.Status;
            _issueRepository = unitOfWork.Issue;
        }

        public async Task<ICollection<StatusViewModel>> GetStatuses(StatusRequest filter)
        {
            return await _statusRepository.GetMany(status => filter.ProjectId != null ? status.ProjectId.Equals(filter.ProjectId) : false)
                .Select(status => new StatusViewModel
                {
                    Id = status.Id,
                    Description = status.Description,
                    Name = status.Name,
                    Position = status.Position,
                    IsFirst = status.IsFirst,
                    IsLast = status.IsLast,
                    Limit = status.Limit,
                    Issues = status.Issues.Where(issue => !issue.IsChild).OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
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
                        Priority = new PriorityViewModel
                        {
                            Id = issue.Priority.Id,
                            Description = issue.Priority.Description,
                            Name = issue.Priority.Name,
                            Value = issue.Priority.Value
                        },
                        ProjectId = issue.Project.Id,
                        Reporter = new UserViewModel
                        {
                            Id = issue.Reporter.Id,
                            Email = issue.Reporter.Email,
                            Name = issue.Reporter.Name,
                            Username = issue.Reporter.Username
                        },
                        StatusId = issue.StatusId,
                        Type = new TypeViewModel
                        {
                            Id = issue.Type.Id,
                            Description = issue.Type.Description,
                            Name = issue.Type.Name,
                        },
                        ResolveAt = issue.ResolveAt,
                        Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                        {
                            Id = issueLabel.Label.Id,
                            Name = issueLabel.Label.Name,
                            ProjectId = issueLabel.Label.ProjectId,
                        }).ToList()
                    }).ToList(),
                }).ToListAsync();
        }

        public async Task<StatusViewModel> GetStatus(Guid id)
        {
            return await _statusRepository.GetMany(status => status.Id.Equals(id))
                .Select(status => new StatusViewModel
                {
                    Id = status.Id,
                    Description = status.Description,
                    Name = status.Name,
                    Position = status.Position,
                    IsFirst = status.IsFirst,
                    IsLast = status.IsLast,
                    Limit = status.Limit,
                    Issues = status.Issues.Where(issue => !issue.IsChild).OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
                    {
                        Id = issue.Id,
                        CreateAt = issue.CreateAt,
                        UpdateAt = issue.UpdateAt,
                        Position = issue.Position,
                        IsChild = issue.IsChild,
                        Description = issue.Description,
                        IsClose = issue.IsClose,
                        Name = issue.Name,
                        EstimateTime = issue.EstimateTime,
                        ResolveAt = issue.ResolveAt,
                        DueDate = issue.DueDate,
                        Assignee = issue.Assignee != null ? new UserViewModel
                        {
                            Id = issue.Assignee.Id,
                            Email = issue.Assignee.Email,
                            Name = issue.Assignee.Name,
                            Username = issue.Assignee.Username
                        } : null!,
                        Priority = new PriorityViewModel
                        {
                            Id = issue.Priority.Id,
                            Description = issue.Priority.Description,
                            Name = issue.Priority.Name,
                            Value = issue.Priority.Value
                        },
                        ProjectId = issue.Project.Id,
                        Reporter = new UserViewModel
                        {
                            Id = issue.Reporter.Id,
                            Email = issue.Reporter.Email,
                            Name = issue.Reporter.Name,
                            Username = issue.Reporter.Username
                        },
                        StatusId = issue.StatusId,
                        Type = new TypeViewModel
                        {
                            Id = issue.Type.Id,
                            Description = issue.Type.Description,
                            Name = issue.Type.Name,
                        },
                        Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                        {
                            Id = issueLabel.Label.Id,
                            Name = issueLabel.Label.Name,
                            ProjectId = issueLabel.Label.ProjectId,
                        }).ToList()
                    }).ToList(),
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<StatusViewModel> UpdateStatus(Guid id, UpdateStatusRequestModel model)
        {
            if (model != null)
            {
                var status = await _statusRepository.GetMany(status => status.Id.Equals(id))
                    .Include(status => status.Project)
                    .FirstOrDefaultAsync();
                if (status != null)
                {
                    if (model.Name != null) status.Name = model.Name;
                    if (model.IsFirst != null) status.IsFirst = (bool)model.IsFirst;
                    if (model.IsLast != null) status.IsLast = (bool)model.IsLast;
                    if (model.Description != null) status.Description = model.Description;
                    if (model.Position != null) status.Position = (int)model.Position;
                    status.Project.LastActivity = DateTime.Now;
                    _statusRepository.Update(status);
                    var result = await _unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return await GetStatus(id);
                    }
                }
            }
            return null!;
        }

        public async Task<StatusViewModel> CreateStatus(CreateStatusRequestModel model)
        {
            if (model != null)
            {
                var id = Guid.NewGuid();
                var status = new Status
                {
                    Id = id,
                    Description = model.Description,
                    Name = model.Name,
                    IsFirst = false,
                    IsLast = false,
                    Position = model.Position,
                    ProjectId = model.ProjectId,
                };
                _statusRepository.Add(status);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    var updateStatus = await _statusRepository.GetMany(status => status.Id.Equals(id))
                        .Include(status => status.Project)
                        .FirstOrDefaultAsync();
                    updateStatus!.Project.LastActivity = DateTime.Now;
                    _statusRepository.Update(updateStatus);
                    return await _unitOfWork.SaveChanges() > 0 ? await GetStatus(id) : null!;
                }
            }
            return null!;
        }

        public async Task<ICollection<StatusViewModel>> UpdateStatuses(ICollection<UpdateStatusRequestModel> models)
        {
            if (models.Count > 0)
            {
                var statuses = new List<Status>();
                foreach (var model in models)
                {
                    var status = await _statusRepository.GetMany(status => status.Id.Equals(model.Id))
                        .Include(status => status.Project)
                        .Include(status => status.Issues).ThenInclude(issue => issue.Project)
                        .Include(status => status.Issues).ThenInclude(issue => issue.Type)
                        .Include(status => status.Issues).ThenInclude(issue => issue.Reporter)
                        .Include(status => status.Issues).ThenInclude(issue => issue.Assignee)
                        .Include(status => status.Issues).ThenInclude(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                        .FirstOrDefaultAsync();
                    if (status != null)
                    {
                        if (model.Name != null) status.Name = model.Name;
                        if (model.IsFirst != null) status.IsFirst = (bool)model.IsFirst;
                        if (model.IsLast != null) status.IsLast = (bool)model.IsLast;
                        if (model.Description != null) status.Description = model.Description;
                        if (model.Position != null) status.Position = (int)model.Position;
                        status.Project.LastActivity = DateTime.Now;
                        statuses.Add(status);
                    }
                }
                _statusRepository.UpdateRange(statuses);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return statuses.OrderBy(status => status.Position).Select(status => new StatusViewModel
                    {
                        Id = status.Id,
                        Description = status.Description,
                        Name = status.Name,
                        Position = status.Position,
                        IsLast = status.IsLast,
                        IsFirst = status.IsFirst,
                        Limit = status.Limit,
                        Issues = status.Issues.OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
                        {
                            Id = issue.Id,
                            CreateAt = issue.CreateAt,
                            UpdateAt = issue.UpdateAt,
                            Description = issue.Description,
                            IsClose = issue.IsClose,
                            IsChild= issue.IsChild,
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
                            Priority = new PriorityViewModel
                            {
                                Id = issue.Priority.Id,
                                Description = issue.Priority.Description,
                                Name = issue.Priority.Name,
                                Value = issue.Priority.Value
                            },
                            ProjectId = issue.Project.Id,
                            Position = issue.Position,
                            Reporter = new UserViewModel
                            {
                                Id = issue.Reporter.Id,
                                Email = issue.Reporter.Email,
                                Name = issue.Reporter.Name,
                                Username = issue.Reporter.Username
                            },
                            StatusId = issue.StatusId,
                            Type = new TypeViewModel
                            {
                                Id = issue.Type.Id,
                                Description = issue.Type.Description,
                                Name = issue.Type.Name,
                            },
                            ResolveAt = issue.ResolveAt,
                            Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                            {
                                Id = issueLabel.Label.Id,
                                Name = issueLabel.Label.Name,
                                ProjectId = issueLabel.Label.ProjectId,
                            }).ToList(),
                        }).ToList(),
                    }).ToList();
                }
            }
            return null!;
        }

        public async Task<bool> DeleteStatus(Guid id, Guid inheritanceId)
        {
            var status = await _statusRepository.GetMany(status => status.Id.Equals(id))
                .Include(status => status.Project)
                .Include(status => status.Issues)
                .FirstOrDefaultAsync();
            if (status != null)
            {
                if (status.IsLast)
                {
                    var inheritanceStatus = await _statusRepository.GetMany(status => status.Id.Equals(inheritanceId)).FirstOrDefaultAsync();
                    if (inheritanceStatus != null)
                    {
                        inheritanceStatus.IsLast = true;
                        _statusRepository.Update(inheritanceStatus);
                    }
                }
                var issues = new List<Issue>();
                foreach (var issue in status.Issues)
                {
                    issue.StatusId = inheritanceId;
                }
                status.Project.LastActivity = DateTime.Now;
                _issueRepository.UpdateRange(issues);
                _statusRepository.Remove(status);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
