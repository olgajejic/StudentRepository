using Olga.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FON.Olga.StudentManagement.Entities
{
    public class Professor: Entity
    {
        public Professor(long ID, string Name, string Surname, int versionNumber)
        {
            this.ID = ID;
            this.Name = Name;
            this.Surname = Surname;
            this.VersionNumber = versionNumber;
        }

        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString()
        {
            return "Student: " + ID + " " + Name + " " + Surname + " " + VersionNumber;
        }
    }
}
