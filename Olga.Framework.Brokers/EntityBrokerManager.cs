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
        public abstract void Insert(Type entityType, params Entity[] entities);
        public abstract List<Entity> GetAll(Type entityType);
        public abstract void Delete(long id, Type entityType);
        public abstract void Update(Entity e, Type type);
    }
}
