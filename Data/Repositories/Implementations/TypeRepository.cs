using Data.Entities;
using Data.Repositories.Interfaces;
using Type = Data.Entities.Type;

namespace Data.Repositories.Implementations
{
    public class TypeRepository : Repository<Type>, ITypeRepository
    {
        public TypeRepository(KanbanContext context) : base(context)
        {
        }
    }
}
