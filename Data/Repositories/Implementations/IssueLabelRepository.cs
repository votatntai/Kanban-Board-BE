using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class IssueLabelRepository : Repository<IssueLabel>, IIssueLabelRepository
    {
        public IssueLabelRepository(KanbanContext context) : base(context)
        {
        }
    }
}
