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
        public abstract void Insert(Entity entity);
    }
}
