using FON.Olga.StudentManagement.Brokers;
using FON.Olga.StudentManagement.Entities;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Olga.Framework.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FON.Olga.StudentManagement
{
    public class Menu
    {
        public Menu()
        {
            Start();

        }
        private dynamic WriteToFile(List<Student> students)
        {
            try
            {
                var compression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
                byte[] contentsToWriteToFile = MessagePackSerializer.Serialize(students, compression);

                File.WriteAllBytes(@"C:\Users\snezanaj\source\repos\mp.msgpack", contentsToWriteToFile);
                return contentsToWriteToFile;
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        private void ReadFromFile(byte[] x)
        {

            try
            {
                List<Student> students = new List<Student>();
                var compression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
                var stu = MessagePackSerializer.Deserialize<List<Student>>(File.ReadAllBytes(@"C:\Users\snezanaj\source\repos\mp.msgpack"), compression);

                //foreach (var student in stu)
                //{
                //    Console.WriteLine(student);
                //}

            }
            catch (Exception)
            {

                throw new Exception("Cannot read from a file");
            }

        }

        private void WriteToJsonFile(List<Entity> students)
        {
            string filePath = @"C:\Users\snezanaj\source\repos\mp.json";
            JsonSerializer jsonSerialize = new JsonSerializer(); //object for json class
            if (File.Exists(filePath)) File.Delete(filePath);

            StreamWriter sw = new StreamWriter(filePath); //create file
            JsonWriter jsonWriter = new JsonTextWriter(sw); //write serialized json in file

            jsonSerialize.Serialize(jsonWriter, students);
            jsonWriter.Close();
            sw.Close();


        }

        public void ReadFromJsonFIle()
        {
            string filePath = @"C:\Users\snezanaj\source\repos\mp.json";
            if (File.Exists(filePath) == false)
                throw new Exception($"File doesn't exist on given path: '{filePath}'");

          var entities = JsonConvert.DeserializeObject<List<Student>>(File.ReadAllText(filePath));
            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
            }


            //JsonSerializer jsonSerializer = new JsonSerializer();
            //if (File.Exists(filePath))
            //{
            //    StreamReader sr = new StreamReader(filePath);
            //    JsonReader jsonReader = new JsonTextReader(sr);

            //    //  JsonConvert.DeserializeObject<List<Student>>(File.read(filePath));
            //    var st = jsonSerializer.Deserialize<List<Student>>(jsonReader);


            //    foreach (var s in st)
            //    {
            //        Console.WriteLine(s);
            //    }

            //    jsonReader.Close(); //better to use using then closing here
            //    sr.Close();
            //    Console.ReadLine();
            //}

        }
        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public void Start()
        {
            BrokerManager brokerManager = new BrokerManager();

            bool end = false;
            while (!end)
            {
                Console.WriteLine("Press:\n 1 - insert\n 2 - delete\n 3 - update\n 4 - save\n 5 - get all\n 6 - get\n 7 - exit\n");
                int n = Convert.ToInt32(Console.ReadLine());

                switch (n)
                {
                    case 1:
                        {
                            //Student student1 = Data();
                            //Student student2 = Data();

                            byte[] x = Array.Empty<byte>();
                            List<Entity> entities = new List<Entity>();
                            List<Student> students = new List<Student>();
                            
                            for (int i = 1; i <= 1000; i++)
                            {
                                Student student = new Student(i, RandomString(RandomNumber(50, 156)), RandomString(RandomNumber(50, 156)), RandomNumber(0,i));
                               // Professor professor = new Professor(i, RandomString(RandomNumber(50, 156)), RandomString(RandomNumber(50, 156)), RandomNumber(0, i));
                                students.Add(student);
                                //entities.Add(professor);

                            }

                            var b = DateTime.Now;
                            x = WriteToFile(students);
                            //WriteToJsonFile(entities);
                           // ReadFromJsonFIle();
                            ReadFromFile(x);
                            Console.WriteLine(DateTime.Now-b);

                            try
                            {

                                // brokerManager.Insert(student1);
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
                                        //student.EntityState = State.DELETED;
                                        //brokerManager.Save(null, null, entities);
                                        brokerManager.Delete(student);
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
                                        brokerManager.Update(student);
                                        //entities.Add(student);
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
                        brokerManager.Save();
                        break;
                    case 5:
                        {

                            try
                            {
                                List<EntityState> students = brokerManager.GetAll(typeof(Student));
                                foreach (EntityState entity in students)
                                {
                                    Console.WriteLine(entity.Entity);
                                }
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex.Message);
                            }

                            break;
                        }
                    case 6:

                        Console.WriteLine("Insert students's ID: ");
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
            //student.EntityState = State.CHANGED;
        }
    }
}
