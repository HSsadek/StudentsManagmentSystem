using StudentsDataAccessLayer;
using System;

namespace StudentsBusinessLayer
{
    public class Student
    {
        public static List<StudentDTO> GetAllStudents()
        {
            return StudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static decimal GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

    }
}
