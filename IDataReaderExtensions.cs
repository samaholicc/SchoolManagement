using System;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public static class IDataReaderExtensions
    {
        public static async Task<bool> ReadAsync(this IDataReader reader, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            // Wrap the synchronous Read call in a Task for simplicity
            return await Task.Run(() => reader.Read(), cancellationToken);
        }
    }
}