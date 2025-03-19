using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Login : KryptonForm
    {
        public static string ID { get; private set; } // User's last identity
        public static string TYPE_USER { get; private set; } // User type (Admin, Teacher, Student)
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public Login()
        {
            InitializeComponent();
            ConfigureForm();
        }

        #region Initialization
        private void ConfigureForm()
        {
            txtPassword.UseSystemPasswordChar = true; // Mask password input
            this.FormClosed += (s, e) => Application.Exit(); // Ensure app exits when form closes
        }
        #endregion

        #region Event Handlers
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                AuthenticateUser();
            }
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                var currentLanguage = GetCurrentLanguage();
                var newLanguage = currentLanguage == "fr-FR" ? "en-US" : "fr-FR";
                var changeLanguage = new ChangeLanguage();
                changeLanguage.UpdateConfig("language", newLanguage);
                MessageBox.Show(GetLocalizedMessage("Language switched. Restarting application...", "Langue changée. Redémarrage de l'application..."));
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error switching language: " + ex.Message, "Erreur lors du changement de langue : " + ex.Message));
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }
        #endregion

        #region Authentication Methods
        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                error.Text = GetLocalizedMessage("Please enter your username and password.", "Veuillez entrer votre identifiant et votre mot de passe.");
                return false;
            }
            return true;
        }

        private void AuthenticateUser()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, ROLE, PASSWORD FROM SYSTEM.ACCOUNT WHERE ID = @username";

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
                                error.Text = GetLocalizedMessage("Invalid username or password.", "Identifiant ou mot de passe invalide.");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(GetLocalizedMessage("Database error: " + ex.Message, "Erreur de base de données : " + ex.Message),
                                GetLocalizedMessage("Error", "Erreur"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message),
                                GetLocalizedMessage("Error", "Erreur"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateUser(MySqlDataReader dr)
        {
            string id = dr.GetString(0);
            string userRole = dr.GetString(1);
            string hashedPasswordFromDb = dr.GetString(2); // Stored hash from the database

            // Hash the entered password using Encrypt.HashString
            string enteredPassword = txtPassword.Text;
            string hashedEnteredPassword = Encrypt.HashString(enteredPassword);

            // Compare the hashed entered password with the stored hash
            if (hashedEnteredPassword == hashedPasswordFromDb)
            {
                ID = id;
                TYPE_USER = userRole;
                OpenUserMenu(userRole);
            }
            else
            {
                error.Text = GetLocalizedMessage("Invalid username or password.", "Identifiant ou mot de passe invalide.");
            }
        }

        private void OpenUserMenu(string userRole)
        {
            this.Hide();

            Form userMenu = null;
            switch (userRole.ToLower())
            {
                case "admin":
                    userMenu = new MenuAdmin();
                    break;
                case "teacher":
                    userMenu = new TeacherMenu();
                    break;
                case "student":
                    userMenu = new StudentMenu();
                    break;
                default:
                    MessageBox.Show(GetLocalizedMessage("Unknown user role.", "Rôle d'utilisateur inconnu."));
                    break;
            }

            if (userMenu != null)
            {
                userMenu.ShowDialog();
            }

            this.Close();
        }
        #endregion

        #region Helper Methods
        private string GetCurrentLanguage()
        {
            return ConfigurationManager.AppSettings["language"] ?? "en-US"; // Default to English if not set
        }

        private string GetLocalizedMessage(string englishMessage, string frenchMessage)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            return currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase) ? frenchMessage : englishMessage;
        }
        #endregion
    }

    
}