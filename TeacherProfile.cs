using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Necessary for MySQL

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
            LoadTextBox();
        }

        private void LoadTextBox()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                // Assurez-vous que Login.ID est correctement défini  
                if (string.IsNullOrEmpty(Login.ID))
                {
                    MessageBox.Show("Login ID is not set. Please log in again.");
                    this.Close();
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = @"
            SELECT 
                t.TEACHER_ID, 
                t.FULL_NAME, 
                t.ADRESS,   
                t.GENDER, 
                t.DATE_OF_BIRTH, 
                a.USER, 
                a.PASSWORD 
            FROM SYSTEM.TEACHER t 
            JOIN SYSTEM.ACCOUNT a ON a.USER = t.FULL_NAME  
            WHERE t.FULL_NAME = @name"; // Recherche par FULL_NAME

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", Login.ID); // Passer le FULL_NAME

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtID.Text = dr.GetInt32(0).ToString();
                            txtHoTen.Text = dr.GetString(1);
                            txtAddress.Text = dr.GetString(2);
                            txtBirth.Text = dr.GetDateTime(4).ToString("yyyy-MM-dd");
                            txtGender.Text = dr.GetString(3);
                            txtPassword.Text = dr.GetString(6);
                        }
                        else
                        {
                            MessageBox.Show("No data found for the given Teacher name.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"MySQL error ({ex.Number}): {ex.Message}\n{ex.StackTrace}");
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Close(); // Fermer le formulaire  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtHoTen.Text;
                string passInsert = txtPassword.Text;

                // Assurez-vous que InsertData est bien configuré pour mettre à jour le mot de passe  
                InsertData dataInserter = new InsertData();
                string result = dataInserter.UpdatePassword(username, passInsert); // Ajustez l'appel selon vos besoins

                if (result != null)
                {
                    MessageBox.Show("Account modified successfully.");
                    this.Close(); // Fermez le formulaire après que la modification a été effectuée  
                }
                else
                {
                    MessageBox.Show("Failed to modify account.");
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message); // Afficher tout message d'erreur  
            }
        }
    }
}