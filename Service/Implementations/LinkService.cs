using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class LinkService : BaseService, ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        public LinkService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _linkRepository = unitOfWork.Link;
        }

        public async Task<LinkViewModel> GetLink(Guid id)
        {
            return await _linkRepository.GetMany(link => link.Id.Equals(id))
                .Select(link => new LinkViewModel
                {
                    Id = link.Id,
                    Description = link.Description!,
                    IssueId = link.IssueId,
                    Url = link.Url
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<LinkViewModel> CreateLink(CreateLinkRequestModel model)
        {
            var id = Guid.NewGuid();
            var link = new Link
            {
                Id = id,
                Description = model.Description,
                IssueId = model.IssueId,
                Url = model.Url
            };
            _linkRepository.Add(link);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetLink(id);
            }
            return null!;
        }

        public async Task<bool> DeleteLink(Guid id)
        {
            var result = false;
            var link = await _linkRepository.GetMany(link => link.Id.Equals(id)).FirstOrDefaultAsync();
            if (link != null)
            {
                _linkRepository.Remove(link);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }

    }
}
