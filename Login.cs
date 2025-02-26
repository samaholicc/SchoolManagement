using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Login : KryptonForm
    {
        public static string ID { get; private set; } // Dernière Identité de l'utilisateur  
        public static string TYPE_USER { get; private set; } // Type d'utilisateur (Admin, Teacher, Student)

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                AuthenticateUser();
            }
        }

        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                error.Text = "Veuillez entrer votre identifiant et votre mot de passe.";
                return false;
            }
            return true;
        }

        private void AuthenticateUser()
        {
            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
            try
            {
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
                                ValidateUser(dr);
                            }
                            else
                            {
                                error.Text = "Identifiant ou mot de passe invalide.";
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de base de données : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateUser(MySqlDataReader dr)
        {
            string IDName = dr.GetString(0);
            string userRole = dr.GetString(1);
            string hashedPasswordFromDb = dr.GetString(2);

            if (Encrypt.HashString(txtPassword.Text) == hashedPasswordFromDb)
            {
                ID = IDName;
                TYPE_USER = userRole;
                OpenUserMenu(userRole);
            }
            else
            {
                error.Text = "Identifiant ou mot de passe invalide.";
            }
        }

        private void OpenUserMenu(string userRole)
        {
            this.Hide();

            Form userMenu = null;

            switch (userRole)
            {
                case "Admin":
                    userMenu = new MenuAdmin();
                    break;
                case "Teacher":
                    userMenu = new TeacherMenu();
                    break;
                case "Student":
                    userMenu = new StudentMenu();
                    break;
                default:
                    break;
            }

            if (userMenu != null)
            {
                userMenu.ShowDialog();
            }

            this.Close();
        }


        private string GetCurrentLanguage()
        {
            return ConfigurationManager.AppSettings["language"];
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            var currentLanguage = GetCurrentLanguage();
            var newLanguage = currentLanguage == "en-US" ? "fr-FR" : "en-US";
            var changeLanguage = new ChangeLanguage();
            changeLanguage.UpdateConfig("language", newLanguage);
            Application.Restart();
        }

        private void kryptonWrapLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
