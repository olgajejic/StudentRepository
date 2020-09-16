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

        public void Start()
        {
            BrokerManager brokerManager = new BrokerManager();
            List<Student> listForInsert = new List<Student>();
            List<Student> listForUpdate = new List<Student>();
            List<Student> listForDelete = new List<Student>();
            bool end = false;
            while (!end)
            {
                Console.WriteLine("Press:\n 1 - insert\n 2 - delete\n 3 - update\n 4 - save\n 5 - get all\n 6 - get\n 7 - exit\n");
                int n = Convert.ToInt32(Console.ReadLine());

                switch (n)
                {
                    case 1:
                        {
                            Student student1 = Data();
                            Student student2 = Data();
                            try
                            {
                                //rokerManager.Insert(student1, student2);     
                                // brokerManager.Save(new List<Student>() { student1, student2 }, null, null);
                                listForInsert.Add(student1);
                                listForInsert.Add(student2);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case 2:
                        {
                            bool exit = false;
                            while (!exit)
                            {
                                try
                                {
                                    Console.WriteLine("Inesrt ID: ");
                                    long id = Convert.ToInt64(Console.ReadLine());
                                    Console.WriteLine("To stop insert 0");
                                    var e = Convert.ToInt32(Console.ReadLine());

                                    if (e.Equals(0)) 
                                        exit = true;

                                    Student student = brokerManager.Get(id, typeof(Student)) as Student;
                                    if (student == null)
                                        Console.WriteLine($"Student with id: {id} is not in the database");

                                    else
                                    {
                                        listForDelete.Add(student);
                                        //brokerManager.Save(null, null, entities);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            break;
                        }
                    case 3:
                        {

                            bool exit = false;
                            List<Entity> entities = new List<Entity>();
                            while (!exit)
                            {
                                try
                                {
                                    Console.WriteLine("Inesrt ID: ");
                                    long id = Convert.ToInt64(Console.ReadLine());
                                   
                                    Student student = brokerManager.Get(id, typeof(Student)) as Student;
                                    UpdateData(student);
                                    Console.WriteLine("To stop insert 0");
                                    var e = Convert.ToInt32(Console.ReadLine());

                                    if (e.Equals(0))
                                        exit = true;

                                    if (student == null)
                                        Console.WriteLine($"Student with id: {id} is not in the database");

                                    else
                                    {
                                        listForUpdate.Add(student);
                                        //brokerManager.Save(null, entities, null);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            break;
                        }
                    case 4:
                        brokerManager.Save(listForInsert, listForUpdate, listForDelete);
                        break;
                    case 5:
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
                    case 6:

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
                    case 7:
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
