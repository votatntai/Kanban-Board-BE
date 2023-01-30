using Data;
using Data.Entities;
using Data.Models.Requests.Create;
using Data.Models.Requests.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class LabelService : BaseService, ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IIssueLabelRepository _issueLabelRepository;
        public LabelService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _labelRepository = unitOfWork.Label;
            _issueLabelRepository = unitOfWork.IssueLabel;
        }

        public async Task<LabelViewModel> GetLabel(Guid id)
        {
            return await _labelRepository.GetMany(label => label.Id.Equals(id))
                .Select(label => new LabelViewModel
                {
                    Id = label.Id,
                    Name = label.Name,
                    ProjectId = label.ProjectId,
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<LabelViewModel> CreateLabel(CreateLabelRequestModel model)
        {
            var id = Guid.NewGuid();
            var label = new Label
            {
                Id = id,
                Name = model.Name,
                ProjectId = model.ProjectId,
                UpdateAt = DateTime.Now,
            };
            _labelRepository.Add(label);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetLabel(id);
            }
            return null!;
        }

        public async Task<LabelViewModel> UpdateLabel(Guid id, UpdateLabelRequestModel model)
        {
            var label = await _labelRepository.GetMany(label => label.Id.Equals(id)).Include(label => label.Project).FirstOrDefaultAsync();
            if (label != null)
            {
                if (model.Name != null) label.Name = model.Name;
                label.UpdateAt = DateTime.Now;
                label.Project.LastActivity = DateTime.Now;
                _labelRepository.Update(label);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetLabel(id);
                }
            }
            return null!;
        }

        public async Task<bool> DeleteLabel(Guid id)
        {
            var result = false;
            var label = await _labelRepository.GetMany(label => label.Id.Equals(id)).Include(label => label.Project).FirstOrDefaultAsync();
            if (label != null)
            {
                var issueLabels = await _issueLabelRepository.GetMany(issueLabel => issueLabel.LabelId.Equals(label.Id)).ToListAsync();
                label.Project.LastActivity = DateTime.Now;
                _issueLabelRepository.RemoveRange(issueLabels);
                _labelRepository.Update(label);
                _labelRepository.Remove(label);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }
    }
}
