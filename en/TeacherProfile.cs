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
            t.DEP_ID,
            a.ID, 
            a.PASSWORD 
        FROM SYSTEM.TEACHER t 
        JOIN SYSTEM.ACCOUNT a ON a.ID = t.TEACHER_ID  -- Jointure basée sur TEACHER_ID, 
        WHERE t.TEACHER_ID = @id"; // Recherche par TEACHER_ID

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", Login.ID); // Passer le TEACHER_ID

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtClass.Text = dr.GetString(5);
                            txtID.Text = dr.GetString(6);
                            txtHoTen.Text = dr.GetString(1);
                            txtAddress.Text = dr.GetString(2);
                            txtBirth.Text = dr.GetDateTime(4).ToString("yyyy-MM-dd");
                            txtGender.Text = dr.GetString(3);
                            txtPassword.Text = dr.GetString(7);
                        }
                        else
                        {
                            MessageBox.Show("No data found for the given Teacher ID.");
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
            this.Close(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                InsertData dataInserter = new InsertData();

                string passInsert = txtPassword.Text;

                string userID = txtID.Text;
                string hashedPassword = Encrypt.HashString(passInsert);

                
                string result = dataInserter.UpdatePassword(userID, hashedPassword);

                if (result != null)
                {
                    MessageBox.Show("Account modified successfully.");
                    this.Close();
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message); // Afficher tout message d'erreur  
            }
        }

        private void TeacherProfile_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}