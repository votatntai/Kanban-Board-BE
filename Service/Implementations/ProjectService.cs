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
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _projectRepository = unitOfWork.Project;
        }

        public async Task<ProjectViewModel> CreateProject(CreateProjectRequestModel model, Guid leaderId)
        {
            var id = Guid.NewGuid();
            var project = new Project
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CreateAt = DateTime.Now,
                IsClose = false,
                LeaderId = leaderId,
            };
            _projectRepository.Add(project);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetProject(id);
            }
            return null!;
        }

        public async Task<bool> DeleteProject(Guid id)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(project => project.Id.Equals(id));
            if (project != null)
            {
                _projectRepository.Remove(project);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ProjectViewModel> GetProject(Guid id)
        {
            return await _projectRepository.GetMany(project => project.Id.Equals(id))
                .Include(project => project.Statuses).ThenInclude(status => status.Issues)
                .Select(project => new ProjectViewModel
            {
                Id = project.Id,
                CreateAt = project.CreateAt,
                UpdateAt = project.UpdateAt,
                Description = project.Description,
                IsClose = project.IsClose,
                Name = project.Name,
                Leader = new UserViewModel
                {
                    Id = project.Leader.Id,
                    Name = project.Leader.Name,
                    Username = project.Leader.Username,
                    Email = project.Leader.Email,
                },
                DefaultAssignee = project.DefaultAssignee != null ? new UserViewModel
                {
                    Id = project.DefaultAssignee.Id,
                    Name = project.DefaultAssignee.Name,
                    Username = project.DefaultAssignee.Username,
                    Email = project.DefaultAssignee.Email,
                } : null!,
                Statuses = project.Statuses.Select(status => new StatusViewModel
                {
                    Id = status.Id,
                    Description= status.Description,
                    Name= status.Name,
                    Position = status.Position,
                    Issues = status.Issues.Select(issue => new IssueViewModel
                    {
                        Id = issue.Id,
                        Name= issue.Name,
                        Description= issue.Description,
                        CreateAt= issue.CreateAt,
                        DueDate= issue.DueDate,
                        EstimateTime= issue.EstimateTime,
                        IsClose= issue.IsClose,
                        UpdateAt= issue.UpdateAt,
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
                    }).OrderBy(issue => issue.Position).ToList(),
                }).OrderBy(status => status.Position).ToList(),
                    Members = project.Users.Select(user => new UserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Username = user.Username
                    }).ToList()
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<ProjectViewModel>> GetProjects(string? name)
        {
            return await _projectRepository
                .GetMany(project => project.Name.Contains(name ?? ""))
                .Include(project => project.Statuses).ThenInclude(status => status.Issues)
                .ThenInclude(issue => issue.Project)
                .Select(project => new ProjectViewModel
            {
                Id = project.Id,
                CreateAt = project.CreateAt,
                UpdateAt = project.UpdateAt,
                Description = project.Description,
                IsClose = project.IsClose,
                Name = project.Name,
                Leader = new UserViewModel
                {
                    Id = project.Leader.Id,
                    Name = project.Leader.Name,
                    Username = project.Leader.Username,
                    Email = project.Leader.Email,
                },
                DefaultAssignee = project.DefaultAssignee != null ? new UserViewModel
                {
                    Id = project.DefaultAssignee.Id,
                    Name = project.DefaultAssignee.Name,
                    Username = project.DefaultAssignee.Username,
                    Email = project.DefaultAssignee.Email,
                } : null!,
                Statuses = project.Statuses.Select(status => new StatusViewModel
                {
                    Id = status.Id,
                    Description = status.Description,
                    Name = status.Name,
                    Issues = status.Issues.Select(issue => new IssueViewModel
                    {
                        Id = issue.Id,
                        Name = issue.Name,
                        Description = issue.Description,
                        CreateAt = issue.CreateAt,
                        DueDate = issue.DueDate,
                        EstimateTime = issue.EstimateTime,
                        IsClose = issue.IsClose,
                        UpdateAt = issue.UpdateAt,
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
                    }).OrderBy(issue => issue.Position).ToList(),
                }).OrderBy(status => status.Position).ToList(),
                    Members = project.Users.Select(user => new UserViewModel
                {
                    Id= user.Id,
                    Email= user.Email,
                    Name = user.Name,
                    Username = user.Username
                }).ToList()
            }).ToListAsync();
        }

        public async Task<ProjectViewModel> UpdateProject(Guid id, UpdateProjectRequestModel model)
        {
            var project = await _projectRepository.GetMany(project => project.Id.Equals(id)).FirstOrDefaultAsync();
            if (project != null)
            {
                if (model.Name != null) project.Name = model.Name;
                if (model.Description != null) project.Description = model.Description;
                if (model.DefaultAssigneeId != null) project.DefaultAssigneeId = model.DefaultAssigneeId;
                if (model.LeaderId != null) project.LeaderId = (Guid)model.LeaderId;
                if (model.IsClose != null) project.IsClose = (bool)model.IsClose;
                project.UpdateAt = DateTime.UtcNow;
                _projectRepository.Update(project);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetProject(id);
                }
            }
            return null!;
        }
    }
}
