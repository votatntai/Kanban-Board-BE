using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _commentRepository = unitOfWork.Comment;
        }

        public async Task<CommentViewModel> GetComment(Guid id)
        {
            return await _commentRepository.GetMany(comment => comment.Id.Equals(id))
                .Select(comment => new CommentViewModel
                {
                    Id = comment.Id,
                    IssueId = comment.IssueId,
                    Content = comment.Content,
                    CreateAt = DateTime.Now,
                    User = new UserViewModel
                    {
                        Id = comment.User.Id,
                        Email = comment.User.Email,
                        Name = comment.User.Name,
                        Username = comment.User.Username
                    }
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CommentViewModel> CreateComment(CreateCommentRequestModel model)
        {
            var id = Guid.NewGuid();
            var comment = new Comment
            {
                Id = id,
                Content = model.Content,
                CreateAt = DateTime.Now,
                IssueId = model.IssueId,
                UserId = model.UserId,
            };
            _commentRepository.Add(comment);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                var commentProject = await _commentRepository.GetMany(comment => comment.Id.Equals(id))
                    .Include(comment => comment.Issue).ThenInclude(issue => issue.Project).FirstOrDefaultAsync();
                if (commentProject != null)
                {
                    commentProject.Issue.Project.LastActivity = DateTime.Now;
                    _commentRepository.Update(commentProject);
                    var update = await _unitOfWork.SaveChanges();
                    if (update > 0)
                    {
                        return await GetComment(id);
                    }
                }
            }
            return null!;
        }

        public async Task<bool> DeleteComment(Guid id)
        {
            var result = false;
            var comment = await _commentRepository.GetMany(comment => comment.Id.Equals(id))
                    .Include(comment => comment.Issue).ThenInclude(issue => issue.Project).FirstOrDefaultAsync();
            if (comment != null)
            {
                comment.Issue.Project.LastActivity = DateTime.Now;
                _commentRepository.Update(comment);
                _commentRepository.Remove(comment);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }
    }
}
