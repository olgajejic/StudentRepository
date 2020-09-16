using Olga.Framework.Entities;
using System;
using System.Collections.Generic;

namespace Olga.Framework.Brokers
{
    public interface IEntityBrokerManager
    {
        Entity Get(long id, Type entityType);
        void Insert(Type entityType, params Entity[] entities);
        List<Entity> GetAll(Type entityType);
        void Delete(long id, Type entityType);
        void Update(Entity e, Type type);
    }
}
