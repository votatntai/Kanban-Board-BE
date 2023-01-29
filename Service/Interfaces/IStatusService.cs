using Data.Models.Requests.Create;
using Data.Models.Requests.Get;
using Data.Models.Requests.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IStatusService
    {
        Task<ICollection<StatusViewModel>> GetStatuses(StatusRequest filter);
        Task<StatusViewModel> GetStatus(Guid id);
        Task<StatusViewModel> CreateStatus(CreateStatusRequestModel model);
        Task<StatusViewModel> UpdateStatus(Guid id, UpdateStatusRequestModel model);
        Task<ICollection<StatusViewModel>> UpdateStatuses(ICollection<UpdateStatusRequestModel> models);
        Task<bool> DeleteStatus(Guid id, Guid inheritanceId);
    }
}
