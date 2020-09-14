using FON.Olga.StudentManagement.Brokers;
using FON.Olga.StudentManagement.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace FON.Olga.StudentManagement
{
    public class Menu
    {
        public Menu()
        {
            Start();
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

        public void Start()
        {
            StudentBroker broker = new StudentBroker();

            bool end = false;
            while (!end)
            {
                Console.WriteLine("Press:\n 1 - insert\n 2 - delte\n 3 - update\n 4 - get all\n 5 - exit\n");
                int n = Convert.ToInt32(Console.ReadLine());

                switch (n)
                {
                    case 1:
                        Student student1 = Data();
                        Student student2 = Data();

                        OracleConnection connection = GetConnection();
                        OracleTransaction transaction = null;

                        try
                        {
                            connection.Open();
                            transaction = connection.BeginTransaction();

                            broker.Insert(student1, connection, transaction);
                            broker.Update(student2, connection, transaction);

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
                        break;
                    //case 2:
                    //    Console.WriteLine("Inesrt ID: ");
                    //    long id = Convert.ToInt64(Console.ReadLine());
                    //    broker.Delete(id, connection);
                    //    break;
                    //case 3:
                    //    Console.WriteLine("Inesrt id:");
                    //    long sID = Convert.ToInt64(Console.ReadLine());
                    //    Entity e = UpdateData(sID);

                    //    broker.Update(e, connection);
                    //    break;
                    //case 4:
                    //    List<Entity> students = broker.GetAll(connection);
                    //    foreach (Entity entity in students)
                    //    {
                    //        Console.WriteLine(entity);
                    //    }
                    //    break;
                    //case 5:
                    //    end = true;
                    //    break;
                    default:
                        break;
                }

            }
        }

        private Student Data()
        {
            Console.WriteLine("Inesrt id:");
            long _id = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Inesrt first name:");
            string _name = Console.ReadLine();
            Console.WriteLine("Inesrt last name:");
            string _lastName = Console.ReadLine();

            Student s = new Student();
            s.ID = _id;
            s.Name = _name;
            s.Surname = _lastName;

            return s;
        }

        private Student UpdateData(long id)
        {
            Console.WriteLine("Inesrt first name:");
            string _name = Console.ReadLine();
            Console.WriteLine("Inesrt last name:");
            string _lastName = Console.ReadLine();

            Student s = new Student();
            s.ID = id;
            s.Name = _name;
            s.Surname = _lastName;

            return s;
        }

    }
}
