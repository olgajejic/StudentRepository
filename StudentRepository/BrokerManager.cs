using System;
using System.Data.Common;

namespace StudentRepository
{
    public class BrokerManager
    {
        //public void Insert(Entity entity)
        //{
        //    DbConnection connection = null;
        //    // open connection
        //    //begin transaction

        //    var broker = CreateEntityBroker(entity);

        //    //broker.Insert(entity, connection);

        //    //commit transation
        //    //close connection
        //}

        //private IEntityBroker CreateEntityBroker(Entity entity)
        //{
        //    var entityType = entity.GetType();

        //    switch (entityType.Name)
        //    {
        //        case "Student": return new StudentBroker();
        //        default:
        //            throw new Exception("Unsuported entity ...");
        //    }
        //}
    }
}
