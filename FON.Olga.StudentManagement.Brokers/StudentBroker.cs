using FON.Olga.StudentManagement.Entities;
using Olga.Framework.Brokers;
using Olga.Framework.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;

namespace FON.Olga.StudentManagement.Brokers
{
    public class StudentBroker : IEntityBroker
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

        public void Insert(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            Student s = null;

            OracleCommand command = (connection as OracleConnection).CreateCommand();
            command.Transaction = transaction as OracleTransaction;

            string sql = "insert into students values(:id, :name, :surname)";
            command.Parameters.Add("@id", s.ID);
            command.Parameters.Add("@name", s.Name);
            command.Parameters.Add("@surname", s.Surname);
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public void Update(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            Student sForUpdate = entity as Student;

            OracleCommand command = new OracleCommand("update students set name = :name, surname = :surname where id = :id", connection as OracleConnection);
            command.Transaction = transaction as OracleTransaction;

            command.Parameters.Add("@name", sForUpdate.Name);
            command.Parameters.Add("@surname", sForUpdate.Surname);
            command.Parameters.Add("@id", entity.ID);

            command.ExecuteNonQuery();
        }

        public void Delete(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            OracleCommand command = (connection as OracleConnection).CreateCommand();
            command.Transaction = transaction as OracleTransaction;

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
                result.Add(s);
            }

            return result;
        }


    }
}
