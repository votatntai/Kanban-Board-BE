using Data.Models.Requests.Create;
using Data.Models.Views;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces
{
    public interface IAttachmentService
    {
        Task<AttachmentViewModel> SaveFile(Guid issueId, IFormFile file);
        Task<byte[]> GetFile(Guid id);
    }
}
