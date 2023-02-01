using Data.Models.Requests.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentViewModel> GetComment(Guid id);
        Task<CommentViewModel> CreateComment(CreateCommentRequestModel model);
        Task<bool> DeleteComment(Guid id);
    }
}
