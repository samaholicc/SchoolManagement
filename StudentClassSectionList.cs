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
    public partial class StudentClassSectionList : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private readonly string classSectionID;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public StudentClassSectionList(string classSectionID = null)
        {
            InitializeComponent();
            this.classSectionID = classSectionID ?? TeacherClassSection.ClassID; // Fallback to TeacherClassSection.ClassID if null
            LoadStudents();
        }

        #region Load Data Methods
        private void LoadStudents()
        {
            try
            {
                if (string.IsNullOrEmpty(classSectionID))
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
                        FROM SYSTEM.STUDENTSTABLE s
                        WHERE s.STUDENT_ID NOT IN (
                            SELECT sc.STUDENT_ID
                            FROM SYSTEM.STUDENT_CLASSES sc
                            WHERE sc.CLASS_ID = @ClassID
                        )
                        ORDER BY s.FULL_NAME";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classSectionID);

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

                    // Check if the student is already enrolled
                    string checkQuery = "SELECT COUNT(*) FROM SYSTEM.STUDENT_CLASSES WHERE STUDENT_ID = @StudentID AND CLASS_ID = @ClassID";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@StudentID", selectedStudent.StudentID);
                        cmdCheck.Parameters.AddWithValue("@ClassID", classSectionID);

                        int studentCount = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (studentCount > 0)
                        {
                            MessageBox.Show(GetLocalizedMessage("StudentAlreadyEnrolled"));
                            return;
                        }
                    }

                    // Insert the new enrollment
                    string insertQuery = "INSERT INTO SYSTEM.STUDENT_CLASSES (STUDENT_ID, CLASS_ID) VALUES (@StudentID, @ClassID)";
                    using (MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@StudentID", selectedStudent.StudentID);
                        cmdInsert.Parameters.AddWithValue("@ClassID", classSectionID);

                        cmdInsert.ExecuteNonQuery();
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

        private void StudentClassSectionList_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
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
                    { "ErrorAdding", "Erreur lors de l'ajout de l'étudiant : " }
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
                    { "ErrorAdding", "Error adding student: " }
                };

            string message; // Explicit declaration for C# 6.0 compatibility
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