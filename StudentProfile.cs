using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; 

namespace SchoolManagement
{
    public partial class StudentProfile : KryptonForm
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

        public StudentProfile()
        {
            InitializeComponent();
            LoadTextBox();
            
        }
        private void LoadTextBox()
        {
            try
            {
                MessageBox.Show("Attempting to load data...");
                MessageBox.Show("Login.ID: " + Login.ID);

                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT A.STUDENT_ID, A.FULL_NAME, A.CLASS_ID, A.DATE_OF_BIRTH, A.ADRESS, A.GENDER, B.Password " +
                         "FROM STUDENTSTABLE A " +
                         "JOIN ACCOUNT B ON A.FULL_NAME = B.USER " +
                         "WHERE B.USER = @ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                MessageBox.Show("Data retrieved successfully.");
                                txtID.Text = dr.GetInt32(0).ToString();
                                txtHoTen.Text = dr.GetString(1);

                                // Modification ici pour vérifier et assigner class ID  
                                txtClass.Text = dr.GetInt32(2).ToString(); // Assurez-vous que c'est un int  

                                txtBirth.Text = dr.GetDateTime(3).ToString("yyyy-MM-dd");
                                txtAddress.Text = dr.GetString(4);
                                txtGender.Text = dr.GetString(5);
                                txtPassword.Text = dr.GetString(6);
                            }
                            else
                            {
                                MessageBox.Show("No data found for the provided username.");
                            }
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("Error: " + es.Message);
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

        private void StudentProfile_Load(object sender, EventArgs e)
        {
        }

        private void txtGender_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
   
