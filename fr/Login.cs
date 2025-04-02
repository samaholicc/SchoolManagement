using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Threading;
using System.Windows.Forms;
using rskibbe.I18n.Models;
using rskibbe.I18n.Winforms;

namespace SchoolManagement
{
    public partial class Login : KryptonForm
    {
        public static string ID; // Dernière Identité de l'utilisateur  
        public static string TYPE_USER; // Type d'utilisateur (Admin, Teacher, Student)

        public Login()
        {
            InitializeComponent();
          
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Vérifier si les champs de texte sont vides  
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                error.Text = "Please enter your ID and password.";
                return;
            }

            // Connexion à la base de données  
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT ID, ROLE, PASSWORD FROM SYSTEM.ACCOUNT WHERE ID=@username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // Récupération des données  
                                string IDName = dr.GetString(0);
                                string userRole = dr.GetString(1);
                                string hashedPasswordFromDb = dr.GetString(2);
                                MessageBox.Show($"Debug: Hash en base: {hashedPasswordFromDb}\nHash entré: {Encrypt.HashString(txtPassword.Text)}");

                                // Comparaison des mots de passe hachés  
                                if (Encrypt.HashString(txtPassword.Text) == hashedPasswordFromDb)
                                {
                                    ID = IDName;
                                    TYPE_USER = userRole;
                                    OpenUserMenu(userRole);
                                }
                                else
                                {
                                    error.Text = "Invalid username or password.";
                                }
                            }
                            else
                            {
                                error.Text = "Invalid username or password.";
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenUserMenu(string userRole)
        {
            this.Hide();
            Form userMenu = null;
            if (userRole == "Admin")
            {
                userMenu = new AdminMenu();
            }
            else if (userRole == "Teacher")
            {
                userMenu = new TeacherMenu();
            }
            else if (userRole == "Student")
            {
                userMenu = new StudentMenu();
            }

            if (userMenu != null)
            {
                userMenu.ShowDialog();
            }


            this.Close(); // Fermer le formulaire de connexion après avoir ouvert le menu utilisateur  
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
       
        private void lbID_Click(object sender, EventArgs e)
        {

        }

        private void lbClassSectionID_Click(object sender, EventArgs e)
        {

        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {

        }
    }
}
