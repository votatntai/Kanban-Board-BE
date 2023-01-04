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

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
