using Olga.Framework.Entities;
using System;
using System.Collections.Generic;

namespace Olga.Framework.Brokers
{
    public interface IEntityBrokerManager
    {
        Entity Get(long id, Type entityType);
        List<EntityState> GetAll(Type entityType);
        void Insert(params Entity[] entities);        
        void Delete(params Entity[] entities);
        void Update(params Entity[] entities);

        void Save(IEnumerable<Entity> entitiesForInsert, IEnumerable<Entity> entitiesForUpdate, IEnumerable<Entity> entitiesForDelete);
        void SaveAll(IEnumerable<Entity> entities);
        void Save();
    }
}
