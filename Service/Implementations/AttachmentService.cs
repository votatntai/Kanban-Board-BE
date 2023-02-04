using Data;
using Data.Entities;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class AttachmentService : BaseService, IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _attachmentRepository = unitOfWork.Attachment;
        }

        public async Task<AttachmentViewModel> SaveFile(Guid issueId, IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", file.FileName);
            var id = Guid.NewGuid();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileEntity = new Attachment
            {
                Id = id,
                Name = file.FileName,
                Url = filePath,
                IssueId = issueId,
            };

            _attachmentRepository.Add(fileEntity);
            await _unitOfWork.SaveChanges();

            return await GetAttachment(id);
        }

        private async Task<AttachmentViewModel> GetAttachment(Guid id)
        {
            return await _attachmentRepository.GetMany(attachment => attachment.Id.Equals(id)).Select(attachment => new AttachmentViewModel
            {
                Id = attachment.Id,
                IssueId = attachment.IssueId,
                Name = attachment.Name,
                Url = attachment.Url,
            }).FirstOrDefaultAsync() ?? null!;
        }


        public async Task<byte[]> GetFile(Guid id)
        {
            var file = await _attachmentRepository.FirstOrDefaultAsync(file => file.Id.Equals(id));

            if (file != null)
            {
                return System.IO.File.ReadAllBytes(file.Url);
            }
            return null!;
        }

    }
}
