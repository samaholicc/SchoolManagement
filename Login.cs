using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

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
            // Vérifiez si les champs de texte sont vides  
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
                    string query = "SELECT USER, ROLE, PASSWORD FROM SYSTEM.ACCOUNT WHERE USER=@username"; // Utilisation d'une requête sécurisée

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text); // Ajoute le nom d'utilisateur en tant que paramètre

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())  // Vérifie s'il y a des résultats  
                            {
                                // Récupération des données  
                                string username = dr.GetString(0); // Nom d'utilisateur  
                                string userRole = dr.GetString(1); // Rôle de l'utilisateur  
                                string hashedPasswordFromDb = dr.GetString(2); // Mot de passe haché stocké

                                // Hachage du mot de passe fourni par l'utilisateur  
                                string hashedPasswordInput = Encrypt.HashString(txtPassword.Text);

                                // Comparaison des mots de passe hachés  
                                if (hashedPasswordInput == hashedPasswordFromDb)
                                {
                                    // Identification de l'utilisateur avec le bon rôle  
                                    ID = username; // Assigner le nom d'utilisateur  
                                    TYPE_USER = userRole; // Assigner le rôle de l'utilisateur

                                    // Ouvrir le menu correspondant  
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
                error.Text = "Database error: " + ex.Message; // Affiche l'erreur de la base de données  
            }
            catch (Exception ex)
            {
                error.Text = "An error occurred: " + ex.Message; // Affiche une erreur générique  
            }
        }

        private void OpenUserMenu(string userRole)
        {
            this.Hide();

            switch (userRole)
            {
                case "Admin":
                    AdminMenu adminMenu = new AdminMenu();
                    adminMenu.ShowDialog();
                    break;
                case "Teacher":
                    TeacherMenu teacherMenu = new TeacherMenu();
                    teacherMenu.ShowDialog();
                    break;
                case "Student":
                    StudentMenu studentMenu = new StudentMenu();
                    studentMenu.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Unknown user role.");
                    break;
            }

            this.Close(); // Fermer le formulaire de connexion après avoir ouvert le menu utilisateur  
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Logique d'initialisation (si nécessaire)
        }
    }
}