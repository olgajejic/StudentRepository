using Olga.Framework.Brokers;
using Olga.Framework.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public override void Insert(params Entity[] entities)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                InsertImpl(entities, connection, transaction);

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

        private void InsertImpl(IEnumerable<Entity> entities, OracleConnection connection, OracleTransaction transaction)
        {
            foreach (var entity in entities)
            {
                IEntityBroker broker = GetBroker(entity.GetType());

                broker.Insert(entity, connection, transaction);
            }
        }

        public override void Delete(params Entity[] entities)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                DeleteImpl(entities, connection, transaction);

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
        private void DeleteImpl(IEnumerable<Entity> entities, OracleConnection connection, OracleTransaction transaction)
        {
            foreach (var entity in entities)
            {
                IEntityBroker broker = GetBroker(entity.GetType());

                broker.Delete(entity, connection, transaction);
            }
        }

        public override void Update(params Entity[] entities)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                UpdateImpl(entities, connection, transaction);
                
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

        private void UpdateImpl(IEnumerable<Entity> entities, OracleConnection connection, OracleTransaction transaction)
        {
            foreach (var entity in entities)
            {
                IEntityBroker broker = GetBroker(entity.GetType());

                broker.Update(entity, connection, transaction);
            }
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

        public override void Save(IEnumerable<Entity> entitiesForInsert, IEnumerable<Entity> entitiesForUpdate, IEnumerable<Entity> entitiesForDelete)
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                if ((entitiesForInsert != null) && entitiesForInsert.Count() > 0)
                    InsertImpl(entitiesForInsert, connection, transaction);

                if ((entitiesForUpdate != null) && entitiesForUpdate.Count() > 0)
                    UpdateImpl(entitiesForUpdate, connection, transaction);

                if ((entitiesForDelete != null) && entitiesForDelete.Count() > 0)
                    DeleteImpl(entitiesForDelete, connection, transaction);                

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
