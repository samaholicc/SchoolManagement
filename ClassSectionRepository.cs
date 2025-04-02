using System.Data.Common;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class ClassSectionRepository : IClassSectionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ClassSectionRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<ClassSection>> GetClassesAsync(int page, int pageSize)
        {
            var classes = new List<ClassSection>();
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return classes;
                }

                await conn.OpenAsync();

                // Check if required tables exist
                using (var checkTablesCmd = conn.CreateCommand())
                {
                    if (checkTablesCmd == null)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }

                    checkTablesCmd.CommandText =
                        "SELECT COUNT(*) FROM information_schema.tables " +
                        "WHERE table_schema = 'system' AND table_name IN ('class', 'subject', 'teacher')";
                    var tableCountResult = await checkTablesCmd.ExecuteScalarAsync();
                    if (tableCountResult == null || (long)tableCountResult != 3)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }
                }

                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }

                    cmd.CommandText =
                        "SELECT a.CLASS_ID, a.SUB_ID, s.SUB_NAME, a.TEACHER_ID, t.FULL_NAME, " +
                        "a.START_DATE, a.FINISH_DATE, a.SCHEDULE, a.NB_S " +
                        "FROM system.class a " +
                        "LEFT JOIN system.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN system.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "ORDER BY a.CLASS_ID ASC " +
                        "LIMIT @PageSize OFFSET @Offset";

                    var pageSizeParam = cmd.CreateParameter();
                    pageSizeParam.ParameterName = "@PageSize";
                    pageSizeParam.Value = pageSize;
                    cmd.Parameters.Add(pageSizeParam);

                    var offsetParam = cmd.CreateParameter();
                    offsetParam.ParameterName = "@Offset";
                    offsetParam.Value = (page - 1) * pageSize;
                    cmd.Parameters.Add(offsetParam);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null)
                        {
                            await conn.CloseAsync();
                            return classes;
                        }

                        var dbReader = (DbDataReader)reader; // Cast to DbDataReader
                        var columns = Enumerable.Range(0, dbReader.FieldCount).Select(i => dbReader.GetName(i)).ToList();
                        if (!columns.Contains("SUB_NAME") || !columns.Contains("FULL_NAME"))
                        {
                            await conn.CloseAsync();
                            return classes;
                        }

                        while (await dbReader.ReadAsync())
                        {
                            var classSection = MapClassSection(dbReader);
                            if (classSection != null)
                            {
                                classes.Add(classSection);
                            }
                        }
                    }
                }

                await conn.CloseAsync();
            }
            return classes;
        }

        public async Task<List<ClassSection>> GetClassesBySearchTermAsync(string searchTerm)
        {
            var classes = new List<ClassSection>();
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return classes;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }

                    cmd.CommandText =
                        "SELECT a.CLASS_ID, a.SUB_ID, s.SUB_NAME, a.TEACHER_ID, t.FULL_NAME, " +
                        "a.START_DATE, a.FINISH_DATE, a.SCHEDULE, a.NB_S " +
                        "FROM system.class a " +
                        "LEFT JOIN system.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN system.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "WHERE a.CLASS_ID LIKE @SearchTerm " +
                        "OR s.SUB_NAME LIKE @SearchTerm " +
                        "OR t.FULL_NAME LIKE @SearchTerm " +
                        "OR a.SCHEDULE LIKE @SearchTerm " +
                        "ORDER BY a.CLASS_ID ASC";

                    var searchTermParam = cmd.CreateParameter();
                    searchTermParam.ParameterName = "@SearchTerm";
                    searchTermParam.Value = $"%{searchTerm}%";
                    cmd.Parameters.Add(searchTermParam);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null)
                        {
                            await conn.CloseAsync();
                            return classes;
                        }

                        var dbReader = (DbDataReader)reader; // Cast to DbDataReader
                        while (await dbReader.ReadAsync())
                        {
                            var classSection = MapClassSection(dbReader);
                            if (classSection != null)
                            {
                                classes.Add(classSection);
                            }
                        }
                    }
                }

                await conn.CloseAsync();
            }
            return classes;
        }

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            var teachers = new List<Teacher>();
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return teachers;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return teachers;
                    }

                    cmd.CommandText = "SELECT TEACHER_ID, FULL_NAME FROM system.teacher ORDER BY FULL_NAME";

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null)
                        {
                            await conn.CloseAsync();
                            return teachers;
                        }

                        var dbReader = (DbDataReader)reader; // Cast to DbDataReader
                        while (await dbReader.ReadAsync())
                        {
                            try
                            {
                                teachers.Add(new Teacher
                                {
                                    Id = dbReader.GetString(0),
                                    FullName = dbReader.GetString(1)
                                });
                            }
                            catch
                            {
                                // Skip invalid rows
                            }
                        }
                    }
                }

                await conn.CloseAsync();
            }
            return teachers;
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = new List<Subject>();
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return subjects;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return subjects;
                    }

                    cmd.CommandText = "SELECT SUB_ID, SUB_NAME FROM system.subject ORDER BY SUB_NAME";

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null)
                        {
                            await conn.CloseAsync();
                            return subjects;
                        }

                        var dbReader = (DbDataReader)reader; // Cast to DbDataReader
                        while (await dbReader.ReadAsync())
                        {
                            try
                            {
                                subjects.Add(new Subject
                                {
                                    Id = dbReader.GetInt32(0),
                                    Name = dbReader.GetString(1)
                                });
                            }
                            catch
                            {
                                // Skip invalid rows
                            }
                        }
                    }
                }

                await conn.CloseAsync();
            }
            return subjects;
        }

        public async Task<bool> AddClassAsync(ClassSection classSection)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return false;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return false;
                    }

                    cmd.CommandText = "SP_CLASS_ADD";
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddClassParameters(cmd, classSection);

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        await conn.CloseAsync();
                        return false;
                    }
                }

                await conn.CloseAsync();
            }
            return true;
        }

        public async Task<bool> UpdateClassAsync(ClassSection classSection)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return false;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return false;
                    }

                    cmd.CommandText = "SP_CLASS_UPDATE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddClassParameters(cmd, classSection);

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        await conn.CloseAsync();
                        return false;
                    }
                }

                await conn.CloseAsync();
            }
            return true;
        }

        public async Task DeleteClassAsync(string classId)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return;
                }

                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return;
                    }

                    cmd.CommandText = "DELETE FROM system.class WHERE CLASS_ID = @ClassID";
                    var classIdParam = cmd.CreateParameter();
                    classIdParam.ParameterName = "@ClassID";
                    classIdParam.Value = classId;
                    cmd.Parameters.Add(classIdParam);

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        // Silently ignore errors
                    }
                }

                await conn.CloseAsync();
            }
        }

        public async Task<List<ClassSection>> GetAllClassesAsync()
        {
            var classes = new List<ClassSection>();
            using (var conn = _connectionFactory.CreateConnection())
            {
                if (conn == null)
                {
                    return classes;
                }

                await conn.OpenAsync();

                // Check if required tables exist
                using (var checkTablesCmd = conn.CreateCommand())
                {
                    if (checkTablesCmd == null)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }

                    checkTablesCmd.CommandText =
                        "SELECT COUNT(*) FROM information_schema.tables " +
                        "WHERE table_schema = 'system' AND table_name IN ('class', 'subject', 'teacher')";
                    var tableCountResult = await checkTablesCmd.ExecuteScalarAsync();
                    if (tableCountResult == null || (long)tableCountResult != 3)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }
                }

                using (var cmd = conn.CreateCommand())
                {
                    if (cmd == null)
                    {
                        await conn.CloseAsync();
                        return classes;
                    }

                    cmd.CommandText =
                        "SELECT a.CLASS_ID, a.SUB_ID, s.SUB_NAME, a.TEACHER_ID, t.FULL_NAME, " +
                        "a.START_DATE, a.FINISH_DATE, a.SCHEDULE, a.NB_S " +
                        "FROM system.class a " +
                        "LEFT JOIN system.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN system.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "ORDER BY a.CLASS_ID ASC";

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader == null)
                        {
                            await conn.CloseAsync();
                            return classes;
                        }

                        var dbReader = (DbDataReader)reader; // Cast to DbDataReader
                        var columns = Enumerable.Range(0, dbReader.FieldCount).Select(i => dbReader.GetName(i)).ToList();
                        if (!columns.Contains("SUB_NAME") || !columns.Contains("FULL_NAME"))
                        {
                            await conn.CloseAsync();
                            return classes;
                        }

                        while (await dbReader.ReadAsync())
                        {
                            var classSection = MapClassSection(dbReader);
                            if (classSection != null)
                            {
                                classes.Add(classSection);
                            }
                        }
                    }
                }

                await conn.CloseAsync();
            }
            return classes;
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
                            return null; // This is being hit
                        }
                    }
                }
            }
        }

        private ClassSection MapClassSection(DbDataReader reader)
        {
            try
            {
                return new ClassSection
                {
                    ClassId = reader.GetString(reader.GetOrdinal("CLASS_ID")),
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SUB_ID")),
                    SubjectName = reader.IsDBNull(reader.GetOrdinal("SUB_NAME")) ? "" : reader.GetString(reader.GetOrdinal("SUB_NAME")),
                    TeacherId = reader.GetString(reader.GetOrdinal("TEACHER_ID")),
                    TeacherName = reader.IsDBNull(reader.GetOrdinal("FULL_NAME")) ? "" : reader.GetString(reader.GetOrdinal("FULL_NAME")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("START_DATE")),
                    FinishDate = reader.GetDateTime(reader.GetOrdinal("FINISH_DATE")),
                    Schedule = reader.GetString(reader.GetOrdinal("SCHEDULE")),
                    NumberOfStudents = reader.GetInt32(reader.GetOrdinal("NB_S"))
                };
            }
            catch
            {
                return null;
            }
        }

        private void AddClassParameters(IDbCommand cmd, ClassSection classSection)
        {
            var subIdParam = cmd.CreateParameter();
            subIdParam.ParameterName = "p_SUB_ID";
            subIdParam.Value = classSection.SubjectId;
            cmd.Parameters.Add(subIdParam);

            var teacherIdParam = cmd.CreateParameter();
            teacherIdParam.ParameterName = "p_TEACHER_ID";
            teacherIdParam.Value = classSection.TeacherId;
            cmd.Parameters.Add(teacherIdParam);

            var startDateParam = cmd.CreateParameter();
            startDateParam.ParameterName = "p_START_DATE";
            startDateParam.Value = classSection.StartDate;
            cmd.Parameters.Add(startDateParam);

            var finishDateParam = cmd.CreateParameter();
            finishDateParam.ParameterName = "p_FINISH_DATE";
            finishDateParam.Value = classSection.FinishDate;
            cmd.Parameters.Add(finishDateParam);

            var scheduleParam = cmd.CreateParameter();
            scheduleParam.ParameterName = "p_SCHEDULE";
            scheduleParam.Value = classSection.Schedule;
            cmd.Parameters.Add(scheduleParam);

            var nbSParam = cmd.CreateParameter();
            nbSParam.ParameterName = "p_NB_S";
            nbSParam.Value = classSection.NumberOfStudents;
            cmd.Parameters.Add(nbSParam);

            var classIdParam = cmd.CreateParameter();
            classIdParam.ParameterName = "p_CLASS_ID";
            classIdParam.Value = classSection.ClassId;
            cmd.Parameters.Add(classIdParam);
        }
    }
}