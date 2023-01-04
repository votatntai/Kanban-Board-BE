using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        public IProjectRepository Project { get; }

        Task<int> SaveChanges();
    }
}
