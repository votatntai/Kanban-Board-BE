using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class LinkRepository : Repository<Link>, ILinkRepository
    {
        public LinkRepository(KanbanContext context) : base(context)
        {
        }
    }
}
