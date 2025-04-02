using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class Account
    {
        public string Id { get; set; } // Changed to string
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    

    public class Department
    {
        public string Id { get; set; } 
        public string Name { get; set; }
    }


    public class ClassSection
    {
        public string ClassId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Schedule { get; set; }
        public int NumberOfStudents { get; set; }

        public string SubjectDisplay => $"{SubjectId} - {(string.IsNullOrEmpty(SubjectName) ? "Unknown Subject" : SubjectName)}";
        public string TeacherDisplay => $"{TeacherId} - {(string.IsNullOrEmpty(TeacherName) ? "Unknown Teacher" : TeacherName)}";
    }

    public class Teacher
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Student
    {
        public string StudentID { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return $"{StudentID} - {FullName}"; 
        }
    }
}


