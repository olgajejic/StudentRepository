using Olga.Framework.Entities;
using System;

namespace Olga.Framework.Brokers
{
    public interface IEntityBrokerManager
    {
        Entity Get(long id, Type entityType);
        void Insert(Entity entity);
    }
}
