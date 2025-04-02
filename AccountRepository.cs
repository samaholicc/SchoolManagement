using System;
using System.Threading.Tasks;
using System.Data.Common;

namespace SchoolManagement
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AccountRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory; // No throw, allow null
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            using (var conn = _connectionFactory?.CreateConnection())
            {
                if (conn == null)
                {
                    return null; // Return null if connection factory or connection is null
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return null; // Return null if command creation fails
                    }

                    cmd.CommandText = "SELECT * FROM ACCOUNT WHERE ID = @id";
                    var idParam = cmd.CreateParameter();
                    idParam.ParameterName = "@id";
                    idParam.Value = id ?? (object)DBNull.Value; // Handle null id
                    cmd.Parameters.Add(idParam);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null || !await reader.ReadAsync())
                        {
                            await conn.CloseAsync();
                            return null; // Return null if reader is null or no rows
                        }

                        try
                        {
                            return new Account
                            {
                                Id = reader.GetString(reader.GetOrdinal("ID")),
                                FullName = reader.GetString(reader.GetOrdinal("FULL_NAME")),
                                Password = reader.GetString(reader.GetOrdinal("PASSWORD")),
                                Role = reader.GetString(reader.GetOrdinal("ROLE"))
                            };
                        }
                        catch
                        {
                            await conn.CloseAsync();
                            return null; // Return null if mapping fails
                        }
                    }
                }
            }
        }

        public async Task<bool> UpdatePasswordAsync(string id, string hashedPassword)
        {
            if (id == null || hashedPassword == null) // Add explicit null check
            {
                return false;
            }

            using (var conn = _connectionFactory?.CreateConnection())
            {
                if (conn == null)
                {
                    return false; // Return false if connection factory or connection is null
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return false; // Return false if command creation fails
                    }

                    cmd.CommandText = "UPDATE ACCOUNT SET PASSWORD = @password WHERE ID = @id";
                    var passwordParam = cmd.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = hashedPassword ?? (object)DBNull.Value; // Handle null password (already handled by the check above)
                    cmd.Parameters.Add(passwordParam);

                    var idParam = cmd.CreateParameter();
                    idParam.ParameterName = "@id";
                    idParam.Value = id ?? (object)DBNull.Value; // Handle null id (already handled by the check above)
                    cmd.Parameters.Add(idParam);

                    try
                    {
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        await conn.CloseAsync();
                        return rowsAffected > 0;
                    }
                    catch
                    {
                        await conn.CloseAsync();
                        return false; // Return false if execution fails
                    }
                }
            }
        }
    }
}