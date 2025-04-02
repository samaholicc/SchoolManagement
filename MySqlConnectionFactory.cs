using MySql.Data.MySqlClient;
using System;

namespace SchoolManagement
{
    public interface IDbConnectionFactory
    {
        IDbConnectionWrapper CreateConnection();
    }

    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            _connectionString = connectionString;
        }

        public IDbConnectionWrapper CreateConnection()
        {
            return new DbConnectionWrapper(new MySqlConnection(_connectionString));
        }
    }
}