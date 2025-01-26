using MySql.Data.MySqlClient; // Ensure you have the MySQL data access library
using System;
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
                    string query;

                    if (isTeacher)
                    {
                        query = "SELECT A.SUB_ID, D.SUB_NAME, D.Credits, A.Schedule " +
                                "FROM Class A " +
                                "JOIN Subject D ON A.SUB_ID = D.SUB_ID " +
                                "WHERE A.Teacher_ID = @teacherID " +
                                "ORDER BY A.Schedule ASC";
                    }
                    else
                    {
                        query = "SELECT B.SUB_ID, C.SUB_NAME, B.Schedule " +
                                "FROM results A " +
                                "JOIN Class B ON A.CLASS_ID = B.CLASS_ID " +
                                "JOIN Subject C ON B.SUB_ID = C.SUB_ID " +
                                "WHERE A.STUDENT_ID = @ID_LG " +
                                "ORDER BY B.Schedule ASC";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (isTeacher)
                        {
                            cmd.Parameters.AddWithValue("@teacherID", Login.ID);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ID_LG", Login.ID);
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string sname;
                                if (isTeacher)
                                {
                                    // Retrieve SUB_ID as int and Schedule as string  
                                    int subId = reader.GetInt32(0);
                                    string subName = reader.GetString(1);
                                    sname = reader.GetString(3); // Schedule  

                                    // Check which day the schedule corresponds to, and add it to the correct text box  
                                    if (sname.Contains("Lundi"))
                                    {
                                        txtMonday.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Mardi"))
                                    {
                                        txtTuesday.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Mercredi"))
                                    {
                                        txtWed.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Jeudi"))
                                    {
                                        txtThurs.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Vendredi"))
                                    {
                                        txtFri.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Samedi"))
                                    {
                                        txtSat.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                }
                                else
                                {
                                    // Retrieve SUB_ID as int and Schedule as string  
                                    int subId = reader.GetInt32(0);
                                    string subName = reader.GetString(1);
                                    sname = reader.GetString(2); // Schedule

                                    // Check which day the schedule corresponds to, and add it to the correct text box  
                                    if (sname.Contains("Lundi"))
                                    {
                                        txtMonday.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Mardi"))
                                    {
                                        txtTuesday.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Mercredi"))
                                    {
                                        txtWed.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Jeudi"))
                                    {
                                        txtThurs.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Vendredi"))
                                    {
                                        txtFri.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                    else if (sname.Contains("Samedi"))
                                    {
                                        txtSat.Text += $"{subId} - {subName}\n{sname}\n\n\n";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("Error: " + es.Message + "\n" + es.StackTrace);
            }
        }
        private void Schedule_Load(object sender, EventArgs e)
        {

        }
    }
}