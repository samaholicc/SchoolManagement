using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Schedule : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        private static bool isTeacher;
        private DateTime currentWeekStart;

        public Schedule()
        {
            InitializeComponent();
            SetWeekStart(DateTime.Today);
            LoadSchedule();
        }

        public Schedule(bool b)
        {
            isTeacher = b;
            InitializeComponent();
            SetWeekStart(DateTime.Today);
            LoadSchedule();
        }

        private void SetWeekStart(DateTime date)
        {
            int daysOffset = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            currentWeekStart = date.Date.AddDays(-daysOffset);
            lblWeek.Text = $"{currentWeekStart:dd MMMM yyyy} - {currentWeekStart.AddDays(6):dd MMMM yyyy}";
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            dgvSchedule.Columns.Clear();
            dgvSchedule.Rows.Clear();

            // Define time slots (e.g., hourly from 08:00 to 18:00)
            string[] timeSlots = new string[]
            {
                "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                "12:00 - 13:00", "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00",
                "16:00 - 17:00", "17:00 - 18:00"
            };
          

            // Add "Time" column (fixed width, frozen, no Fill)
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Time",
                Name = "Time",
                Width = 100, // Fixed width
                Frozen = true // Keep visible while scrolling
                // Removed AutoSizeMode.Fill to avoid conflict
            });

            // Add day columns (use Fill for these)
            for (int i = 0; i < 6; i++)
            {
                DateTime day = currentWeekStart.AddDays(i);
                dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = $"{day:dddd, dd MMMM yyyy}",
                    Name = day.ToString("yyyy-MM-dd"),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Stretch to fill remaining width
                });
            }

            // Add rows to match time slots
            dgvSchedule.Rows.Add(timeSlots.Length);

            // Populate the time slot column
            for (int i = 0; i < timeSlots.Length; i++)
            {
                dgvSchedule.Rows[i].Cells[0].Value = timeSlots[i];
            }

            // Adjust row height to fill the grid vertically
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
                    row.Height = rowHeight; // Set uniform height to fill space
                }
            }
        }

        private void LoadSchedule()
        {
            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    string query = isTeacher
                        ? "SELECT A.SUB_ID, D.SUB_NAME, A.Schedule " +
                          "FROM Class A " +
                          "JOIN Subject D ON A.SUB_ID = D.SUB_ID " +
                          "WHERE A.Teacher_ID = @teacherID " +
                          "ORDER BY A.Schedule ASC"
                        : "SELECT A.SUB_ID, S.SUB_NAME, A.SCHEDULE " +
                          "FROM Class A " +
                          "JOIN Subject S ON A.SUB_ID = S.SUB_ID " +
                          "JOIN Student_Classes SC ON SC.CLASS_ID = A.CLASS_ID " +
                          "WHERE SC.STUDENT_ID = @ID_LG " +
                          "ORDER BY A.SCHEDULE ASC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(isTeacher ? "@teacherID" : "@ID_LG", Login.ID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine($"Initial row count: {dgvSchedule.Rows.Count}");

                            while (reader.Read())
                            {
                                int subId = reader.GetInt32(0);
                                string subName = reader.GetString(1);
                                string schedule = reader.GetString(2);

                                // Split the schedule into parts
                                string[] parts = schedule.Split(new string[] { " - " }, StringSplitOptions.None);
                                if (parts.Length != 2) continue;

                                string dateTimePart = parts[0].Trim();
                                string endTimePart = parts[1].Trim();

                                int lastSpaceIndex = dateTimePart.LastIndexOf(' ');
                                if (lastSpaceIndex == -1) continue;

                                string datePart = dateTimePart.Substring(0, lastSpaceIndex).Trim();
                                string startTimePart = dateTimePart.Substring(lastSpaceIndex + 1).Trim();

                                DateTime scheduleDate;
                                if (!DateTime.TryParseExact(datePart, "dddd, dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out scheduleDate))
                                {
                                    continue;
                                }

                                TimeSpan startTime, endTime;
                                if (!TimeSpan.TryParseExact(startTimePart, "hh\\:mm", CultureInfo.InvariantCulture, out startTime) ||
                                     !TimeSpan.TryParseExact(endTimePart, "hh\\:mm", CultureInfo.InvariantCulture, out endTime))
                                {
                                    continue;
                                }

                                // Ensure the schedule is within the current week
                                if (scheduleDate >= currentWeekStart && scheduleDate < currentWeekStart.AddDays(7))
                                {
                                    int dayIndex = (int)(scheduleDate.Date - currentWeekStart).TotalDays; // Fixed day index


                                    // Loop through the rows and check for time matching
                                    // Loop through the rows and check for time matching
                                    for (int row = 0; row < dgvSchedule.Rows.Count; row++)
                                    {
                                        if (dgvSchedule.Rows[row].Cells[0].Value != null)
                                        {
                                            string timeSlot = dgvSchedule.Rows[row].Cells[0].Value.ToString();

                                            // Split the time slot into start and end times
                                            string[] slotParts = timeSlot.Split(new string[] { " - " }, StringSplitOptions.None);
                                            TimeSpan slotStart = TimeSpan.Parse(slotParts[0]);
                                            TimeSpan slotEnd = TimeSpan.Parse(slotParts[1]);

                                            // Check if the fetched schedule falls within the current time slot (with overlap consideration)
                                            // The class is within the slot if its start time is before the slot end time
                                            // and its end time is after the slot start time
                                            if (startTime < slotEnd && endTime > slotStart)
                                            {
                                                dgvSchedule.Rows[row].Cells[dayIndex + 1].Value = $"{subId} - {subName}";
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
                MessageBox.Show($"Error: {ex.Message}");
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

        private void Schedule_Load(object sender, EventArgs e)
        {

        }

        private void Schedule_Load_1(object sender, EventArgs e)
        {

        }
    }
}