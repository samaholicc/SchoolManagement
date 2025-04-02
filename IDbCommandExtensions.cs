using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public static class IDbCommandExtensions
    {
        public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CommandBehavior behavior = CommandBehavior.Default, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return await Task.Run(() => command.ExecuteReader(behavior), cancellationToken);
        }

        public static async Task<object> ExecuteScalarAsync(this IDbCommand command, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return await Task.Run(() => command.ExecuteScalar(), cancellationToken);
        }

        public static async Task<int> ExecuteNonQueryAsync(this IDbCommand command, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return await Task.Run(() => command.ExecuteNonQuery(), cancellationToken);
        }
    }
}