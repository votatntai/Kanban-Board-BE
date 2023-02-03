using Data;
using Data.Entities;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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

            return await GetFile(id);
        }

        public async Task<AttachmentViewModel> GetFile(Guid id)
        {
            var file = await _attachmentRepository.FirstOrDefaultAsync(file => file.Id.Equals(id));

            if (file != null)
            {
                var fileBytes = System.IO.File.ReadAllBytes(file.Url);
                return new AttachmentViewModel
                {
                    Id = file.Id,
                    Name = file.Name,
                    Url = file.Url,
                    File = fileBytes,
                    IssueId = file.IssueId
                };

            }
            return null!;
        }

    }
}
