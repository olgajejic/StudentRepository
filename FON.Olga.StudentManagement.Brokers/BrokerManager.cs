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
        List<EntityState> eStates = new List<EntityState>();
        public override Entity Get(long id, Type entityType)
        {

            foreach (var es in eStates)
            {
                if (es.Entity.ID == id)
                {
                    return es.Entity;
                }
            }

            OracleConnection connection = GetConnection();

            try
            {
                connection.Open();

                IEntityBroker broker = GetBroker(entityType);

                Entity entity = broker.Get(id, connection);

                eStates.Add(new EntityState(entity, State.UNCHANGED));

                return entity;
            }
            finally
            {
                connection?.Close();
            }
        }

        public override List<EntityState> GetAll(Type entityType)
        {
            OracleConnection connection = GetConnection();

            try
            {
                connection.Open();

                IEntityBroker broker = GetBroker(entityType);

                List<Entity> entities = broker.GetAll(connection);

                foreach (var entity in entities)
                {
                    eStates.Add(new EntityState(entity, State.UNCHANGED));
                }

                return eStates;
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
            foreach (var entity in entities)
            {
                foreach (var e in eStates)
                {
                    if (entity.ID == e.Entity.ID)
                        throw new Exception("Cannot insert two entities with the same id!");
                }
                eStates.Add(new EntityState(entity, State.NEW));
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
            foreach (var entity in entities)
            {
                foreach (var e in eStates.ToArray())
                {
                    if (entity.ID == e.Entity.ID)
                    {
                        switch (e.EState)
                        {
                            case State.NEW:
                                eStates.Remove(e);
                                break;
                            case State.UNCHANGED:
                                e.EState = State.DELETED;
                                break;
                            case State.CHANGED:
                                e.EState = State.DELETED;
                                break;
                            default:
                                break;
                        }
                    }
                }
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

            foreach (var entity in entities)
            {
                foreach (var e in eStates)
                {
                    if (entity.ID == e.Entity.ID)
                    {
                        switch (e.EState)
                        {
                            case State.NEW:
                                break;
                            case State.UNCHANGED:
                                e.EState = State.CHANGED;
                                break;
                            case State.CHANGED:
                                break;
                            case State.DELETED:
                                throw new Exception("Cannot update deleted entity!");
                            default:
                                break;
                        }
                    }
                }
            }

        }

        public override void Save()
        {
            OracleConnection connection = GetConnection();
            OracleTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                foreach (var entity in eStates)
                {
                    if (entity.EState == State.NEW)
                        InsertOne(entity.Entity, connection, transaction);
                    if (entity.EState == State.DELETED)
                        DeleteOne(entity.Entity, connection, transaction);
                    if (entity.EState == State.CHANGED)
                        UpdateOne(entity.Entity, connection, transaction);
                }

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

        public override void SaveAll(IEnumerable<Entity> entities)
        {

        }

        private void InsertOne(Entity entity, OracleConnection connection, OracleTransaction transaction)
        {
            IEntityBroker broker = GetBroker(entity.GetType());
            broker.Insert(entity, connection, transaction);
        }

        private void DeleteOne(Entity entity, OracleConnection connection, OracleTransaction transaction)
        {
            IEntityBroker broker = GetBroker(entity.GetType());
            broker.Delete(entity, connection, transaction);
        }

        private void UpdateOne(Entity entity, OracleConnection connection, OracleTransaction transaction)
        {

                IEntityBroker broker = GetBroker(entity.GetType());
                broker.Update(entity, connection, transaction);
            
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
