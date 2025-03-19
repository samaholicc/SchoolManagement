using System;
using System.Configuration;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace SchoolManagement
{
    public partial class TeacherProfile : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;

        // Customize form appearance
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public TeacherProfile()
        {
            InitializeComponent();
            LoadTeacherData();
        }

        private void LoadTeacherData()
        {
            try
            {
                if (string.IsNullOrEmpty(Login.ID))
                {
                    MessageBox.Show(GetLocalizedMessage("LoginIDNotSet"));
                    this.Close();
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.TEACHER_ID, 
                            t.FULL_NAME, 
                            t.ADRESS,   
                            t.GENDER, 
                            t.DATE_OF_BIRTH, 
                            t.DEP_ID,
                            a.ID 
                        FROM SYSTEM.teacher t 
                        JOIN SYSTEM.account a ON a.ID = t.TEACHER_ID 
                        WHERE t.TEACHER_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", Login.ID);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtClass.Text = dr.GetString(5); // DEP_ID
                            txtID.Text = dr.GetString(6);    // ACCOUNT.ID
                            txtHoTen.Text = dr.GetString(1); // FULL_NAME
                            txtAddress.Text = dr.GetString(2); // ADRESS
                            txtBirth.Text = dr.GetDateTime(4).ToString("yyyy-MM-dd"); // DATE_OF_BIRTH
                            txtGender.Text = dr.GetString(3); // GENDER
                            txtPassword.Text = ""; // Do not display password for security
                        }
                        else
                        {
                            MessageBox.Show(GetLocalizedMessage("NoDataFound"));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Log error instead of showing stack trace to users
                MessageBox.Show(GetLocalizedMessage("DatabaseError"));
                // Consider logging: Logger.LogError(ex);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;

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
                MessageBox.Show(GetLocalizedMessage("UpdateError"));
                // Consider logging: Logger.LogError(ex);
            }
        }

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

            var messages_fr = new System.Collections.Generic.Dictionary<string, string>
            {
                { "LoginIDNotSet", "Login ID n'est pas défini. Veuillez vous reconnecter." },
                { "NoDataFound", "Aucune donnée trouvée pour cet identifiant d'enseignant." },
                { "AccountModified", "Compte modifié avec succès." },
                { "DatabaseError", "Erreur de connexion à la base de données." },
                { "NoPasswordChange", "Aucun changement de mot de passe détecté." },
                { "UpdateFailed", "Échec de la mise à jour du compte." },
                { "UpdateError", "Erreur lors de la mise à jour du compte." },
                { "PasswordTooShort", "Le mot de passe doit contenir au moins 8 caractères." },
                { "PasswordNoSpecialChar", "Le mot de passe doit contenir au moins un caractère spécial." }
            };

            var messages_en = new System.Collections.Generic.Dictionary<string, string>
            {
                { "LoginIDNotSet", "Login ID is not set. Please log in again." },
                { "NoDataFound", "No data found for the given Teacher ID." },
                { "AccountModified", "Account modified successfully." },
                { "DatabaseError", "Database connection error." },
                { "NoPasswordChange", "No password change detected." },
                { "UpdateFailed", "Failed to update account." },
                { "UpdateError", "Error updating account." },
                { "PasswordTooShort", "Password must be at least 8 characters long." },
                { "PasswordNoSpecialChar", "Password must contain at least one special character." }
            };

            var messages = currentCulture.StartsWith("fr") ? messages_fr : messages_en;
            string message;
            return messages.TryGetValue(messageKey, out message) ? message : "Message not found.";
        }

        private void TeacherProfile_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }
    }

    public class DataManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public string UpdatePassword(string userId, string hashedPassword)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE SYSTEM.account SET PASSWORD = @password WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0 ? "Success" : null;
            }
        }
    }
}