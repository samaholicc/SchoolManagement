using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access
using System.Security.Cryptography;
using static SchoolManagement.Login;

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
                    string query = "SELECT * FROM ACCOUNT WHERE user=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Login.ID); // Using parameterized query to prevent SQL injection

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) // Ensure that there is a row to read
                            {
                                txtHoTen.Text = dr.GetString(1); // Assuming column 1 is Full Name
                                txtID.Text = dr.GetInt32(0).ToString();  // Assuming column 0 is ID_LG
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
                InsertData dataInserter = new InsertData();

                string userID = txtHoTen.Text;
                string passInsert = txtPassword.Text;

                int insertID;
                if (!int.TryParse(txtID.Text, out insertID))
                {
                    MessageBox.Show("Please enter a valid ID.");
                    return;
                }




                string result = dataInserter.UpdatePassword(userID, passInsert);

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
    }
}