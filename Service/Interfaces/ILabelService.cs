using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ILabelService
    {
        Task<LabelViewModel> GetLabel(Guid id);
        Task<LabelViewModel> CreateLabel(CreateLabelRequestModel model);
        Task<LabelViewModel> UpdateLabel(Guid id, UpdateLabelRequestModel model);
        Task<bool> DeleteLabel(Guid id);
    }
}
