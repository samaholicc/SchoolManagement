using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class AdminProfile : KryptonForm
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

        public AdminProfile()
        {
            InitializeComponent();
            LoadTextBox();
        }

        private void LoadTextBox()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT * FROM ACCOUNT WHERE ID=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Login.ID); // Using parameterized query to prevent SQL injection

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) // Ensure that there is a row to read
                            {
                                txtName.Text = dr.GetString(1); // Assuming column 1 is Full Name
                                txtID.Text = dr.GetString(0);  // Assuming column 0 is ID
                                txtPassword.Text = dr.GetString(2); // Assuming column 2 is Password
                            }
                           
                        }
                    }
                }
            }
            catch (Exception es)
            {
                ShowMessage(es.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string passInsert = txtPassword.Text;
                string insertID = txtID.Text;

                string newPassword = txtPassword.Text; // Assuming there's a txtNewPassword TextBox
                string currentPassword = string.Empty;

                // Open MySQL connection to get current password from the database
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    currentPassword = GetCurrentPassword(insertID, conn);
                }

                // Validate new password before saving
                if (!string.IsNullOrEmpty(newPassword) && newPassword != currentPassword && IsPasswordValid(newPassword))
                {
                    // Hash the new password before saving it
                    string hashedPassword = Encrypt.HashString(newPassword);  // Hash the password

                    // Only update password if it was changed
                    InsertData insertData = new InsertData();
                    string updateResult = insertData.UpdatePassword(insertID, hashedPassword); // Pass the hashed password

                    if (updateResult != "Success")
                    {
                        ShowMessage("FailedToUpdatePassword");
                    }
                    else
                    {
                        ShowMessage("PasswordUpdated");
                    }
                }
                else
                {
                    ShowMessage("InvalidPassword");
                    return;  
                }

                // If no password change, don't attempt to update the database with the same password
                InsertData dataInserter = new InsertData();
                string result = dataInserter.UpdatePassword(insertID, Encrypt.HashString(passInsert));  // Always hash the password

                if (result != null)
                {
                    
                    this.Close();
                }
            }
            catch (Exception es)
            {
                ShowMessage(es.Message);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminProfile_Load(object sender, EventArgs e)
        {
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

        public string GetCurrentPassword(string insertID, MySqlConnection conn)
        {
            string password = string.Empty;
            string query = "SELECT PASSWORD FROM ACCOUNT WHERE ID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", insertID);
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
            string message = string.Empty;

            // Check the current UI culture (language) and display the appropriate message
            if (CultureInfo.CurrentUICulture.Name == "fr-FR")  // French
            {
                switch (key)
                {
                    case "PasswordTooShort":
                        message = "Le mot de passe doit contenir au moins 8 caractères !";
                        break;
                    case "PasswordNoSpecialChar":
                        message = "Le mot de passe doit contenir au moins un caractère spécial !";
                        break;
                    case "InvalidID":
                        message = "Veuillez entrer un ID valide.";
                        break;
                    case "FailedToUpdatePassword":
                        message = "Échec de la mise à jour du mot de passe. Vérifiez les détails.";
                        break;
                    case "PasswordUpdated":
                        message = "Mot de passe mis à jour avec succès.";
                        break;
                    case "InvalidPassword":
                        message = "Le mot de passe est invalide ou inchangé.";
                        break;
                    case "NoUserFound":
                        message = "Aucun utilisateur trouvé.";
                        break;
       
                    default:
                        message = key; 
                        break;
                }
            }
            else  
            {
                switch (key)
                {
                    case "PasswordTooShort":
                        message = "Password must be at least 8 characters!";
                        break;
                    case "PasswordNoSpecialChar":
                        message = "Password must contain at least one special character!";
                        break;
                    case "InvalidID":
                        message = "Please enter a valid ID.";
                        break;
                    case "FailedToUpdatePassword":
                        message = "Failed to update password. Please check the details.";
                        break;
                    case "PasswordUpdated":
                        message = "Password updated successfully.";
                        break;
                    case "InvalidPassword":
                        message = "Password is either unchanged or it does not meet the criteria.";
                        break;
                    case "NoUserFound":
                        message = "No user found.";
                        break;
                   
                    default:
                        message = key; // Default to key if no match
                        break;
                }
            }

            MessageBox.Show(message);
        }
    }
}
