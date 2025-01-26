using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class StudentClassList : KryptonForm
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

        private string classSectionID;  // Declare classSectionID as a field

        public StudentClassList(string classSectionID)
        {
            InitializeComponent();
            this.classSectionID = classSectionID;
            ClassManager.ClassSectionID = classSectionID;
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                cbStudents.Items.Clear(); // Clear existing items before reloading

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    // Corrected the SQL query to load students
                    MySqlCommand cmd = new MySqlCommand("SELECT STUDENT_ID FROM STUDENTSTABLE", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int studentID = reader.GetInt32(0); // Get the student ID
                            cbStudents.Items.Add(studentID); // Add to ComboBox
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Make sure a student is selected from the ComboBox
            string studentID = cbStudents.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Please select a student.");
                return;
            }

            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // Prepare the stored procedure call
                    using (MySqlCommand cmd = new MySqlCommand("SP_CLASS_STUDENT_ADD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        cmd.Parameters.AddWithValue("p_CLASS_ID", classSectionID);
                        cmd.Parameters.AddWithValue("p_STUDENT_ID", studentID);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Student successfully added to the class!");

                // Reload the list of students after success
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void StudentClassList_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here if needed.
        }
    }
}
