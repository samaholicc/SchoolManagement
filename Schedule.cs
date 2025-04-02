using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing; // Added for Color
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Schedule : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private static bool isTeacher;
        private DateTime currentWeekStart;
        private string studentId;
        private Dictionary<int, Color> classColors; // Dictionary to map SUB_ID to a color

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public Schedule()
        {
            InitializeComponent();
            isTeacher = false; // Par défaut, étudiant
            studentId = Login.ID;
            classColors = new Dictionary<int, Color>(); // Initialize the color dictionary
            ConfigureDateTimePicker();
            SetWeekStart(DateTime.Today);
            LoadSchedule();
        }

        public Schedule(bool isTeacherRole, string studentId = null)
        {
            InitializeComponent();
            isTeacher = isTeacherRole;
            this.studentId = studentId ?? Login.ID;
            classColors = new Dictionary<int, Color>(); // Initialize the color dictionary
            ConfigureDateTimePicker();
            SetWeekStart(DateTime.Today);
            LoadSchedule();
        }

        
        private void ConfigureDateTimePicker()
        {
            kryptonDateTimePicker.Format = DateTimePickerFormat.Custom;
            kryptonDateTimePicker.CustomFormat = "dd, dddd dd MMMM yyyy";
        }

        private void SetWeekStart(DateTime date)
        {
            int daysOffset = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            currentWeekStart = date.Date.AddDays(-daysOffset);
            lblWeek.Text = String.Format("{0:dd MMMM yyyy} - {1:dd MMMM yyyy}", currentWeekStart, currentWeekStart.AddDays(6));
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvSchedule.Columns.Clear();
            dgvSchedule.Rows.Clear();

            string[] timeSlots = new string[]
            {
                "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                "12:00 - 13:00", "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00",
                "16:00 - 17:00", "17:00 - 18:00"
            };

            DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Time",
                Name = "Time",
                Width = 100,
                Frozen = true
            };
            dgvSchedule.Columns.Add(timeColumn);

            for (int i = 0; i < 6; i++)
            {
                DateTime day = currentWeekStart.AddDays(i);
                DataGridViewTextBoxColumn dayColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = String.Format("{0:dddd, dd MMMM yyyy}", day),
                    Name = day.ToString("yyyy-MM-dd"),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                dgvSchedule.Columns.Add(dayColumn);
            }

            dgvSchedule.Rows.Add(timeSlots.Length);

            for (int i = 0; i < timeSlots.Length; i++)
            {
                dgvSchedule.Rows[i].Cells[0].Value = timeSlots[i];
            }

            AdjustRowHeights();
        }

        private void AdjustRowHeights()
        {
            int headerHeight = dgvSchedule.ColumnHeadersHeight;
            int totalHeight = dgvSchedule.Height - headerHeight;
            int rowCount = dgvSchedule.Rows.Count;
            if (rowCount > 0)
            {
                int rowHeight = totalHeight / rowCount;
                foreach (DataGridViewRow row in dgvSchedule.Rows)
                {
                    row.Height = rowHeight;
                }
            }
        }

        private void LoadSchedule()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = isTeacher
                        ? "SELECT A.SUB_ID, D.SUB_NAME, A.SCHEDULE, T.FULL_NAME " +
                          "FROM SYSTEM.Class A " +
                          "JOIN SYSTEM.Subject D ON A.SUB_ID = D.SUB_ID " +
                          "JOIN SYSTEM.teacher T ON A.TEACHER_ID = T.TEACHER_ID " +
                          "WHERE A.TEACHER_ID = @teacherID " +
                          "ORDER BY A.SCHEDULE ASC"
                        : "SELECT A.SUB_ID, S.SUB_NAME, A.SCHEDULE, T.FULL_NAME " +
                          "FROM SYSTEM.Class A " +
                          "JOIN SYSTEM.Subject S ON A.SUB_ID = S.SUB_ID " +
                          "JOIN SYSTEM.Student_Classes SC ON SC.CLASS_ID = A.CLASS_ID " +
                          "JOIN SYSTEM.teacher T ON A.TEACHER_ID = T.TEACHER_ID " +
                          "WHERE SC.STUDENT_ID = @ID_LG " +
                          "ORDER BY A.SCHEDULE ASC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        string idToUse = isTeacher ? Login.ID : studentId;
                        cmd.Parameters.AddWithValue(isTeacher ? "@teacherID" : "@ID_LG", idToUse);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int rowCount = 0;
                            while (reader.Read())
                            {
                                rowCount++;
                                int subId = reader.GetInt32(0);
                                string subName = reader.GetString(1);
                                string schedule = reader.GetString(2);
                                string teacherName = reader.GetString(3); // Fetch teacher name


                                string[] parts = schedule.Split(new string[] { " - " }, StringSplitOptions.None);
                                if (parts.Length != 2)
                                {
                                    continue;
                                }

                                string dateTimePart = parts[0].Trim();
                                string endTimePart = parts[1].Trim();

                                // Debug the split parts

                                int lastSpaceIndex = dateTimePart.LastIndexOf(' ');
                                if (lastSpaceIndex == -1)
                                {
                                    continue;
                                }

                                string datePart = dateTimePart.Substring(0, lastSpaceIndex).Trim();
                                string startTimePart = dateTimePart.Substring(lastSpaceIndex + 1).Trim();

                                DateTime scheduleDate;
                                bool dateParsed = DateTime.TryParseExact(datePart, "dddd, dd MMMM yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out scheduleDate);
                                if (!dateParsed)
                                {
                                    continue;
                                }

                                // Clean up startTimePart and endTimePart to remove any unexpected characters
                                startTimePart = startTimePart.Replace("+", "").Trim();
                                endTimePart = endTimePart.Replace("+", "").Trim();

                                TimeSpan startTime;
                                bool startTimeParsed = TimeSpan.TryParseExact(startTimePart, "HH:mm", CultureInfo.InvariantCulture, out startTime);
                                TimeSpan endTime;
                                bool endTimeParsed = TimeSpan.TryParseExact(endTimePart, "HH:mm", CultureInfo.InvariantCulture, out endTime);
                                if (!startTimeParsed || !endTimeParsed)
                                {
                                    // Fallback: Try parsing without strict format
                                    startTimeParsed = TimeSpan.TryParse(startTimePart, CultureInfo.InvariantCulture, out startTime);
                                    endTimeParsed = TimeSpan.TryParse(endTimePart, CultureInfo.InvariantCulture, out endTime);
                                    if (!startTimeParsed || !endTimeParsed)
                                    {
                                        continue;
                                    }
                                }

                                if (scheduleDate >= currentWeekStart && scheduleDate < currentWeekStart.AddDays(7))
                                {
                                    int dayIndex = (int)(scheduleDate - currentWeekStart).TotalDays;

                                    // Assign a color to the class if not already assigned
                                    if (!classColors.ContainsKey(subId))
                                    {
                                        // Generate a random pastel color for the class
                                        Random rand = new Random(subId); // Use subId as seed for consistency
                                        classColors[subId] = Color.FromArgb(255, rand.Next(150, 255), rand.Next(150, 255), rand.Next(150, 255));
                                    }

                                    for (int row = 0; row < dgvSchedule.Rows.Count; row++)
                                    {
                                        if (dgvSchedule.Rows[row].Cells[0].Value != null)
                                        {
                                            string timeSlot = dgvSchedule.Rows[row].Cells[0].Value.ToString();
                                            string[] slotParts = timeSlot.Split(new string[] { " - " }, StringSplitOptions.None);
                                            TimeSpan slotStart = TimeSpan.Parse(slotParts[0]);
                                            TimeSpan slotEnd = TimeSpan.Parse(slotParts[1]);

                                            if (startTime < slotEnd && endTime > slotStart)
                                            {
                                                // Include teacher name in the display
                                                dgvSchedule.Rows[row].Cells[dayIndex + 1].Value = String.Format("{0} - {1} ({2})", subId, subName, teacherName);
                                                // Set the background color of the cell
                                                dgvSchedule.Rows[row].Cells[dayIndex + 1].Style.BackColor = classColors[subId];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error: {0}", ex.Message));
            }
        }

        private void BtnPrevWeek_Click(object sender, EventArgs e)
        {
            SetWeekStart(currentWeekStart.AddDays(-7));
            LoadSchedule();
        }

        private void BtnNextWeek_Click(object sender, EventArgs e)
        {
            SetWeekStart(currentWeekStart.AddDays(7));
            LoadSchedule();
        }

        private void KryptonDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            SetWeekStart(kryptonDateTimePicker.Value);
            LoadSchedule();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ExportToCsvAsync();
        }

        #region Exportation vers CSV
        private async void ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    Title = "Save Schedule as CSV",
                    FileName = String.Format("Schedule_{0}_{1}_{2:yyyyMMdd}", isTeacher ? "Teacher" : "Student", studentId, currentWeekStart)
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchScheduleDataAsync();

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();

                        string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                            .Select(column => String.Format("\"{0}\"", column.ColumnName))
                            .ToArray();
                        csvContent.AppendLine(String.Join(",", columnNames));

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string[] fields = row.ItemArray.Select(field =>
                                String.Format("\"{0}\"", field != null ? field.ToString().Replace("\"", "\"\"") : ""))
                                .ToArray();
                            csvContent.AppendLine(String.Join(",", fields));
                        }

                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                        MessageBox.Show(GetLocalizedMessage("ExportSuccess"));
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show(GetLocalizedMessage("NoDataToExport"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}: {1}", GetLocalizedMessage("ErrorExporting"), ex.Message));
            }
        }

        private async Task<DataTable> FetchScheduleDataAsync()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = isTeacher
                        ? "SELECT A.SUB_ID, D.SUB_NAME, A.Schedule, T.FULL_NAME " +
                          "FROM SYSTEM.Class A " +
                          "JOIN SYSTEM.Subject D ON A.SUB_ID = D.SUB_ID " +
                          "JOIN SYSTEM.teacher T ON A.TEACHER_ID = T.TEACHER_ID " +
                          "WHERE A.Teacher_ID = @teacherID " +
                          "AND A.START_DATE >= @WeekStart AND A.START_DATE < @WeekEnd " +
                          "ORDER BY A.Schedule ASC"
                        : "SELECT A.SUB_ID, S.SUB_NAME, A.SCHEDULE, T.FULL_NAME " +
                          "FROM SYSTEM.Class A " +
                          "JOIN SYSTEM.Subject S ON A.SUB_ID = S.SUB_ID " +
                          "JOIN SYSTEM.Student_Classes SC ON SC.CLASS_ID = A.CLASS_ID " +
                          "JOIN SYSTEM.teacher T ON A.TEACHER_ID = T.TEACHER_ID " +
                          "WHERE SC.STUDENT_ID = @ID_LG " +
                          "AND A.START_DATE >= @WeekStart AND A.START_DATE < @WeekEnd " +
                          "ORDER BY A.SCHEDULE ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(isTeacher ? "@teacherID" : "@ID_LG", studentId);
                        cmd.Parameters.AddWithValue("@WeekStart", currentWeekStart);
                        cmd.Parameters.AddWithValue("@WeekEnd", currentWeekStart.AddDays(7));

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            await Task.Run(() => adapter.Fill(dataTable));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}: {1}", GetLocalizedMessage("ErrorFetchingData"), ex.Message));
            }
            return dataTable;
        }
        #endregion

        #region Localisation
        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "ErrorExporting", "Erreur lors de l'exportation vers CSV" },
                    { "ExportSuccess", "Exporté avec succès vers CSV" },
                    { "NoDataToExport", "Aucune donnée à exporter" },
                    { "ErrorFetchingData", "Erreur lors de la récupération des données" }
                }
                : new Dictionary<string, string>
                {
                    { "ErrorExporting", "Error exporting to CSV" },
                    { "ExportSuccess", "Exported successfully to CSV" },
                    { "NoDataToExport", "No data to export" },
                    { "ErrorFetchingData", "Error fetching data" }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        private void lblWeek_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}