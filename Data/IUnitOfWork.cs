using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        public IProjectRepository Project { get; }
        public IIssueRepository Issue { get; }
        public IStatusRepository Status { get; }
        public IPriorityRepository Priority { get; }
        public ILabelRepository Label { get; }
        public IIssueLabelRepository IssueLabel { get; }
        public ITypeRepository Type { get; }

        Task<int> SaveChanges();
    }
}
