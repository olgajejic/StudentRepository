﻿using Olga.Framework.Entities;
using System;

namespace FON.Olga.StudentManagement.Entities
{
    public class Student : Entity
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
