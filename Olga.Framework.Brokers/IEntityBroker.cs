using Olga.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Olga.Framework.Brokers
{
    public interface IEntityBroker
    {
        Entity Get(long id, DbConnection connection);
        void Insert(Entity entity, DbConnection connection, DbTransaction transaction);
        void Update(Entity entity, DbConnection connection, DbTransaction transaction);
        void Delete(Entity entity, DbConnection connection, DbTransaction transaction);
        List<Entity> GetAll(DbConnection connection);
       
    }
}
