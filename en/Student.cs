using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class Student
    {
        public string StudentID { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return $"{StudentID} - {FullName}"; // Affichage de l'ID et du nom complet
        }
    }
}