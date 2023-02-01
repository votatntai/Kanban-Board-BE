using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class LogWorkRepository : Repository<LogWork>, ILogWorkRepository
    {
        public LogWorkRepository(KanbanContext context) : base(context)
        {
        }
    }
}
