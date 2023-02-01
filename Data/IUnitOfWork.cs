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
        public ICommentRepository Comment { get; }
        public IIssueLabelRepository IssueLabel { get; }
        public IProjectMemberRepository ProjectMember { get; }
        public ITypeRepository Type { get; }
        public ILinkRepository Link { get; }
        public ILogWorkRepository LogWork { get; }

        Task<int> SaveChanges();
    }
}
