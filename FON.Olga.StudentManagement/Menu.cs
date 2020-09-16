using FON.Olga.StudentManagement.Brokers;
using FON.Olga.StudentManagement.Entities;
using Olga.Framework.Entities;
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
                Console.WriteLine("Press:\n 1 - insert\n 2 - delete\n 3 - update\n 4 - get all\n 5 - get\n 6 - exit\n");
                int n = Convert.ToInt32(Console.ReadLine());

                switch (n)
                {
                    case 1:
                        {
                            Student student1 = Data();
                            Student student2 = Data();

                            OracleConnection connection = GetConnection();
                            OracleTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                broker.Insert(student1, connection, transaction);
                                broker.Insert(student2, connection, transaction);

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
                        }
                    case 2:
                        {
                            OracleConnection connection1 = GetConnection();
                            OracleTransaction transaction1 = null;

                            try
                            {
                                Console.WriteLine("Inesrt ID: ");
                                long id = Convert.ToInt64(Console.ReadLine());

                                connection1.Open();
                                transaction1 = connection1.BeginTransaction();

                                broker.Delete(id, connection1, transaction1);

                                transaction1.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction1?.Rollback();
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                connection1?.Close();
                            }
                            break;
                        }
                    case 3:
                        {
                            OracleConnection connection3 = GetConnection();
                            OracleTransaction transaction3 = null;

                            try
                            {
                                Console.WriteLine("Inesrt id:");
                                long sID = Convert.ToInt64(Console.ReadLine());
                                Entity e = UpdateData(sID);

                                connection3.Open();
                                transaction3 = connection3.BeginTransaction();

                                broker.Update(e, connection3, transaction3);

                                transaction3.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction3?.Rollback();
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                connection3?.Close();
                            }
                            break;
                        }
                    case 4:
                        {
                            OracleConnection connection4 = GetConnection();                            

                            try
                            {
                                connection4.Open();                                

                                List<Entity> students = broker.GetAll(connection4);
                                foreach (Entity entity in students)
                                {
                                    Console.WriteLine(entity);
                                }                                
                            }
                            catch (Exception ex)
                            {
                                //if (transaction4 != null)
                                //{
                                //    transaction4.Rollback();
                                //}
                                
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                connection4?.Close();
                            }
                            break;
                        }
                    case 5:
                        BrokerManager brokerManager = new BrokerManager();

                        try
                        {
                            var student = brokerManager.Get(1000, typeof(Student)) as Student;                            

                            Console.WriteLine(student);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                        break;
                    case 6:
                        end = true;
                        break;
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
