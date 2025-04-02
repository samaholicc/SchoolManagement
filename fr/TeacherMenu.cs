using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class TeacherMenu : KryptonForm
    {
        public TeacherMenu()
        {
            InitializeComponent();
            LoadInfo();
            
        }

        private void TeacherMenu_Load(object sender, EventArgs e)
        {
            // Load additional data if needed  
        }

        private void LoadInfo()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // Assume that Login.ID contains the Teacher ID  
                    using (MySqlCommand cmd = new MySqlCommand("SELECT FULL_NAME FROM TEACHER WHERE TEACHER_ID = @ID_LG", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_LG", Login.ID);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                lbHello.Text = "Hello, Teacher " + dr.GetString(0);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                RedirectToLogin(); // Redirect also on exceptions  
            }
        }

        private void RedirectToLogin()
        {
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void pbLogout_Click(object sender, EventArgs e)
        {
            RedirectToLogin();
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            TeacherProfile teacherProfile = new TeacherProfile();
            teacherProfile.ShowDialog();
        }

        private void pbSection_Click(object sender, EventArgs e)
        {
            
            TeacherClassSection teacherClassSection = new TeacherClassSection();
            teacherClassSection.Show();
        }

        private void pbCalendar_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule(true);
            schedule.Show();
        }

        private void lbHello_Click(object sender, EventArgs e)
        {
            // Additional actions for when the hello label is clicked, if needed  
        }
    }
}