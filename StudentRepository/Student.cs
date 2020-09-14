using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRepository
{
    public abstract class Entity
    {
        public long ID { get; set; }
    }

    public class Student: Entity
    {
        public Student()
        {
          
        }
        public Student(long ID, string Name, string Surname)
        {
            this.ID = ID;
            this.Name = Name;
            this.Surname = Surname;
        }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public override string ToString()
        {
            return "Student: " + Name + " " + Surname;
        }
    }

}
