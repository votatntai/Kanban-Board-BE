using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {
        public PriorityRepository(KanbanContext context) : base(context)
        {
        }
    }
}
