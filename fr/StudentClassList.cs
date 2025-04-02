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
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Vérifier qu'un étudiant a été sélectionné
            string studentID = cbStudents.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.");
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
                                MessageBox.Show("Étudiant introuvable !");
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

                MessageBox.Show("Étudiant ajouté avec succès à la classe !");
                LoadStudents(); // Recharger la liste des étudiants
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        

        private void StudentClassList_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here if needed.
        }
    }
}
