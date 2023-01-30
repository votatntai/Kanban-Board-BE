using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Type = Data.Entities.Type;

namespace Service.Implementations
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _projectRepository = unitOfWork.Project;
            _priorityRepository = unitOfWork.Priority;
            _projectMemberRepository = unitOfWork.ProjectMember;
        }

        public async Task<ProjectViewModel> CreateProject(CreateProjectRequestModel model, Guid leaderId)
        {
            var id = Guid.NewGuid();
            var defaultStatuses = new List<Status>
            {
                new Status
                {
                    Id = Guid.NewGuid(),
                    Name = "To Do",
                    Description = "To do task",
                    IsFirst = true,
                    IsLast = false,
                    Position = 65536,
                    ProjectId = id
                },
                new Status
                {
                    Id = Guid.NewGuid(),
                    Name = "Processing",
                    Description = "Processing task",
                    IsFirst = false,
                    IsLast = false,
                    Position = 131072,
                    ProjectId = id
                },
                new Status
                {
                    Id = Guid.NewGuid(),
                    Name = "Done",
                    Description = "Done task",
                    IsFirst = false,
                    IsLast = true,
                    Position = 196608,
                    ProjectId = id
                }
            };
            var defaultPriority = await _priorityRepository.GetAll().ToListAsync();
            var defaultType = new List<Type>
            {
                new Type{
                    Id = Guid.NewGuid(),
                    Name = "Bug",
                    ProjectId= id,
                    Description = "Bug"
                },
               new Type{
                    Id = Guid.NewGuid(),
                    Name = "Task",
                    ProjectId= id,
                    Description = "Task"
                },
                new Type{
                    Id = Guid.NewGuid(),
                    Name = "SubTask",
                    ProjectId= id,
                    Description = "Sub Task"
                },
                new Type{
                    Id = Guid.NewGuid(),
                    Name = "Story",
                    ProjectId= id,
                    Description = "Story"
                }
            };
            var project = new Project
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CreateAt = DateTime.Now,
                Statuses = defaultStatuses,
                Types = defaultType,
                ProjectPriorities = defaultPriority.Select(priority => new ProjectPriority
                {
                    ProjectId = id,
                    PriorityId = priority.Id,
                    Description = priority.Description,
                }).ToList(),
                IsClose = false,
                LeaderId = leaderId,
                ProjectMembers = new List<ProjectMember>
                {
                    new ProjectMember
                    {
                        ProjectId= id,
                        UserId = leaderId,
                        IsOwner= true,
                        JoinAt = DateTime.Now
                    }
                },
                LastActivity = DateTime.Now,
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

        public async Task<MemberViewModel> AddMember(Guid projectId, Guid memberId)
        {
            var projectMember = new ProjectMember
            {
                ProjectId = projectId,
                UserId = memberId,
                IsOwner = false,
                JoinAt= DateTime.Now
            };
            _projectMemberRepository.Add(projectMember);
            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                return await _projectMemberRepository.GetMany(projectMember => 
                projectMember.ProjectId.Equals(projectId) &&
                projectMember.UserId.Equals(memberId)).Select(member => new MemberViewModel
                {
                    Id = member.User.Id,
                    Email = member.User.Email,
                    Name= member.User.Name,
                    Username= member.User.Username,
                    IsOwner= member.IsOwner,
                    JoinAt = member.JoinAt,
                }) .FirstOrDefaultAsync() ?? null!;
            }
            return null!;
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
                    Priorities = project.ProjectPriorities.OrderBy(priority => priority.Priority.Value).Select(projectPriority => new PriorityViewModel
                    {
                        Id = projectPriority.Priority.Id,
                        Description = projectPriority.Priority.Description,
                        Name = projectPriority.Priority.Name
                    }).ToList(),
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
                        IsFirst = status.IsFirst,
                        IsLast = status.IsLast,
                        Limit = status.Limit,
                        Position = status.Position,
                        Issues = status.Issues.Select(issue => new IssueViewModel
                        {
                            Id = issue.Id,
                            Name = issue.Name,
                            Description = issue.Description,
                            CreateAt = issue.CreateAt,
                            DueDate = issue.DueDate,
                            ChildIssues = issue.InverseParent.Select(issue => new ChildIssueViewModel
                            {
                                Id = issue.Id,
                                Name = issue.Name,
                                Description = issue.Description,
                                CreateAt = issue.CreateAt,
                                IsClose = issue.IsClose,
                                IsChild = issue.IsChild,
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
                                ResolveAt = issue.ResolveAt,
                                Reporter = new UserViewModel
                                {
                                    Id = issue.Reporter.Id,
                                    Email = issue.Reporter.Email,
                                    Name = issue.Reporter.Name,
                                    Username = issue.Reporter.Username
                                },
                                StatusId = issue.StatusId,
                                TypeId = issue.TypeId,
                                Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                                {
                                    Id = issueLabel.Label.Id,
                                    Name = issueLabel.Label.Name,
                                    ProjectId = issueLabel.Label.ProjectId,
                                }).ToList()
                            }).ToList(),
                            EstimateTime = issue.EstimateTime,
                            IsClose = issue.IsClose,
                            IsChild = issue.IsChild,
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
                            ResolveAt = issue.ResolveAt,
                            Reporter = new UserViewModel
                            {
                                Id = issue.Reporter.Id,
                                Email = issue.Reporter.Email,
                                Name = issue.Reporter.Name,
                                Username = issue.Reporter.Username
                            },
                            StatusId = issue.StatusId,
                            TypeId = issue.TypeId,
                            Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                            {
                                Id = issueLabel.Label.Id,
                                Name = issueLabel.Label.Name,
                                ProjectId = issueLabel.Label.ProjectId,
                            }).ToList()
                        }).OrderBy(issue => issue.Position).ToList(),
                    }).OrderBy(status => status.Position).ToList(),
                    Members = project.ProjectMembers.Select(projectMember => new MemberViewModel
                    {
                        Id = projectMember.User.Id,
                        Email = projectMember.User.Email,
                        Name = projectMember.User.Name,
                        Username = projectMember.User.Username,
                        IsOwner = projectMember.IsOwner,
                        JoinAt = projectMember.JoinAt
                    }).ToList(),
                    Labels = project.Labels.Select(label => new LabelViewModel
                    {
                        Id = label.Id,
                        Name = label.Name,
                        ProjectId = label.ProjectId,
                    }).ToList(),
                    LastActivity = project.LastActivity,
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<ProjectViewModel>> GetProjects(string? name)
        {
            return await _projectRepository
                .GetMany(project => project.Name.Contains(name ?? ""))
                .Include(project => project.Statuses).ThenInclude(status => status.Issues)
                .ThenInclude(issue => issue.Project)
                .Include(project => project.Issues).ThenInclude(issue => issue.IssueLabels)
                .ThenInclude(issueLabel => issueLabel.Label)
                .OrderByDescending(project => project.LastActivity)
                .Select(project => new ProjectViewModel
                {
                    Id = project.Id,
                    CreateAt = project.CreateAt,
                    UpdateAt = project.UpdateAt,
                    Description = project.Description,
                    IsClose = project.IsClose,
                    Name = project.Name,
                    Priorities = project.ProjectPriorities.OrderBy(priority => priority.Priority.Value).Select(projectPriority => new PriorityViewModel
                    {
                        Id = projectPriority.Priority.Id,
                        Description = projectPriority.Priority.Description,
                        Name = projectPriority.Priority.Name
                    }).ToList(),
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
                    Statuses = project.Statuses.OrderBy(status => status.Position).Select(status => new StatusViewModel
                    {
                        Id = status.Id,
                        Description = status.Description,
                        Name = status.Name,
                        Position = status.Position,
                        IsFirst = status.IsFirst,
                        IsLast = status.IsLast,
                        Limit = status.Limit,
                        Issues = status.Issues.OrderBy(issue => issue.Position).Select(issue => new IssueViewModel
                        {
                            Id = issue.Id,
                            Name = issue.Name,
                            Description = issue.Description,
                            CreateAt = issue.CreateAt,
                            DueDate = issue.DueDate,
                            EstimateTime = issue.EstimateTime,
                            IsClose = issue.IsClose,
                            IsChild = issue.IsChild,
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
                            ResolveAt = issue.ResolveAt,
                            Labels = issue.IssueLabels.Select(issueLabel => new LabelViewModel
                            {
                                Id = issueLabel.Label.Id,
                                Name = issueLabel.Label.Name,
                                ProjectId = issueLabel.Label.ProjectId,
                            }).ToList()
                        }).ToList(),
                    }).ToList(),
                    Members = project.ProjectMembers.Select(projectMember => new MemberViewModel
                    {
                        Id = projectMember.User.Id,
                        Email = projectMember.User.Email,
                        Name = projectMember.User.Name,
                        Username = projectMember.User.Username,
                        IsOwner = projectMember.IsOwner,
                        JoinAt = projectMember.JoinAt
                    }).ToList(),
                    LastActivity = project.LastActivity,
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
