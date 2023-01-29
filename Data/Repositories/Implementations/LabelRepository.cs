using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class LabelRepository : Repository<Label>, ILabelRepository
    {
        public LabelRepository(KanbanContext context) : base(context)
        {
        }
    }
}
