using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class MenuAdmin : KryptonForm
    {
        private const string MySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

        public MenuAdmin()
        {
            InitializeComponent();
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            LoadTotals();
          




        }

        private void pbLogout_Click(object sender, EventArgs e)
        {
            LogOut();
        }
     
        private void LogOut()
        {
            // Navigate to the login screen
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Loads all total counts for students, teachers, classes, and subjects.
        /// </summary>
        private void LoadTotals()
        {
            lbToTalStudent.Text = GetCount("SELECT COUNT(STUDENT_ID) FROM STUDENTSTABLE").ToString();
            lbTotalTeacher.Text = GetCount("SELECT COUNT(TEACHER_ID) FROM TEACHER").ToString();
            lbTotalClass.Text = GetCount("SELECT COUNT(CLASS_ID) FROM CLASS").ToString();
            lbTotalSubject.Text = GetCount("SELECT COUNT(SUB_ID) FROM SUBJECT").ToString();
        }

        /// <summary>
        /// Generic method to fetch a count from a database table.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <returns>Total count as an integer.</returns>
        private int GetCount(string query)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySqlDb))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Cast the result to a long (MySQL COUNT returns long)
                        long count = (long)cmd.ExecuteScalar();
                        return (int)count;
                    }
                }
            }
            catch (Exception ex)
            {
                // Use string concatenation instead of interpolation for C# 4.0 compatibility
                MessageBox.Show("Error fetching data: " + ex.Message);
                return 0; // Return 0 in case of an error
            }
        }


        private void pbProfile_Click(object sender, EventArgs e)
        {
            AdminProfile myProfile = new AdminProfile();
            myProfile.Show();
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            StudentManager student = new StudentManager();
            student.Show();
        }

        private void pbTeachers_Click(object sender, EventArgs e)
        {
            TeacherManager teacher = new TeacherManager();
            teacher.Show();
        }

        

        private void pbClasses_Click(object sender, EventArgs e)
        {
            ClassSectionManager classManager = new ClassSectionManager();
            classManager.Show();
        }

        private void pbSubjects_Click(object sender, EventArgs e)
        {
            SubjectManager subjectManager = new SubjectManager();
            subjectManager.Show();
        }

        private void pbDepartment_Click(object sender, EventArgs e)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            departmentManager.Show();
        }

        private void menu4_Click(object sender, EventArgs e)
        {

        }

        private void lbClasses_Click(object sender, EventArgs e)
        {

        }
    }
}
