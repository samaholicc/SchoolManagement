using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Ensure you have the MySQL data access library
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Schedule : Form // Assuming you are using a Form  
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

        public Schedule()
        {
            InitializeComponent();
            LoadSchedule();
            
        }

        public Schedule(bool b)
        {
            isTeacher = b;
            InitializeComponent();
            LoadSchedule();
         
        }

        private void LoadSchedule()
        {
            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // If the user is a teacher, get the schedule based on Teacher_ID
                    // If the user is a student, get the schedule based on STUDENT_ID
                    string query = isTeacher
                        ? "_Lingobit_SELECT A.SUB_ID, D.SUB_NAME, A.Schedule " +
                          "FROM Class A " +
                          "JOIN Subject D ON A.SUB_ID = D.SUB_ID " +
                          "WHERE A.Teacher_ID = @teacherID " +
                          "ORDER BY A.Schedule ASC"
                        : "SELECT A.SUB_ID, S.SUB_NAME, A.SCHEDULE " +
                          "FROM Class A " +
                          "JOIN Subject S ON A.SUB_ID = S.SUB_ID " +
                          "JOIN Student_Classes SC ON SC.CLASS_ID = A.CLASS_ID " +
                          "WHERE SC.STUDENT_ID = @ID_LG " + // For students, we use their STUDENT_ID
                          "ORDER BY A.SCHEDULE ASC;";

                    // Adding the parameter depending on whether it's a teacher or student
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(isTeacher ? "@teacherID" : "@ID_LG", Login.ID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Dictionary to store the schedule based on the day of the week
                            Dictionary<string, KryptonRichTextBox> scheduleTextBoxes = new Dictionary<string, KryptonRichTextBox>
                    {
                        { "Lundi", txtMonday },
                        { "Mardi", txtTuesday },
                        { "Mercredi", txtWed },
                        { "Jeudi", txtThurs },
                        { "Vendredi", txtFri },
                        { "Samedi", txtSat }
                    };

                            // Reading each row and processing the data
                            while (reader.Read())
                            {
                                int subId = reader.GetInt32(0); // Subject ID
                                string subName = reader.GetString(1); // Subject Name
                                string schedule = reader.GetString(2); // Schedule (e.g., "Lundi 08:00-10:00")

                                // Splitting the schedule string into day and time
                                string[] parts = schedule.Split(' ');
                                string day = parts[0].Trim(); // Day of the week
                                string time = parts.Length > 1 ? parts[1].Trim() : ""; // Time

                                // If the day exists in our dictionary, append the schedule details
                                if (scheduleTextBoxes.ContainsKey(day))
                                {
                                    scheduleTextBoxes[day].Text += $"_Lingobit_{subId} - {subName}\n{time}\n\n";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("Erreur : " + es.Message + "\n" + es.StackTrace);
            }
        }

        private void Schedule_Load(object sender, EventArgs e)
        {

        }

        private void txtMonday_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtThurs_TextChanged(object sender, EventArgs e)
        {

        }
    }
}