using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access

namespace SchoolManagement
{
    public partial class StudentMenu : KryptonForm
    {
        public StudentMenu()
        {
            InitializeComponent();
            LoadInfo();
           
        }

        private void LoadInfo()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM STUDENTSTABLE WHERE STUDENT_ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();

                                
                                string studentName = dr.GetString(1); 
                                
                                // Afficher le nom de l'étudiant  
                                lbHello.Text = "Hello, Student " + studentName;
                            }
                            
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void pbLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void pbGrade_Click(object sender, EventArgs e)
        {
            StudentGrade studentGrade = new StudentGrade();
            studentGrade.Show();
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            StudentProfile studentProfile = new StudentProfile();
            studentProfile.ShowDialog();
        }

        private void pbCalendar_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule(false);
            schedule.Show();
        }

        private void StudentMenu_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here if needed
        }
    }
}