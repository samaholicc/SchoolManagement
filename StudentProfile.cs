using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class StudentProfile : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public StudentProfile()
        {
            InitializeComponent();
            LoadTextBox();
        }

        #region Load Data
        private void LoadTextBox()
        {
            try
            {
                if (string.IsNullOrEmpty(Login.ID))
                {
                    MessageBox.Show(GetLocalizedMessage("LoginIDNotSet"));
                    this.Close();
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT A.STUDENT_ID, A.FULL_NAME, 
                               GROUP_CONCAT(C.CLASS_ID ORDER BY C.CLASS_ID ASC SEPARATOR ', ') AS CLASS_ID, 
                               A.DATE_OF_BIRTH, A.ADRESS, A.GENDER, B.Password
                        FROM STUDENTSTABLE A
                        JOIN ACCOUNT B ON A.STUDENT_ID = B.ID
                        JOIN STUDENT_CLASSES C ON A.STUDENT_ID = C.STUDENT_ID
                        WHERE B.ID = @ID
                        GROUP BY A.STUDENT_ID, A.FULL_NAME, A.DATE_OF_BIRTH, A.ADRESS, A.GENDER, B.Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                txtID.Text = dr.GetString(0); // STUDENT_ID
                                txtUser.Text = dr.GetString(1); // FULL_NAME
                                txtClass.Text = dr.GetString(2); // CLASS_ID (comma-separated)
                                txtBirth.Text = dr.GetDateTime(3).ToString("yyyy-MM-dd"); // DATE_OF_BIRTH
                                txtAddress.Text = dr.GetString(4); // ADRESS
                                txtGender.Text = dr.GetString(5); // GENDER
                                txtPassword.Text = ""; // Do not display password for security
                            }
                            else
                            {
                                MessageBox.Show(GetLocalizedMessage("NoDataFound"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
        }
        #endregion

        #region Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                string userID = txtID.Text;
                string newPassword = txtPassword.Text;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    string hashedPassword = Encrypt.HashString(newPassword);
                    DataManager dataManager = new DataManager();
                    string result = dataManager.UpdatePassword(userID, hashedPassword);

                    if (result == "Success")
                    {
                        MessageBox.Show(GetLocalizedMessage("AccountModified"));
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(GetLocalizedMessage("UpdateFailed"));
                    }
                }
                else
                {
                    MessageBox.Show(GetLocalizedMessage("NoPasswordChange"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Helper Methods
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                // Allow empty password to mean no change
                return true;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show(GetLocalizedMessage("PasswordTooShort"));
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                MessageBox.Show(GetLocalizedMessage("PasswordNoSpecialChar"));
                return false;
            }

            return true;
        }

        

        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "AttemptingLoad", "Tentative de charger les données..." },
                    { "DataRetrieved", "Données récupérées avec succès." },
                    { "LoginIDNotSet", "L'ID de connexion n'est pas défini. Veuillez vous reconnecter." },
                    { "NoDataFound", "Aucune donnée trouvée pour l'identifiant fourni." },
                    { "AccountModified", "Compte modifié avec succès." },
                    { "UpdateFailed", "Échec de la mise à jour du compte." },
                    { "NoPasswordChange", "Aucun changement de mot de passe détecté." },
                    { "PasswordTooShort", "Le mot de passe doit contenir au moins 8 caractères." },
                    { "PasswordNoSpecialChar", "Le mot de passe doit contenir au moins un caractère spécial." },
                    { "Error", "Erreur : " }
                }
                : new Dictionary<string, string>
                {
                    { "AttemptingLoad", "Attempting to load data..." },
                    { "DataRetrieved", "Data retrieved successfully." },
                    { "LoginIDNotSet", "Login ID is not set. Please log in again." },
                    { "NoDataFound", "No data found for the provided ID." },
                    { "AccountModified", "Account modified successfully." },
                    { "UpdateFailed", "Failed to update account." },
                    { "NoPasswordChange", "No password change detected." },
                    { "PasswordTooShort", "Password must be at least 8 characters long." },
                    { "PasswordNoSpecialChar", "Password must contain at least one special character." },
                    { "Error", "Error: " }
                };

            string message;
            return messages.TryGetValue(messageKey, out message) ? message : "Unknown error";
        }
        #endregion

        #region DataManager Class
        private class DataManager
        {
            private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            public string UpdatePassword(string userId, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE ACCOUNT SET PASSWORD = @Password WHERE ID = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", userId);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Success" : null;
                    }
                }
            }
        }
        #endregion

        private void StudentProfile_Load(object sender, EventArgs e)
        {

        }
    }
}