using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(KanbanContext context) : base(context)
        {
        }
    }
}
