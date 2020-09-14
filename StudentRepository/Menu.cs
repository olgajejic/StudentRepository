using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace StudentRepository
{
    public class Menu
    {
        public Menu()
        {
            Start();
        }

        private string GetConnectionString()
        {
            return null;

            //// connect
            //con.ConnectionString = ocsb.ConnectionString;
            //String connString = "Data Source=olga@/localhost:1521/orcl;Id=olga;Password=olga123";
            //return connString;
        }

        public void Start()
        {
            string connectionString = GetConnectionString();
            OracleConnection connection = new OracleConnection();

            OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();
            ocsb.Password = "olga123";
            ocsb.UserID = "olga";
            ocsb.DataSource = "localhost:1521/orcl";
            connection.ConnectionString = ocsb.ConnectionString;

            connection.Open();


            OracleCommand command = connection.CreateCommand();
            OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            command.Transaction = transaction;

            StudentBroker broker = new StudentBroker();
            try
            {
                bool end = false;
                while (!end)
                {
                    Console.WriteLine("Press:\n 1 - insert\n 2 - delte\n 3 - update\n 4 - get all\n 5 - exit\n");
                    int n = Convert.ToInt32(Console.ReadLine());

                    switch (n)
                    {
                        case 1:
                            Student student = Data();
                            broker.Insert(student, connection);
                            break;
                        case 2:
                            Console.WriteLine("Inesrt ID: ");
                            long id = Convert.ToInt64(Console.ReadLine());
                            broker.Delete(id, connection);
                            break;
                        case 3:
                            Console.WriteLine("Inesrt id:");
                            long sID = Convert.ToInt64(Console.ReadLine());
                            Entity e = UpdateData(sID);

                            broker.Update(e, connection);
                            break;
                        case 4:
                            List<Entity> students = broker.GetAll(connection);
                            foreach (Entity entity in students)
                            {
                                Console.WriteLine(entity);
                            }
                            break;
                        case 5:
                            end = true;
                            break;
                        default:
                            break;
                    }
             
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.ToString());
                Console.WriteLine("Neither record was written to database.");
            }
            finally
            {
                connection.Close();
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
