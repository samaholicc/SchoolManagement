using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class DbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly DbConnection _connection;
        private bool _disposed;

        public DbConnectionWrapper(DbConnection connection)
        {
            _connection = connection; // Can be null as per your previous request
            _disposed = false;
        }

        public IDbCommand CreateCommand() // Returns IDbCommand
        {
            if (_disposed || _connection == null)
            {
                return null; // Return null instead of throwing
            }
            return _connection.CreateCommand(); // DbCommand implements IDbCommand
        }

        public void Open()
        {
            if (_disposed || _connection == null)
            {
                return; // Silently return instead of throwing
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void Close()
        {
            if (_disposed || _connection == null)
            {
                return; // Silently return
            }
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public async Task OpenAsync()
        {
            if (_disposed || _connection == null)
            {
                return; // Silently return instead of throwing
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await ((MySql.Data.MySqlClient.MySqlConnection)_connection).OpenAsync();
            }
        }

        public async Task CloseAsync()
        {
            if (_disposed || _connection == null)
            {
                return; // Silently return
            }
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                await ((MySql.Data.MySqlClient.MySqlConnection)_connection).CloseAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }

            _disposed = true;
        }

        ~DbConnectionWrapper()
        {
            Dispose(false);
        }
    }
}