using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

        private static string GetConnectionString()
        {
            String connString = "host = localhost:1521/orcl;uid = 'olga'; pswd = 'olga123'";
            return connString;
        }
        
        public void Start()
        {
            string connectionString = GetConnectionString();
            OracleConnection connection = new OracleConnection();
            connection.Open();
            connection.ConnectionString = connectionString;

            StudentBroker broker = new StudentBroker();

            Console.WriteLine("Press:\n 1 - insert\n 2 - delte\n 3 - update\n 4 - get all\n 5 - exit\n");
            int n = Convert.ToInt32(Console.ReadLine());



            //switch (n)
            //{
            //    case 1:
            //        broker.Insert(entity, connection);
            //        break;
            //    case 2:
            //        broker.Delete(entity, connection);
            //        break;
            //    case 3:
            //        broker.Update(entity, connection);
            //        break;
            //    case 4:
            //        broker.GetAll(entity, connection);
            //        break;
            //    case 5:
            //        Environment.Exit(0);
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
