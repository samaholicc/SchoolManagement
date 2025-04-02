using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.DependencyInjection; // Add this for IServiceProvider

namespace SchoolManagement
{
    public partial class StudentMenu : KryptonForm
    {
        private readonly IServiceProvider _serviceProvider;

        public StudentMenu(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
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
            var login = _serviceProvider.GetService<Login>(); // Use DI to get Login
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void pbGrade_Click(object sender, EventArgs e)
        {
            var studentGrade = _serviceProvider.GetService<StudentGrade>(); // Use DI
            studentGrade.Show();
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            var studentProfile = _serviceProvider.GetService<StudentProfile>(); // Use DI
            studentProfile.ShowDialog();
        }

        private void pbCalendar_Click(object sender, EventArgs e)
        {
            var schedule = _serviceProvider.GetService<Schedule>(); // Use DI
            schedule.Show();
        }

        private void StudentMenu_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here if needed
        }

        private void lbHello_Click(object sender, EventArgs e)
        {
            // Empty for now
        }
    }
}