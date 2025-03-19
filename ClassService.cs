using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

public class ClassService
{
    private readonly string _connectionString;

    // Constructor to initialize the connection string
    public ClassService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Example method to add: GetClassesBySearchTermAsync
    public async Task<DataTable> GetClassesBySearchTermAsync(string searchTerm)
    {
        var dataTable = new DataTable();
        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            // SQL query to search class sections by CLASS_ID or Subject name
            var query = @"
                SELECT 
                    a.CLASS_ID AS 'CLASS SECTION ID',
                    CONCAT(a.Sub_ID, ' - ', s.SUB_NAME) AS 'Subject',
                    CONCAT(a.TEACHER_ID, ' - ', t.Full_name) AS 'Teacher',
                    a.START_DATE AS 'START',
                    a.FINISH_DATE AS 'FINISH',
                    a.SCHEDULE AS 'SCHEDULE',
                    a.NB_S AS 'N.O.S'
                FROM SYSTEM.CLASS a
                JOIN SYSTEM.SUBJECT s ON a.Sub_ID = s.SUB_ID
                JOIN SYSTEM.TEACHER t ON a.TEACHER_ID = t.TEACHER_ID
                WHERE a.CLASS_ID LIKE @SearchTerm 
                   OR s.SUB_NAME LIKE @SearchTerm";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
        }
        return dataTable;
    }

    // Other existing methods in ClassService can remain unchanged
}
