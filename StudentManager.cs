using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access

namespace SchoolManagement
{
    public partial class StudentManager : KryptonForm
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

        private int action; // 0 - add, 1 - edit  
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public StudentManager()
        {
            InitializeComponent();
            LoadStudents();
            LoadComboBoxClass();

        }

        private void LoadStudents()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    // Modifiez la requête pour combiner CLASS_ID et CLASS_NAME  
                    string query = @"
            SELECT 
                a.STUDENT_ID, 
                CONCAT(a.CLASS_ID, ' - ', c.CLASS_NAME) AS `Class`, 
                a.FULL_NAME AS `Name`, 
                a.DATE_OF_BIRTH AS `Birth`, 
                a.GENDER AS `Gender`, 
                a.ADRESS AS `Address`
            FROM SYSTEM.STUDENTSTABLE a 
            JOIN SYSTEM.CLASS c ON a.CLASS_ID = c.CLASS_ID;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Mettre à jour le DataGridView avec le DataTable 
                            dgvTeachers.DataSource = dataTable;

                            // Supprimez explicitement CLASS_ID si vous le souhaitez  
                            // dgvStudents.Columns.Remove("CLASS_ID"); // Optionnel si CLASS_ID n'est plus dans la requête

                            // Si vous avez besoin de renommer d'autres colonnes, vous pouvez le faire ici  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT STUDENT_ID, CLASS_ID AS `Class`, FULL_NAME AS `Name`, DATE_OF_BIRTH AS `BIRTH`, " +
                                   "GENDER AS `GENDER`, ADRESS AS `Address` " +
                                   "FROM SYSTEM.STUDENTSTABLE WHERE FULL_NAME LIKE @search OR STUDENT_ID LIKE @search " +
                                   "OR CLASS_ID LIKE @search OR GENDER LIKE @search OR ADRESS LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvTeachers.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                showAction();

                DataGridViewRow row = dgvTeachers.Rows[e.RowIndex];

                // Assuming the structure is as follows:
                // 0: STUDENT_ID, 1: CLASS_ID, 2: FULL_NAME, 3: DATE_OF_BIRTH, 4: GENDER, 5: ADRESS  
                txtID.Text = row.Cells[0].Value.ToString(); // STUDENT_ID  
                txtName.Text = row.Cells[2].Value.ToString(); // FULL_NAME

                // Correctly set the date of birth  
                if (DateTime.TryParse(row.Cells[3].Value.ToString(), out DateTime dateOfBirth))
                {
                    dtpBirth.Value = dateOfBirth;
                }

                // Set the address field correctly  
                txtAddress.Text = row.Cells[5].Value?.ToString() ?? string.Empty; // ADRESS

                // Set the ComboBox to the selected CLASS_ID  
                string classId = row.Cells[1].Value.ToString(); // CLASS_ID  
                if (cbClass.Items.Contains(classId))
                {
                    cbClass.SelectedItem = classId; // Set the selected class in ComboBox  
                }
                else
                {
                    // Optionally, you can handle the case where the CLASS_ID is not found in the ComboBox  
                    cbClass.Text = classId; // Set the text if the item is not found  
                }

                // Set gender based on cell value  
                if (row.Cells[4].Value.ToString() == "Homme")
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0;

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Visible = false;
            label10.Visible = false;

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;

            txtPassword.Text = "";
            txtPassword.Enabled = true;

            cbClass.Text = "";
            cbClass.Enabled = true;

            dtpBirth.Enabled = true;

            rbMale.Checked = false;
            rbMale.Enabled = true;
            rbFemale.Checked = false;
            rbFemale.Enabled = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a student to edit!");
                return;
            }
            action = 1;

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Enabled = false;
            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbClass.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void showAction()
        {
            pbStudents.Visible = true;
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (action == 0) // Add new student  
                    {
                        // Check if a class is selected  
                        // Check if a class is selected  
                        if (cbClass.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a class.");
                            return; // Exit the method if no class is selected  
                        }
                        int classId = Convert.ToInt32(cbClass.SelectedItem.ToString().Split('-')[0]);


                        string query = "INSERT INTO SYSTEM.STUDENTSTABLE (CLASS_ID, FULL_NAME, DATE_OF_BIRTH, GENDER, ADRESS, PASSWORD) " +
                                       "VALUES (@classId, @prenomnom, @dateofbirth, @gender, @adress, @password); SELECT LAST_INSERT_ID();";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@classId", classId); // Get CLASS_ID from selected item  
                        cmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                        cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                        cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                        cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@password", Encrypt.HashString(txtPassword.Text)); // Hash the password

                        // Execute the insert command and retrieve the new student ID  
                        int newStudentId = Convert.ToInt32(cmd.ExecuteScalar());

                        // Create an account for the new student  
                        InsertData insertData = new InsertData();
                        string result = insertData.AddAccount(txtName.Text, txtPassword.Text, newStudentId, "Student", classId);

                        MessageBox.Show(result != null ? "Add success, and account created." : "Account creation failed.");
                    }
                    else // Edit existing student  
                    {    
                        // Check if a class is selected  
                        if (cbClass.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a class.");
                            return; // Exit the method if no class is selected  
                        }
                        int classId = Convert.ToInt32(cbClass.SelectedItem.ToString().Split('-')[0]);
                        string query = "UPDATE SYSTEM.STUDENTSTABLE SET CLASS_ID = @classId, FULL_NAME = @prenomnom, " +
                                       "DATE_OF_BIRTH = @dateofbirth, GENDER = @gender, ADRESS = @adress, PASSWORD = @password WHERE STUDENT_ID = @id";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@classId", classId) ; // Get CLASS_ID from selected item  
                        cmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                        cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                        cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                        cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@password", Encrypt.HashString(txtPassword.Text)); // Hash the password  
                        cmd.Parameters.AddWithValue("@id", txtID.Text); // Ensure the Student ID is included

                        cmd.ExecuteNonQuery();

                        InsertData insertData = new InsertData();
                        string result = insertData.UpdatePassword(txtName.Text, txtPassword.Text);
                        MessageBox.Show("Edit success");
                    }

                    // Clear inputs after save  
                    ClearInputs();
                    LoadStudents(); // Reload the student list to reflect changes  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            // Clear input fields  
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            cbClass.SelectedIndex = -1; // Clears selection  
            dtpBirth.Value = DateTime.Now; // Reset to current date  
            rbMale.Checked = false; // Uncheck male radio button  
            rbFemale.Checked = false; // Uncheck female radio button  
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a student to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        conn.Open();
                        string query = "DELETE FROM SYSTEM.STUDENTSTABLE WHERE STUDENT_ID=@id";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Deleted success");
                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            isSelected = false;
            showAction();
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            currFrom = 1;
            LoadStudents();
            showAction();
            txtSearch.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            cbClass.Text = "";
            dtpBirth.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadStudents();
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadStudents();
            }
        }

        private void StudentManager_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here  
        }
        private void LoadComboBoxClass()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT CLASS_ID, CLASS_NAME FROM SYSTEM.CLASS", conn); // Assuming CLASS_ID and CLASS_NAME are correct

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string classId = reader.GetInt32(0).ToString();
                            string className = reader.GetString(1);
                            cbClass.Items.Add($"{classId} - {className}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}