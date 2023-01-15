using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(KanbanContext context) : base(context)
        {
        }
    }
}
