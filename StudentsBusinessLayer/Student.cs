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

    }
}
