using Data.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KanbanContext _context;

        private IUserRepository _user = null!;
        private IProjectRepository _project = null!;
        private IIssueRepository _issue = null!;
        private IStatusRepository _status = null!;
        private IPriorityRepository _priority = null!;
        private ILabelRepository _label = null!;
        private ICommentRepository _comment = null!;
        private IIssueLabelRepository _issueLabel = null!;
        private IProjectMemberRepository _projectMember = null!;
        private ITypeRepository _type = null!;
        private ILinkRepository _link = null!;
        private ILogWorkRepository _logWork = null!;

        public UnitOfWork(KanbanContext context)
        {
            _context = context;
        }
        public IUserRepository User
        {
            get { return _user ??= new UserRepository(_context); }
        }
        public IProjectRepository Project
        {
            get { return _project ??= new ProjectRepository(_context); }
        }   
        public IIssueRepository Issue
        {
            get { return _issue ??= new IssueRepository(_context); }
        }
        public IStatusRepository Status
        {
            get { return _status ??= new StatusRepository(_context); }
        }    
        public IPriorityRepository Priority
        {
            get { return _priority ??= new PriorityRepository(_context); }
        }
        public ILabelRepository Label
        {
            get { return _label ??= new LabelRepository(_context); }
        }
        public ICommentRepository Comment
        {
            get { return _comment ??= new CommentRepository(_context); }
        }
        public IIssueLabelRepository IssueLabel
        {
            get { return _issueLabel ??= new IssueLabelRepository(_context); }
        }
        public IProjectMemberRepository ProjectMember
        {
            get { return _projectMember ??= new ProjectMemberRepository(_context); }
        }
        public ITypeRepository Type
        {
            get { return _type ??= new TypeRepository(_context); }
        }
        public ILinkRepository Link
        {
            get { return _link ??= new LinkRepository(_context); }
        }
        public ILogWorkRepository LogWork
        {
            get { return _logWork ??= new LogWorkRepository(_context); }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
