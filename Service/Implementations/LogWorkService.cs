using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class LogWorkService : BaseService, ILogWorkService
    {
        private readonly ILogWorkRepository _logWorkRepository;
        public LogWorkService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _logWorkRepository = unitOfWork.LogWork;
        }

        public async Task<LogWorkViewModel> GetLogWork(Guid id)
        {
            return await _logWorkRepository.GetMany(logWork => logWork.Id.Equals(id)).Select(logWork => new LogWorkViewModel
            {
                Id = logWork.Id,
                Description = logWork.Description,
                IssueId = logWork.IssueId,
                RemainingTime = logWork.RemainingTime,
                SpentTime = logWork.SpentTime,
                User = new UserViewModel
                {
                    Id = logWork.User.Id,
                    Email = logWork.User.Email,
                    Name = logWork.User.Name,
                    Username = logWork.User.Username
                },
                CreateAt = logWork.CreateAt
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<LogWorkViewModel> CreateLogWork(CreateLogWorkRequestModel model)
        {
            var id = Guid.NewGuid();
            var logWork = new LogWork
            {
                Id = id,
                Description = model.Description,
                IssueId = model.IssueId,
                RemainingTime = model.RemainingTime,
                SpentTime = model.SpentTime,
                UserId = model.UserId,
                CreateAt = DateTime.Now,
            };
            _logWorkRepository.Add(logWork);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetLogWork(id);
            }
            return null!;
        }

        public async Task<bool> DeleteLogWork(Guid id)
        {
            var result = false;
            var logWork = await _logWorkRepository.GetMany(logWork => logWork.Id.Equals(id))
                    .Include(logWork => logWork.Issue).ThenInclude(issue => issue.Project).FirstOrDefaultAsync();
            if (logWork != null)
            {
                logWork.Issue.Project.LastActivity = DateTime.Now;
                _logWorkRepository.Update(logWork);
                _logWorkRepository.Remove(logWork);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }
    }
}
