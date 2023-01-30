using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(KanbanContext context) : base(context)
        {
        }
    }
}
