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
        public StudentDTO() { }

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

        // Adds a new student using stored procedure SP_AddStudent
        // SP_AddStudent expects an OUTPUT parameter @NewStudentId; add it and read after ExecuteNonQuery
        public static int AddStudent(StudentDTO student)
        {
            int newId = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", student.name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Age", student.age);
                    cmd.Parameters.AddWithValue("@Grade", student.grade);

                    // Add the output parameter expected by the stored procedure
                    var outParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    conn.Open();
                    // Use ExecuteNonQuery when relying on OUTPUT parameters
                    cmd.ExecuteNonQuery();

                    if (outParam.Value != null && outParam.Value != DBNull.Value)
                    {
                        newId = Convert.ToInt32(outParam.Value);
                    }
                }
            }
            return newId;
        }
    }
}
