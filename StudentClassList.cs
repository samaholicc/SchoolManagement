using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

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
        private string ClassID;

        public StudentClassList(string ClassID)
        {
            InitializeComponent();
            this.ClassID = ClassID;
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
                            string studentID = reader.GetString(0); // Get the student ID
                            cbStudents.Items.Add(studentID); // Add to ComboBox
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message));
            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Vérifier qu'un étudiant a été sélectionné
            string studentID = cbStudents.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show(GetLocalizedMessage("Please select a student.", "Veuillez sélectionner un étudiant."));
                return;
            }

            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                string studentName = string.Empty; // Initialiser la variable pour le nom

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // Étape 1 : Récupérer le nom de l'étudiant
                    using (MySqlCommand nameCmd = new MySqlCommand("SELECT full_name FROM STUDENTSTABLE WHERE STUDENT_ID = @StudentID", conn))
                    {
                        nameCmd.Parameters.AddWithValue("@StudentID", studentID);

                        // Exécuter la requête et lire le résultat
                        using (MySqlDataReader reader = nameCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentName = reader.GetString(0); // Récupérer le nom
                            }
                            else
                            {
                                MessageBox.Show(GetLocalizedMessage("Student not found!", "Étudiant introuvable !"));
                                return;
                            }
                        }
                    }

                    // Étape 2 : Appeler la procédure stockée avec les données récupérées
                    using (MySqlCommand cmd = new MySqlCommand("SP_CLASS_STUDENT_ADD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_CLASS_ID", ClassID);
                        cmd.Parameters.AddWithValue("p_STUDENT_ID", studentID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(GetLocalizedMessage("Student added to the class successfully!", "Étudiant ajouté avec succès à la classe !"));
                LoadStudents(); // Recharger la liste des étudiants
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error: " + ex.Message, "Erreur : " + ex.Message));
            }
        }


        private string GetLocalizedMessage(string englishMessage, string frenchMessage)
        {
            if (CultureInfo.CurrentCulture.Name == "fr-FR")
            {
                return frenchMessage;  // Return the French message if culture is French
            }
            else
            {
                return englishMessage;  // Return the English message by default
            }
        }


        private void StudentClassList_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here if needed.
        }
    }
}
