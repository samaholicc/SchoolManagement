using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class StudentClassList : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private readonly string classID;
        private readonly int studentLimit; // New field to store the student limit

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public StudentClassList(string classID, int studentLimit)
        {
            InitializeComponent();
            this.classID = classID;
            this.studentLimit = studentLimit; // Store the student limit
            LoadStudents();
        }

        #region Load Data Methods
        private void LoadStudents()
        {
            try
            {
                if (string.IsNullOrEmpty(classID))
                {
                    MessageBox.Show(GetLocalizedMessage("NoClassSelected"));
                    this.Close();
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT s.STUDENT_ID, s.FULL_NAME 
                        FROM SYSTEM.studentstable s
                        WHERE s.STUDENT_ID NOT IN (
                            SELECT sc.STUDENT_ID 
                            FROM SYSTEM.student_classes sc 
                            WHERE sc.CLASS_ID = @ClassID
                        )
                        ORDER BY s.FULL_NAME";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            cbStudents.Items.Clear();
                            while (reader.Read())
                            {
                                Student student = new Student
                                {
                                    StudentID = reader.GetString(0),
                                    FullName = reader.GetString(1)
                                };
                                cbStudents.Items.Add(student);
                            }
                        }
                    }
                }

                if (cbStudents.Items.Count == 0)
                {
                    MessageBox.Show(GetLocalizedMessage("NoStudentsAvailable"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorLoading") + " " + ex.Message);
            }
        }
        #endregion

        #region Event Handlers
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbStudents.SelectedItem == null)
            {
                MessageBox.Show(GetLocalizedMessage("NoStudentSelected"));
                return;
            }

            Student selectedStudent = cbStudents.SelectedItem as Student;
            if (selectedStudent == null)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorSelectingStudent"));
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check the current number of students in the class
                    string countQuery = "SELECT COUNT(*) FROM SYSTEM.student_classes WHERE CLASS_ID = @ClassID";
                    using (MySqlCommand cmdCount = new MySqlCommand(countQuery, conn))
                    {
                        cmdCount.Parameters.AddWithValue("@ClassID", classID);
                        int currentStudentCount = Convert.ToInt32(cmdCount.ExecuteScalar());

                        // Check if adding the student would exceed the limit
                        if (currentStudentCount >= studentLimit)
                        {
                            MessageBox.Show(GetLocalizedMessage("ClassFull"));
                            return;
                        }
                    }

                    // Check if the student is already enrolled
                    string checkQuery = "SELECT COUNT(*) FROM SYSTEM.student_classes WHERE STUDENT_ID = @StudentID AND CLASS_ID = @ClassID";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@StudentID", selectedStudent.StudentID);
                        cmdCheck.Parameters.AddWithValue("@ClassID", classID);

                        int studentCount = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (studentCount > 0)
                        {
                            MessageBox.Show(GetLocalizedMessage("StudentAlreadyEnrolled"));
                            return;
                        }
                    }

                    // Execute stored procedure to add student to class
                    using (MySqlCommand cmd = new MySqlCommand("SP_CLASS_STUDENT_ADD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_CLASS_ID", classID);
                        cmd.Parameters.AddWithValue("p_STUDENT_ID", selectedStudent.StudentID);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show(GetLocalizedMessage("StudentAdded"));
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorAdding") + " " + ex.Message);
            }
        }

        private void StudentClassList_Load(object sender, EventArgs e)
        {
            // Display the class ID and student limit in the form (optional)
            this.Text = $"Add Students to Class {classID} (Limit: {studentLimit})";
        }
        #endregion

        #region Helper Methods
        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoClassSelected", "Aucune classe sélectionnée. Veuillez sélectionner une classe d'abord." },
                    { "NoStudentsAvailable", "Aucun étudiant disponible pour cette classe." },
                    { "NoStudentSelected", "Veuillez sélectionner un étudiant." },
                    { "ErrorSelectingStudent", "Erreur lors de la sélection de l'étudiant." },
                    { "StudentAlreadyEnrolled", "L'étudiant est déjà inscrit dans cette classe." },
                    { "StudentAdded", "Étudiant ajouté avec succès à la classe." },
                    { "ErrorLoading", "Erreur lors du chargement des étudiants : " },
                    { "ErrorAdding", "Erreur lors de l'ajout de l'étudiant : " },
                    { "ClassFull", "La classe est pleine. Limite d'étudiants atteinte." } // Added for class full message
                }
                : new Dictionary<string, string>
                {
                    { "NoClassSelected", "No class selected. Please select a class first." },
                    { "NoStudentsAvailable", "No students available for this class." },
                    { "NoStudentSelected", "Please select a student." },
                    { "ErrorSelectingStudent", "Error selecting student." },
                    { "StudentAlreadyEnrolled", "The student is already enrolled in this class." },
                    { "StudentAdded", "Student added successfully to the class." },
                    { "ErrorLoading", "Error loading students: " },
                    { "ErrorAdding", "Error adding student: " },
                    { "ClassFull", "The class is full. Student limit reached." } // Added for class full message
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        #region Student Class
        private class Student
        {
            public string StudentID { get; set; }
            public string FullName { get; set; }

            public override string ToString()
            {
                return $"{StudentID} - {FullName}";
            }
        }
        #endregion
    }
}