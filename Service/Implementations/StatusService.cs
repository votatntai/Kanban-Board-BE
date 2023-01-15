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
    public class StatusService : BaseService, IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IIssueRepository _issueRepository;

        public StatusService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _statusRepository = unitOfWork.Status;
            _issueRepository = unitOfWork.Issue;
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
                    Issues = status.Issues.OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
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
                    }).ToList(),
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<StatusViewModel> UpdateStatus(Guid id, UpdateStatusRequestModel model)
        {
            if (model != null)
            {
                var status = await _statusRepository.GetMany(status => status.Id.Equals(id)).FirstOrDefaultAsync();
                if (status != null)
                {
                    if (model.Name != null) status.Name = model.Name;
                    if (model.IsFirst != null) status.IsFirst = (bool)model.IsFirst;
                    if (model.IsLast != null) status.IsLast = (bool)model.IsLast;
                    if (model.Description != null) status.Description = model.Description;
                    if (model.Position != null) status.Position = (int)model.Position;
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
                    return await GetStatus(id);
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
                        .Include(status => status.Issues).ThenInclude(issue => issue.Project)
                        .FirstOrDefaultAsync();
                    if (status != null)
                    {
                        if (model.Name != null) status.Name = model.Name;
                        if (model.IsFirst != null) status.IsFirst = (bool)model.IsFirst;
                        if (model.IsLast != null) status.IsLast = (bool)model.IsLast;
                        if (model.Description != null) status.Description = model.Description;
                        if (model.Position != null) status.Position = (int)model.Position;
                        statuses.Add(status);
                    }
                }
                _statusRepository.UpdateRange(statuses);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return statuses.OrderBy(issue => issue.Position).Select(status => new StatusViewModel
                    {
                        Id = status.Id,
                        Description = status.Description,
                        Name = status.Name,
                        Position = status.Position,
                        Issues = status.Issues.Select(issue => new IssueViewModel
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
                            Position = issue.Position,
                            Reporter = new UserViewModel
                            {
                                Id = issue.Reporter.Id,
                                Email = issue.Reporter.Email,
                                Name = issue.Reporter.Name,
                                Username = issue.Reporter.Username
                            },
                            StatusId = issue.StatusId,
                            TypeId = issue.TypeId,
                        }).ToList(),
                    }).ToList();
                }
            }
            return null!;
        }

        public async Task<bool> DeleteStatus(Guid id, Guid inheritanceId)
        {
            var status = await _statusRepository.GetMany(status => status.Id.Equals(id))
                .Include(status => status.Issues)
                .FirstOrDefaultAsync();
            if (status != null)
            {
                var issues = new List<Issue>();
                foreach (var issue in status.Issues)
                {
                    issue.StatusId = inheritanceId;
                }
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
