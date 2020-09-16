using Olga.Framework.Brokers;
using Olga.Framework.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace FON.Olga.StudentManagement.Brokers
{
    public class BrokerManager : EntityBrokerManager
    {
        public override Entity Get(long id, Type entityType)
        {
            OracleConnection connection = GetConnection();

            try
            {
                connection.Open();

                IEntityBroker broker = GetBroker(entityType);

                Entity entity = broker.Get(id, connection);

                return entity;
            }            
            finally
            {
                connection?.Close();
            }
        }

        public override void Insert(Entity entity)
        {
            throw new NotImplementedException();            
        }

        private OracleConnection GetConnection()
        {
            OracleConnection connection = new OracleConnection();

            OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();
            ocsb.Password = "olga123";
            ocsb.UserID = "olga";
            ocsb.DataSource = "localhost:1521/orcl";
            connection.ConnectionString = ocsb.ConnectionString;

            return connection;
        }

        private IEntityBroker GetBroker(Type entityType)
        {
            return null;
        }        
    }
}
