using Olga.Framework.Brokers;
using Olga.Framework.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

        public override void Insert(Type entityType, params Entity[] entities)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                IEntityBroker broker = GetBroker(entityType);

                broker.Insert(connection, transaction, entities);

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        public void Save()
        {
            
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
            switch (entityType.Name)
            {
                case "Student": return new StudentBroker();
                default:
                    throw new Exception("No such broker");
            }
        }

        public override List<Entity> GetAll(Type entityType)
        {
            OracleConnection connection = GetConnection();

            try
            {
                connection.Open();

                IEntityBroker broker = GetBroker(entityType);

                List<Entity> entities = broker.GetAll(connection);

                return entities;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public override void Delete(long id, Type entityType)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                IEntityBroker broker = GetBroker(entityType);

                broker.Delete(id, connection, transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        public override void Update(Entity e, Type entityType)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                IEntityBroker broker = GetBroker(entityType);

                broker.Update(e, connection, transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine(ex.Message);
            }
           
            finally
            {
                connection?.Close();
            }
        }
    }
}
