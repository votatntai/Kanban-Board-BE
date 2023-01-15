using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(KanbanContext context) : base(context)
        {
        }
    }
}
