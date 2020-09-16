using Olga.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Olga.Framework.Brokers
{
    public abstract class EntityBrokerManager : IEntityBrokerManager
    {
        public abstract Entity Get(long id, Type entityType);
        public abstract List<Entity> GetAll(Type entityType);

        public abstract void Insert(params Entity[] entities);        
        public abstract void Delete(params Entity[] entities);
        public abstract void Update(params Entity[] entities);

        public abstract void Save(IEnumerable<Entity> entitiesForInsert, IEnumerable<Entity> entitiesForUpdate, IEnumerable<Entity> entitiesForDelete);
    }
}
