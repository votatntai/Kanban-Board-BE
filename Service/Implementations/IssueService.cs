using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
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
        private readonly ICommentRepository _commentRepository;
        private readonly ILogWorkRepository _logWorkRepository;

        private readonly IIssueLabelRepository _issueLabelRepository;

        public IssueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _commentRepository = unitOfWork.Comment;
            _logWorkRepository = unitOfWork.LogWork;
            _issueRepository = unitOfWork.Issue;
            _projectRepository = unitOfWork.Project;
            _priorityRepository = unitOfWork.Priority;
            _issueLabelRepository = unitOfWork.IssueLabel;
            _statusRepository = unitOfWork.Status;
        }

        public async Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueRequestModel model)
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
                ParentId = model.ParentId,
                AssigneeId = model.AssigneeId != null ? model.AssigneeId : null!,
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
                return await GetChildIssue(id);
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
                    Attachments = issue.Attachments.Select(attachment => new AttachmentViewModel
                    {
                        Id = attachment.Id,
                        Url = attachment.Url,
                        IssueId = attachment.Id,
                        Name = attachment.Name,
                    }).ToList(),
                    LogWorks = issue.LogWorks.Select(logWork => new LogWorkViewModel
                    {
                        Id = logWork.Id,
                        Description = logWork.Description,
                        IssueId = logWork.IssueId,
                        RemainingTime = logWork.RemainingTime,
                        SpentTime = logWork.SpentTime,
                        User = new UserViewModel
                        {
                            Id = logWork.User.Id,
                            Email = logWork.User.Email,
                            Name = logWork.User.Name,
                            Username = logWork.User.Username
                        },
                        CreateAt = logWork.CreateAt,
                    }).ToList(),
                    ChildIssues = issue.InverseParent.Select(child => new ChildIssueViewModel
                    {
                        Id = child.Id,
                        CreateAt = child.CreateAt,
                        UpdateAt = child.UpdateAt,
                        Position = child.Position,
                        Description = child.Description,
                        IsClose = child.IsClose,
                        IsChild = child.IsChild,
                        Name = child.Name,
                        Attachments = child.Attachments.Select(attachment => new AttachmentViewModel
                        {
                            Id = attachment.Id,
                            Url = attachment.Url,
                            IssueId = attachment.Id,
                            Name = attachment.Name,
                        }).ToList(),
                        Links = child.Links.Select(link => new LinkViewModel
                        {
                            Id = link.Id,
                            Description = link.Description!,
                            IssueId = link.IssueId,
                            Url = link.Url
                        }).ToList(),
                        Comments = child.Comments.Select(comment => new CommentViewModel
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreateAt = comment.CreateAt,
                            User = new UserViewModel
                            {
                                Id = comment.User.Id,
                                Email = comment.User.Email,
                                Name = comment.User.Name,
                                Username = comment.User.Username
                            },
                            IssueId = comment.IssueId
                        }).ToList(),
                        Assignee = child.Assignee != null ? new UserViewModel
                        {
                            Id = child.Assignee.Id,
                            Email = child.Assignee.Email,
                            Name = child.Assignee.Name,
                            Username = child.Assignee.Username
                        } : null!,
                        ProjectId = child.Project.Id,
                        Reporter = new UserViewModel
                        {
                            Id = child.Reporter.Id,
                            Email = child.Reporter.Email,
                            Name = child.Reporter.Name,
                            Username = child.Reporter.Username
                        },
                        Priority = new PriorityViewModel
                        {
                            Id = child.Priority.Id,
                            Description = child.Priority.Description,
                            Name = child.Priority.Name,
                            Value = child.Priority.Value
                        },
                        Status = new StatusViewModel
                        {
                            Id = child.Status.Id,
                            Name = child.Status.Name,
                            Description = child.Status.Description,
                            IsFirst = child.Status.IsFirst,
                            IsLast = child.Status.IsLast,
                            Limit = child.Status.Limit,
                            Position = child.Status.Position
                        },
                        Type = new TypeViewModel
                        {
                            Id = child.Type.Id,
                            Description = child.Type.Description,
                            Name = child.Type.Name,
                        },
                        ResolveAt = child.ResolveAt,
                        Labels = child.IssueLabels.Select(issueLabel => new LabelViewModel
                        {
                            Id = issueLabel.Label.Id,
                            Name = issueLabel.Label.Name,
                            ProjectId = issueLabel.Label.ProjectId,
                        }).ToList(),
                    }).ToList(),
                    Links = issue.Links.Select(link => new LinkViewModel
                    {
                        Id = link.Id,
                        Description = link.Description!,
                        IssueId = link.IssueId,
                        Url = link.Url
                    }).ToList(),
                    Comments = issue.Comments.Select(comment => new CommentViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        CreateAt = comment.CreateAt,
                        User = new UserViewModel
                        {
                            Id = comment.User.Id,
                            Email = comment.User.Email,
                            Name = comment.User.Name,
                            Username = comment.User.Username
                        },
                        IssueId = comment.IssueId
                    }).ToList(),
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
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ChildIssueViewModel> GetChildIssue(Guid id)
        {
            return await _issueRepository.GetMany(issue => issue.Id.Equals(id)).Select(child => new ChildIssueViewModel
            {
                Id = child.Id,
                CreateAt = child.CreateAt,
                UpdateAt = child.UpdateAt,
                Position = child.Position,
                Description = child.Description,
                IsClose = child.IsClose,
                IsChild = child.IsChild,
                Name = child.Name,
                Attachments = child.Attachments.Select(attachment => new AttachmentViewModel
                {
                    Id = attachment.Id,
                    Url = attachment.Url,
                    IssueId = attachment.Id,
                    Name = attachment.Name,
                }).ToList(),
                Comments = child.Comments.Select(comment => new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreateAt = comment.CreateAt,
                    User = new UserViewModel
                    {
                        Id = comment.User.Id,
                        Email = comment.User.Email,
                        Name = comment.User.Name,
                        Username = comment.User.Username
                    },
                    IssueId = comment.IssueId
                }).ToList(),
                Assignee = child.Assignee != null ? new UserViewModel
                {
                    Id = child.Assignee.Id,
                    Email = child.Assignee.Email,
                    Name = child.Assignee.Name,
                    Username = child.Assignee.Username
                } : null!,
                Links = child.Links.Select(link => new LinkViewModel
                {
                    Id = link.Id,
                    Description = link.Description!,
                    IssueId = link.IssueId,
                    Url = link.Url
                }).ToList(),
                ProjectId = child.Project.Id,
                Reporter = new UserViewModel
                {
                    Id = child.Reporter.Id,
                    Email = child.Reporter.Email,
                    Name = child.Reporter.Name,
                    Username = child.Reporter.Username
                },
                Priority = new PriorityViewModel
                {
                    Id = child.Priority.Id,
                    Description = child.Priority.Description,
                    Name = child.Priority.Name,
                    Value = child.Priority.Value
                },
                Status = new StatusViewModel
                {
                    Id = child.Status.Id,
                    Name = child.Status.Name,
                    Description = child.Status.Description,
                    IsFirst = child.Status.IsFirst,
                    IsLast = child.Status.IsLast,
                    Limit = child.Status.Limit,
                    Position = child.Status.Position
                },
                Type = new TypeViewModel
                {
                    Id = child.Type.Id,
                    Description = child.Type.Description,
                    Name = child.Type.Name,
                },
                ResolveAt = child.ResolveAt,
                Labels = child.IssueLabels.Select(issueLabel => new LabelViewModel
                {
                    Id = issueLabel.Label.Id,
                    Name = issueLabel.Label.Name,
                    ProjectId = issueLabel.Label.ProjectId,
                }).ToList(),
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
                    Attachments = issue.Attachments.Select(attachment => new AttachmentViewModel
                    {
                        Id = attachment.Id,
                        Url = attachment.Url,
                        IssueId = attachment.Id,
                        Name = attachment.Name,
                    }).ToList(),
                    EstimateTime = issue.EstimateTime,
                    DueDate = issue.DueDate,
                    ChildIssues = issue.InverseParent.Select(child => new ChildIssueViewModel
                    {
                        Id = child.Id,
                        CreateAt = child.CreateAt,
                        UpdateAt = child.UpdateAt,
                        Position = child.Position,
                        Attachments = child.Attachments.Select(attachment => new AttachmentViewModel
                        {
                            Id = attachment.Id,
                            Url = attachment.Url,
                            IssueId = attachment.Id,
                            Name = attachment.Name,
                        }).ToList(),
                        Links = child.Links.Select(link => new LinkViewModel
                        {
                            Id = link.Id,
                            Description = link.Description!,
                            IssueId = link.IssueId,
                            Url = link.Url
                        }).ToList(),
                        Description = child.Description,
                        IsClose = child.IsClose,
                        IsChild = child.IsChild,
                        Name = child.Name,
                        Comments = child.Comments.Select(comment => new CommentViewModel
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreateAt = comment.CreateAt,
                            User = new UserViewModel
                            {
                                Id = comment.User.Id,
                                Email = comment.User.Email,
                                Name = comment.User.Name,
                                Username = comment.User.Username
                            },
                            IssueId = comment.IssueId
                        }).ToList(),
                        Assignee = child.Assignee != null ? new UserViewModel
                        {
                            Id = child.Assignee.Id,
                            Email = child.Assignee.Email,
                            Name = child.Assignee.Name,
                            Username = child.Assignee.Username
                        } : null!,
                        ProjectId = child.Project.Id,
                        Reporter = new UserViewModel
                        {
                            Id = child.Reporter.Id,
                            Email = child.Reporter.Email,
                            Name = child.Reporter.Name,
                            Username = child.Reporter.Username
                        },
                        Priority = new PriorityViewModel
                        {
                            Id = child.Priority.Id,
                            Description = child.Priority.Description,
                            Name = child.Priority.Name,
                            Value = child.Priority.Value
                        },
                        Status = new StatusViewModel
                        {
                            Id = child.Status.Id,
                            Name = child.Status.Name,
                            Description = child.Status.Description,
                            IsFirst = child.Status.IsFirst,
                            IsLast = child.Status.IsLast,
                            Limit = child.Status.Limit,
                            Position = child.Status.Position
                        },
                        Type = new TypeViewModel
                        {
                            Id = child.Type.Id,
                            Description = child.Type.Description,
                            Name = child.Type.Name,
                        },
                        ResolveAt = child.ResolveAt,
                        Labels = child.IssueLabels.Select(issueLabel => new LabelViewModel
                        {
                            Id = issueLabel.Label.Id,
                            Name = issueLabel.Label.Name,
                            ProjectId = issueLabel.Label.ProjectId,
                        }).ToList(),
                    }).ToList(),
                    Comments = issue.Comments.Select(comment => new CommentViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        CreateAt = comment.CreateAt,
                        User = new UserViewModel
                        {
                            Id = comment.User.Id,
                            Email = comment.User.Email,
                            Name = comment.User.Name,
                            Username = comment.User.Username
                        },
                        IssueId = comment.IssueId
                    }).ToList(),
                    LogWorks = issue.LogWorks.Select(logWork => new LogWorkViewModel
                    {
                        Id = logWork.Id,
                        Description = logWork.Description,
                        IssueId = logWork.IssueId,
                        RemainingTime = logWork.RemainingTime,
                        SpentTime = logWork.SpentTime,
                        CreateAt = logWork.CreateAt,
                        User = new UserViewModel
                        {
                            Id = logWork.User.Id,
                            Email = logWork.User.Email,
                            Name = logWork.User.Name,
                            Username = logWork.User.Username
                        },
                    }).ToList(),
                    Links = issue.Links.Select(link => new LinkViewModel
                    {
                        Id = link.Id,
                        Description = link.Description!,
                        IssueId = link.IssueId,
                        Url = link.Url
                    }).ToList(),
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
                    Type = new TypeViewModel
                    {
                        Id = issue.Type.Id,
                        Description = issue.Type.Description,
                        Name = issue.Type.Name,
                    },
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
                    Type = new TypeViewModel
                    {
                        Id = issue.Type.Id,
                        Description = issue.Type.Description,
                        Name = issue.Type.Name,
                    },
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
                issue.EstimateTime = model.EstimateTime;
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

        public async Task<ChildIssueViewModel> UpdateChildIssue(Guid id, UpdateChildIssueRequestModel model)
        {
            var issue = await _issueRepository.GetMany(issue => issue.Id.Equals(id))
                .Include(issue => issue.Project)
                .FirstOrDefaultAsync();
            if (issue != null)
            {
                if (model.Name != null) issue.Name = model.Name;
                if (model.Description != null) issue.Description = model.Description;
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
                issue.AssigneeId = model.AssigneeId;
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
                    return await GetChildIssue(id);
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
                        .Include(issue => issue.Type)
                        .Include(issue => issue.Status)
                        .Include(issue => issue.Priority)
                        .Include(issue => issue.Reporter)
                        .Include(issue => issue.LogWorks).ThenInclude(logWork => logWork.User)
                        .Include(issue => issue.Comments).ThenInclude(comment => comment.User)
                        .Include(issue => issue.Assignee).ThenInclude(user => user!.Roles)
                        .Include(issue => issue.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
                        .Include(issues => issues.Attachments)

                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Project)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Type)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Reporter)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Priority)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Status)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Attachments)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.LogWorks).ThenInclude(logWork => logWork.User)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Comments).ThenInclude(comment => comment.User)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.Assignee).ThenInclude(user => user!.Roles)
                        .Include(issue => issue.InverseParent).ThenInclude(parent => parent.IssueLabels).ThenInclude(issueLabel => issueLabel.Label)
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
                        ChildIssues = issue.InverseParent.Select(child => new ChildIssueViewModel
                        {
                            Id = child.Id,
                            CreateAt = child.CreateAt,
                            UpdateAt = child.UpdateAt,
                            Position = child.Position,
                            Description = child.Description,
                            IsClose = child.IsClose,
                            Links = child.Links.Select(link => new LinkViewModel
                            {
                                Description = link.Description,
                                Id = link.Id,
                                IssueId = link.Id,
                                Url = link.Url,
                            }).ToList(),
                            Attachments = child.Attachments.Select(attachment => new AttachmentViewModel
                            {
                                Id= attachment.Id,
                                Url= attachment.Url,
                                IssueId= attachment.Id,
                                Name= attachment.Name,
                            }).ToList(),
                            IsChild = child.IsChild,
                            Name = child.Name,
                            Comments = child.Comments.Select(comment => new CommentViewModel
                            {
                                Id = comment.Id,
                                Content = comment.Content,
                                CreateAt = comment.CreateAt,
                                User = new UserViewModel
                                {
                                    Id = comment.User.Id,
                                    Email = comment.User.Email,
                                    Name = comment.User.Name,
                                    Username = comment.User.Username
                                },
                                IssueId = comment.IssueId
                            }).ToList(),
                            Assignee = child.Assignee != null ? new UserViewModel
                            {
                                Id = child.Assignee.Id,
                                Email = child.Assignee.Email,
                                Name = child.Assignee.Name,
                                Username = child.Assignee.Username
                            } : null!,
                            ProjectId = child.Project.Id,
                            Reporter = new UserViewModel
                            {
                                Id = child.Reporter.Id,
                                Email = child.Reporter.Email,
                                Name = child.Reporter.Name,
                                Username = child.Reporter.Username
                            },
                            Priority = new PriorityViewModel
                            {
                                Id = child.Priority.Id,
                                Description = child.Priority.Description,
                                Name = child.Priority.Name,
                                Value = child.Priority.Value
                            },
                            Status = new StatusViewModel
                            {
                                Id = child.Status.Id,
                                Name = child.Status.Name,
                                Description = child.Status.Description,
                                IsFirst = child.Status.IsFirst,
                                IsLast = child.Status.IsLast,
                                Limit = child.Status.Limit,
                                Position = child.Status.Position
                            },
                            Type = new TypeViewModel
                            {
                                Id = child.Type.Id,
                                Description = child.Type.Description,
                                Name = child.Type.Name,
                            },
                            ResolveAt = child.ResolveAt,
                            Labels = child.IssueLabels.Select(issueLabel => new LabelViewModel
                            {
                                Id = issueLabel.Label.Id,
                                Name = issueLabel.Label.Name,
                                ProjectId = issueLabel.Label.ProjectId,
                            }).ToList(),
                        }).ToList(),
                        Comments = issue.Comments.Select(comment => new CommentViewModel
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreateAt = comment.CreateAt,
                            User = new UserViewModel
                            {
                                Id = comment.User.Id,
                                Email = comment.User.Email,
                                Name = comment.User.Name,
                                Username = comment.User.Username
                            },
                            IssueId = comment.IssueId
                        }).ToList(),
                        LogWorks = issue.LogWorks.Select(logWork => new LogWorkViewModel
                        {
                            Id = logWork.Id,
                            Description = logWork.Description,
                            IssueId = logWork.IssueId,
                            RemainingTime = logWork.RemainingTime,
                            SpentTime = logWork.SpentTime,
                            CreateAt = logWork.CreateAt,
                            User = new UserViewModel
                            {
                                Id = logWork.User.Id,
                                Email = logWork.User.Email,
                                Name = logWork.User.Name,
                                Username = logWork.User.Username
                            },
                        }).ToList(),
                        Links = issue.Links.Select(link => new LinkViewModel
                        {
                            Id = link.Id,
                            Description = link.Description!,
                            IssueId = link.IssueId,
                            Url = link.Url
                        }).ToList(),
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
                    .Include(childIssue => childIssue.IssueLabels)
                    .Include(childIssue => childIssue.Comments)
                    .ToListAsync();

                issue.Project.LastActivity = DateTime.Now;
                foreach (var childIssue in childIssues)
                {
                    _issueLabelRepository.RemoveRange(childIssue.IssueLabels);
                    _commentRepository.RemoveRange(childIssue.Comments);
                }

                var comments = await _commentRepository.GetMany(comment => comment.IssueId.Equals(issue.Id)).ToListAsync();
                var logWorks = await _logWorkRepository.GetMany(logWork => logWork.IssueId.Equals(issue.Id)).ToListAsync();

                _issueLabelRepository.RemoveRange(issue.IssueLabels);
                _issueRepository.Update(issue);
                _issueRepository.RemoveRange(childIssues);
                _commentRepository.RemoveRange(comments);
                _logWorkRepository.RemoveRange(logWorks);
                _issueRepository.Remove(issue);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }

    }
}
