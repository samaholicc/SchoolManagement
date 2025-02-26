using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access
using System.Security.Cryptography;

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
            MessageBox.Show("Attempting to load user with ID: " + Login.ID);

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
                            else
                            {
                                MessageBox.Show("No user found.");
                            }
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string passInsert = txtPassword.Text;
                string insertID = txtID.Text;

                if (string.IsNullOrWhiteSpace(insertID))
                {
                    MessageBox.Show("Please enter a valid ID.");
                    return;
                }

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
                        MessageBox.Show("Failed to update password. Please check the details.");
                    }
                    else
                    {
                        MessageBox.Show("Password updated successfully.");
                    }
                }
                else
                {
                    MessageBox.Show("Password is either invalid, unchanged, or it does not meet the criteria.");
                    return;  // Don't proceed if the password is invalid or unchanged
                }

                // If no password change, don't attempt to update the database with the same password
                InsertData dataInserter = new InsertData();
                string result = dataInserter.UpdatePassword(insertID, Encrypt.HashString(passInsert));  // Always hash the password

                if (result != null)
                {
                    MessageBox.Show("Account modified successfully.");
                    this.Close();
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
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
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères !");
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                MessageBox.Show("Le mot de passe doit contenir au moins un caractère spécial !");
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
    }
}
