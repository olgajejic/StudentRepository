using FON.Olga.StudentManagement.Entities;
using MessagePack;
using MessagePack.Resolvers;
using Olga.Framework.Brokers;
using Olga.Framework.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

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
                result = new Student((long)reader["ID"], (string)reader["NAME"], (string)reader["SURNAME"], (int)reader["VERSION"]);

            return result;
        }

        public void Insert(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            var s = entity as Student;
            OracleCommand command = (connection as OracleConnection).CreateCommand();
            command.Transaction = transaction as OracleTransaction;

            string sql = "insert into students values(:id, :name, :surname, :version)";
            command.Parameters.Add("@id", s.ID);
            command.Parameters.Add("@name", s.Name);
            command.Parameters.Add("@surname", s.Surname);
            command.Parameters.Add("@version", s.VersionNumber + 1);
            command.CommandText = sql;

            command.ExecuteNonQuery();


        }



        public void Update(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            Student sForUpdate = entity as Student;

            OracleCommand command = new OracleCommand($"update students set name = :name, surname = :surname, version = :version where id = :id and version = :version1", connection as OracleConnection);
            command.Transaction = transaction as OracleTransaction;

            command.Parameters.Add("@name", sForUpdate.Name);
            command.Parameters.Add("@surname", sForUpdate.Surname);
            command.Parameters.Add("@version", sForUpdate.VersionNumber + 1);
            command.Parameters.Add("@id", entity.ID);
            command.Parameters.Add("@version1", entity.VersionNumber);

            var n = command.ExecuteNonQuery();

            if (n < 1)
                throw new Exception($"There's no entity with version number: {entity.VersionNumber}");
            entity.VersionNumber++;
        }

        public void Delete(Entity entity, DbConnection connection, DbTransaction transaction)
        {
            OracleCommand command = (connection as OracleConnection).CreateCommand();
            command.Transaction = transaction as OracleTransaction;

            string sql = "delete students where id = :id and version = :version";
            command.Parameters.Add("@id", entity.ID);
            command.Parameters.Add("@version", entity.VersionNumber);

            command.CommandText = sql;
            int n = command.ExecuteNonQuery();

            if (n < 1)
                throw new Exception($"There's no entity with version number: {entity.VersionNumber}");
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
                int versionNumber = (int)reader["VERSION"];

                Student s = new Student(id, name, surname, versionNumber);
                result.Add(s);
            }

            return result;
        }


    }
}
