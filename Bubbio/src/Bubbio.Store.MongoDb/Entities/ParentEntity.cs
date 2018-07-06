using Bubbio.Core.Contracts;

namespace Bubbio.Store.MongoDb.Entities
{
    public class ParentEntity : GuidEntityBase, IParent
    {
        public IName Name { get; set; }
    }
}