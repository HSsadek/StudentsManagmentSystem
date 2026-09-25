using System;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;


namespace StudentsDataAccessLayer
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int grade { get; set; }
        public StudentDTO(int id , string name , int age , int grade)
        {
            this.Id = id;
            this.name = name;
            this.age = age;
            this.grade = grade;
        }
    }

    public class StudentData
    {
        static string _connectionString = @"
        Data Source=localhost\SQLEXPRESS;
        Database=StudentsDB;
        Integrated Security=True;
        Persist Security Info=False;
        Pooling=False;
        MultipleActiveResultSets=False;
        Connect Timeout=30;
        Encrypt=False;
        TrustServerCertificate=False;
        Packet Size=4096;
        Command Timeout=0";

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentsList = new List<StudentDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
            }
            return StudentsList;
        }
    
        public static List<StudentDTO> GetPassedStudents()
        {
            var PassedStudentsList = new List<StudentDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PassedStudentsList.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))

                                )
                            );
                        }
                    }
                }
            }
            return PassedStudentsList;
        }
    
        public static decimal GetAverageGrade()
        {
            decimal averageGrade = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if(result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDecimal(result);
                    } 
                }
            }
            return averageGrade;
        }
    
        public static StudentDTO GetStudentById(int ID)
        {
            StudentDTO student = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("SP_GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", ID);
                    conn.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                );
                        }
                    }
                }
            }

            return student;
        }
    }
}
