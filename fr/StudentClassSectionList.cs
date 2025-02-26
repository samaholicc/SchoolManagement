using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class StudentClassSectionList : KryptonForm
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

        private string connectionString = "Server=localhost;Database=system;User ID=root;Password=samia;";

        public StudentClassSectionList()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                // Get the selected ClassID from the ClassSectionManager (you might already have this as a global property)
                string selectedClassID = ClassSectionManager.ClassSectionID;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Modify the query to exclude students already in the selected class (using student_classes)
                    string query = @"
                    SELECT s.Student_ID, s.Full_Name 
                    FROM studentstable s
                    WHERE s.Student_ID NOT IN (
                        SELECT sc.Student_ID
                        FROM student_classes sc
                        WHERE sc.Class_ID = @ClassID
                    )";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", selectedClassID); // Add the ClassID as a parameter
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student student = new Student
                                {
                                    StudentID = reader.GetString(0),
                                    FullName = reader.GetString(1)
                                };
                                cbStudents.Items.Add(student); // Add the student object to ComboBox
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des étudiants : " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbStudents.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.");
                return;
            }

            // Récupère l'objet Student sélectionné
            Student selectedStudent = cbStudents.SelectedItem as Student;
            if (selectedStudent == null)
            {
                MessageBox.Show("Erreur lors de la sélection de l'étudiant.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the student is already enrolled in the class (using student_classes table)
                    using (MySqlCommand cmdCheck = new MySqlCommand("SELECT COUNT(*) FROM student_classes WHERE Student_ID = @StudentID AND Class_ID = @ClassID", conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@StudentID", selectedStudent.StudentID);
                        cmdCheck.Parameters.AddWithValue("@ClassID", ClassSectionManager.ClassSectionID);

                        int studentCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (studentCount > 0)
                        {
                            // Student is already enrolled in the class
                            MessageBox.Show("L'étudiant est déjà inscrit dans cette classe.");
                        }
                        else
                        {
                            // Student is not in the class, insert the new record into student_classes table
                            using (MySqlCommand cmdInsert = new MySqlCommand("INSERT INTO student_classes (Student_ID, Class_ID) VALUES (@StudentID, @ClassID)", conn))
                            {
                                cmdInsert.Parameters.AddWithValue("@StudentID", selectedStudent.StudentID);
                                cmdInsert.Parameters.AddWithValue("@ClassID", ClassSectionManager.ClassSectionID);

                                cmdInsert.ExecuteNonQuery();
                            }

                            MessageBox.Show("Étudiant ajouté avec succès à la classe.");
                        }
                    }
                }
                this.Close(); // Close the form after successful operation
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'étudiant : " + ex.Message);
            }
        }

        private void StudentClassSectionList_Load(object sender, EventArgs e)
        {

        }
    }
}
