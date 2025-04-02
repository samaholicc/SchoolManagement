using MySql.Data.MySqlClient;
using SchoolManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SchoolManagement
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("SELECT DEP_ID, DEP_NAME FROM SYSTEM.DEP ORDER BY DEP_ID", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetString(reader.GetOrdinal("DEP_ID")), 
                            Name = reader.GetString(reader.GetOrdinal("DEP_NAME"))
                        });
                    }
                }
            }
            return departments;
        }

        public async Task<bool> AddDepartmentAsync(Department department)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("INSERT INTO SYSTEM.DEP (DEP_NAME) VALUES (@Name)", conn);
                cmd.Parameters.AddWithValue("@Name", department.Name);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("UPDATE SYSTEM.DEP SET DEP_NAME = @Name WHERE DEP_ID = @Id", conn);
                cmd.Parameters.AddWithValue("@Name", department.Name);
                cmd.Parameters.AddWithValue("@Id", department.Id); // Id is string
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task DeleteDepartmentAsync(string id) // Already string, no change needed
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("DELETE FROM SYSTEM.DEP WHERE DEP_ID = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}