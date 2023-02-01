using Data.Models.Requests.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ILogWorkService
    {
        Task<LogWorkViewModel> GetLogWork(Guid id);
        Task<LogWorkViewModel> CreateLogWork(CreateLogWorkRequestModel model);
        Task<bool> DeleteLogWork(Guid id);
    }
}
