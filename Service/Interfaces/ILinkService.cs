using Data.Models.Requests.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ILinkService
    {
        Task<LinkViewModel> GetLink(Guid id);
        Task<LinkViewModel> CreateLink(CreateLinkRequestModel model);
        Task<bool> DeleteLink(Guid id);
    }
}
