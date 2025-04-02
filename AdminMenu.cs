using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagement
{
    public partial class MenuAdmin : KryptonForm
    {
        private const string MySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
        private readonly IServiceProvider _serviceProvider;

        public MenuAdmin(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
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
            // Navigate to the login screen using DI
            var login = _serviceProvider.GetService<Login>();
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
                        long count = (long)cmd.ExecuteScalar();
                        return (int)count;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
                return 0;
            }
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            var myProfile = _serviceProvider.GetService<AdminProfile>();
            myProfile.Show();
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            var student = _serviceProvider.GetService<StudentManager>();
            student.Show();
        }

        private void pbTeachers_Click(object sender, EventArgs e)
        {
            var teacher = _serviceProvider.GetService<TeacherManager>();
            teacher.Show();
        }

        private void pbClasses_Click(object sender, EventArgs e)
        {
            var classManager = _serviceProvider.GetService<ClassSectionManager>();
            classManager.Show();
        }

        private void pbSubjects_Click(object sender, EventArgs e)
        {
            var subjectManager = _serviceProvider.GetService<SubjectManager>();
            subjectManager.Show();
        }

        private void pbDepartment_Click(object sender, EventArgs e)
        {
            var departmentManager = _serviceProvider.GetService<DepartmentManager>();
            departmentManager.Show();
        }
    }
}