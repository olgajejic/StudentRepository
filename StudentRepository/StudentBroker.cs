using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace StudentRepository
{
    public interface EntityBroker
    {
        Entity Get(long id, DbConnection connection);
        void Insert(Entity entity, DbConnection connection);
        void Update(Entity entity, DbConnection connection);
        void Delete(Entity entity, DbConnection connection);
        List<Entity> GetAll(DbConnection connection);
    }

    public class StudentBroker : EntityBroker
    {
        public StudentBroker()
        {
        }


        public Entity Get(long id, DbConnection connection)
        {
            Entity result = null;

            OracleCommand cmd = new OracleCommand("SELECT * FROM STUDENTS where id=:id", connection as OracleConnection);
            cmd.Parameters.Add("@id", id);
            OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
                result = new Student((long)reader["ID"], (string)reader["NAME"], (string)reader["SURNAME"]);

            return result;
        }

        public void Insert(Entity entity, DbConnection connection)
        {
            Student s = entity as Student;
            OracleCommand command = (connection as OracleConnection).CreateCommand();
            string sql = "insert into students values(:id, :name, :surname)";
            command.Parameters.Add("@id", s.ID);
            command.Parameters.Add("@name", s.Name);
            command.Parameters.Add("@surname", s.Surname);
            command.CommandText = sql;
            command.ExecuteNonQuery();

        }

        public void Update(Entity entity, DbConnection connection)
        {
            Student sForUpdate = entity as Student;

            OracleCommand command = new OracleCommand("update students set name = :name, surname = :surname where id = :id", connection as OracleConnection);

            command.Parameters.Add("@name", sForUpdate.Name);
            command.Parameters.Add("@surname", sForUpdate.Surname);
            command.Parameters.Add("@id", entity.ID);

            command.ExecuteNonQuery();
        }

        public void Delete(Entity entity, DbConnection connection)
        {

            OracleCommand command = (connection as OracleConnection).CreateCommand();
            string sql = "delete students where id = :id";
            command.Parameters.Add("@id", entity.ID);
       
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public List<Entity> GetAll(DbConnection connection)
        {
            var result = new List<Entity>();

            OracleCommand cmd = new OracleCommand("SELECT * FROM STUDENTS", connection as OracleConnection);
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                long id = (long)reader["ID"];
                string name = (string)reader["NAME"];
                string surname = (string)reader["SURNAME"];

                Student s = new Student(id, name, surname);
                Debug.WriteLine("bla bla");
                result.Add(s);
            }

            return result;
        }
    }

    public class BrokerManager
    {
        public void Insert(Entity entity)
        {
            DbConnection connection = null;
            // open connection
            //begin transaction

            var broker = CreateEntityBroker(entity);

            broker.Insert(entity, connection);

            //commit transation
            //close connection
        }

        private EntityBroker CreateEntityBroker(Entity entity)
        {
            var entityType = entity.GetType();

            switch (entityType.Name)
            {
                case "Student": return new StudentBroker();
                default:
                    throw new Exception("Unsuported entity ...");
            }
        }
    }
}
