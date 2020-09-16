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
            BrokerManager brokerManager = new BrokerManager();

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
                            try
                            {                                
                                brokerManager.Insert(student1, student2);                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case 2:
                        {

                            try
                            {
                                Console.WriteLine("Inesrt ID: ");
                                long id = Convert.ToInt64(Console.ReadLine());

                                Student student = brokerManager.Get(id, typeof(Student)) as Student;

                                if (student == null)
                                    Console.WriteLine($"Student sa id: {id} nije u bazi");
                                else
                                    brokerManager.Delete(student);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case 3:
                        {

                            try
                            {
                                Console.WriteLine("Inesrt id:");
                                long sID = Convert.ToInt64(Console.ReadLine());

                                Student student = brokerManager.Get(sID, typeof(Student)) as Student;
                                UpdateData(student);

                                brokerManager.Update(student);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case 4:
                        {

                            try
                            {
                                List<Entity> students = brokerManager.GetAll(typeof(Student));
                                foreach (Entity entity in students)
                                {
                                    Console.WriteLine(entity);
                                }
                            }
                            catch (Exception ex)
                            {


                                Console.WriteLine(ex.Message);
                            }

                            break;
                        }
                    case 5:

                        Console.WriteLine("Insert students's IDL ");
                        long studentsID = Convert.ToInt64(Console.ReadLine());

                        try
                        {
                            var student = brokerManager.Get(studentsID, typeof(Student)) as Student;
                            Console.WriteLine(student);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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

        private void UpdateData(Student student)
        {
            Console.WriteLine("Inesrt first name:");
            string _name = Console.ReadLine();
            Console.WriteLine("Inesrt last name:");
            string _lastName = Console.ReadLine();
                        
            student.Name = _name;
            student.Surname = _lastName;
        }

    }
}
