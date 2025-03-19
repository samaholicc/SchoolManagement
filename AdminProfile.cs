using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class AdminProfile : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private static readonly string MySqlDb = GetConnectionString();

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public AdminProfile()
        {
            InitializeComponent();
            LoadTextBox();
        }

        private static string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ConfigurationErrorsException("Connection string 'MySqlConnection' not found in app.config.");
            }
            return connectionString;
        }

        private void LoadTextBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySqlDb))
                {
                    conn.Open();
                    string query = "SELECT * FROM ACCOUNT WHERE ID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Login.ID);
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                txtName.Text = dr.GetString(1); // Assuming column 1 is Full Name
                                txtID.Text = dr.GetString(0);   // Assuming column 0 is ID
                                // Do not display password for security; leave blank or use placeholder
                                txtPassword.Text = "•••••"; // Placeholder instead of actual password
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = txtPassword.Text;
                string userId = txtID.Text;

                using (MySqlConnection conn = new MySqlConnection(MySqlDb))
                {
                    conn.Open();
                    string currentPassword = GetCurrentPassword(userId, conn);

                    if (!string.IsNullOrEmpty(newPassword) && newPassword != currentPassword && IsPasswordValid(newPassword))
                    {
                        string hashedPassword = Encrypt.HashString(newPassword);
                        InsertData insertData = new InsertData();
                        string updateResult = insertData.UpdatePassword(userId, hashedPassword);

                        if (updateResult == "Success")
                        {
                            ShowMessage("PasswordUpdated");
                            this.Close();
                        }
                        else
                        {
                            ShowMessage("FailedToUpdatePassword");
                        }
                    }
                    else
                    {
                        ShowMessage("InvalidPassword");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                ShowMessage("PasswordTooShort");
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                ShowMessage("PasswordNoSpecialChar");
                return false;
            }

            return true;
        }

        public string GetCurrentPassword(string userId, MySqlConnection conn)
        {
            string password = string.Empty;
            string query = "SELECT PASSWORD FROM ACCOUNT WHERE ID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        password = reader["PASSWORD"].ToString();
                    }
                }
            }
            return password;
        }

        private void ShowMessage(string key)
        {
            string message = key;

            if (CultureInfo.CurrentUICulture.Name == "fr-FR") // French
            {
                switch (key)
                {
                    case "PasswordTooShort": message = "Le mot de passe doit contenir au moins 8 caractères !"; break;
                    case "PasswordNoSpecialChar": message = "Le mot de passe doit contenir au moins un caractère spécial !"; break;
                    case "InvalidID": message = "Veuillez entrer un ID valide."; break;
                    case "FailedToUpdatePassword": message = "Échec de la mise à jour du mot de passe. Vérifiez les détails."; break;
                    case "PasswordUpdated": message = "Mot de passe mis à jour avec succès."; break;
                    case "InvalidPassword": message = "Le mot de passe est invalide ou inchangé."; break;
                    case "NoUserFound": message = "Aucun utilisateur trouvé."; break;
                }
            }
            else // Default to English
            {
                switch (key)
                {
                    case "PasswordTooShort": message = "Password must be at least 8 characters!"; break;
                    case "PasswordNoSpecialChar": message = "Password must contain at least one special character!"; break;
                    case "InvalidID": message = "Please enter a valid ID."; break;
                    case "FailedToUpdatePassword": message = "Failed to update password. Please check the details."; break;
                    case "PasswordUpdated": message = "Password updated successfully."; break;
                    case "InvalidPassword": message = "Password is either unchanged or it does not meet the criteria."; break;
                    case "NoUserFound": message = "No user found."; break;
                }
            }

            MessageBox.Show(message);
        }

        private void AdminProfile_Load(object sender, EventArgs e)
        {

        }
    }
}