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
                        string query = "SELECT A.STUDENT_ID, A.FULL_NAME, " +
                   "GROUP_CONCAT(C.CLASS_ID ORDER BY C.CLASS_ID ASC SEPARATOR ', ') AS CLASS_ID, " +
                   "A.DATE_OF_BIRTH, A.ADRESS, A.GENDER, B.Password " +
                   "FROM STUDENTSTABLE A " +
                   "JOIN ACCOUNT B ON A.FULL_NAME = B.FULL_NAME " +
                   "JOIN STUDENT_CLASSES C ON A.STUDENT_ID = C.STUDENT_ID " +
                   "WHERE B.ID = @ID " +
                   "GROUP BY A.STUDENT_ID, A.FULL_NAME, A.DATE_OF_BIRTH, A.ADRESS, A.GENDER, B.Password";


                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("_Lingobit_@ID", Login.ID);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                MessageBox.Show("Data retrieved successfully.");
                                txtID.Text = dr.GetString(0);
                                txtUser.Text = dr.GetString(1);

                                txtClass.Text = dr.GetString(2); 

                                txtBirth.Text = dr.GetDateTime(3).ToString("yyyy-MM-dd");
                                txtAddress.Text = dr.GetString(4);
                                txtGender.Text = dr.GetString(5);
                                txtPassword.Text = dr.GetString(6);
                            }
                            else
                            {
                                MessageBox.Show("_Lingobit_No data found for the provided username.");
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

                string userID = txtID.Text;
                string passInsert = txtPassword.Text;

                // Hash the password before passing it to the UpdatePassword method
                string hashedPassword = Encrypt.HashString(passInsert);

                // Call the UpdatePassword method with the hashed password
                string result = dataInserter.UpdatePassword(userID, hashedPassword);

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
   
