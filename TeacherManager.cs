using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace SchoolManagement
{
    public partial class TeacherManager : KryptonForm
    {
        private int action; // 0 - add, 1 - edit  
        private bool isSelected = false;
        private string connectionString = "Server=localhost;Database=system;User ID=root;Password=samia;";
        private int currFrom = 1;
        private int pageSize = 10;

        public TeacherManager()
        {
            InitializeComponent();
            LoadTeachers();
            LoadComboBoxDepartment();
           


        }

        #region Load Data Methods

        private void LoadComboBoxDepartment()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT DEP_ID, DEP_NAME FROM DEP", conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbDepartment.Items.Clear();
                        while (reader.Read())
                        {
                            // Using string concatenation instead of string interpolation
                            cbDepartment.Items.Add(reader.GetString(0) + " - " + reader.GetString(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading departments: " + ex.Message);
            }
        }


        private void LoadTeachers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    a.TEACHER_ID AS `Teacher ID`, 
                    CONCAT(a.DEP_ID, ' - ', d.DEP_NAME) AS `Department`,  
                    a.FULL_NAME AS `Name`,  
                    a.DATE_OF_BIRTH AS `Birth`, 
                    a.GENDER AS `Gender`, 
                    a.ADRESS AS `Address`
                FROM SYSTEM.TEACHER a  
                JOIN SYSTEM.DEP d ON a.DEP_ID = d.DEP_ID
                LIMIT @PageSize OFFSET @Offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Correct parameter binding
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.AddWithValue("@Offset", (currFrom - 1) * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Ensure correct DataGridView is set here (dgvTeachers or another control)
                            dgvTeachers.DataSource = dataTable;  // Correct DataGridView to show teacher data
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teachers: " + ex.Message);
            }
        }

        #endregion

        #region Button Click Events

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a teacher to edit!");
                return;
            }
            action = 1;
            SetEditMode();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            SaveTeacher();
        }

        #endregion

        #region Save / Update Teacher

        private void SaveTeacher()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (action == 0) // Ajout d'un nouvel enseignant
                    {
                        if (cbDepartment.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a department.");
                            return;
                        }

                        string depId = cbDepartment.SelectedItem.ToString().Split('-')[0].Trim();
                        string query = "INSERT INTO SYSTEM.TEACHER (FULL_NAME, ADRESS, GENDER, DATE_OF_BIRTH, DEP_ID) " +
                                       "VALUES (@fullname, @adress, @gender, @dateofbirth, @depId); " +
                                       "SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                            cmd.Parameters.AddWithValue("@depId", depId);

                            // Récupération de l'ID de l'enseignant
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                string newTeacherId = result.ToString();

                                InsertData insertData = new InsertData();
                                insertData.AddAccount(txtName.Text, txtPassword.Text, newTeacherId, "Teacher");
                                insertData.UpdateUserIdBasedOnRole(txtName.Text, "Teacher");

                                
                            }
                            else
                            {
                                MessageBox.Show("Failed to retrieve the teacher's ID after insertion.");
                            }
                        }
                    }
                    else // Mise à jour d'un enseignant
                    {
                        if (cbDepartment.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a department.");
                            return;
                        }

                        string teacherId = txtID.Text;
                        if (string.IsNullOrWhiteSpace(teacherId))
                        {
                            MessageBox.Show("Teacher ID is required.");
                            return;
                        }

                        string depId = cbDepartment.SelectedItem.ToString().Split('-')[0].Trim();
                        string query = "UPDATE SYSTEM.TEACHER SET FULL_NAME = @fullname, ADRESS = @adress, " +
                                       "GENDER = @gender, DATE_OF_BIRTH = @dateofbirth, DEP_ID = @depId WHERE TEACHER_ID = @id";

                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                            cmd.Parameters.AddWithValue("@depId", depId);
                            cmd.Parameters.AddWithValue("@id", teacherId);  // Utilisation de teacherId comme chaîne

                            cmd.ExecuteNonQuery();
                        }
                        string newPassword = txtPassword.Text.Trim(); // Assuming txtPassword is where the new password is entered

                        // Fetch the current password from the database before updating
                        string currentPassword = GetCurrentPassword(teacherId, conn); // Pass connection to the method
                        // Mise à jour du mot de passe uniquement si un nouveau est saisi
                        if (!string.IsNullOrEmpty(newPassword) && newPassword != currentPassword && IsPasswordValid(newPassword))
                        {
                            // Hash the new password before saving it
                            string hashedPassword = Encrypt.HashString(newPassword);  // Hash the password

                            // Only update password if it was changed
                            InsertData insertData = new InsertData();
                            string updateResult = insertData.UpdatePassword(teacherId, hashedPassword); // Pass the hashed password

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
                            MessageBox.Show("No password change detected.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                ClearInputs();
                showAction();
                LoadTeachers();
            }
        }

        public string GetCurrentPassword(string teacherId, MySqlConnection conn)
        {
            string password = string.Empty;
            string query = "SELECT PASSWORD FROM SYSTEM.ACCOUNT WHERE ID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", teacherId);
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
        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a teacher to delete!");
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

                       

                        string deleteQuery = "DELETE FROM SYSTEM.teacher WHERE teacher_ID=@id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                        string deleteAccountQuery = "DELETE FROM SYSTEM.ACCOUNT WHERE ID=@id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteAccountQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Deleted success");
                    LoadTeachers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            isSelected = false;
            showAction();
        }

        #endregion

        #region Helper Methods

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            cbDepartment.SelectedIndex = -1;
            dtpBirth.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }
        private void showAction()
        {
            pbTeachers.Visible = true;
            lbAddTeacher.Visible = true;
            pbEdit.Visible = true;
            lbEditTeacher.Visible = true;
            pbDelete.Visible = true;
            lbDeleteTeacher.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }
        private void SetEditMode()
        {
            pbTeachers.Visible = false;
            lbAddTeacher.Visible = false;
            pbDelete.Visible = false;
            lbDeleteTeacher.Visible = false;
            pbEdit.Visible = false;
            lbEditTeacher.Visible = false;
            pbDelete.Visible = false;
            lbSave.Visible = true;
            pbSave.Visible = true;
            txtID.Enabled = false;
            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbDepartment.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }
        private void dgvTeachers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                DataGridViewRow row = dgvTeachers.Rows[e.RowIndex];

                // Récupération de l'ID (qui est un VARCHAR) et affichage dans txtID
                string teacherId = row.Cells[0].Value.ToString();
                txtID.Text = teacherId;

                // Vérification de la valeur de l'ID dans txtID
                MessageBox.Show("Selected Teacher ID: " + teacherId); // Concatenation instead of interpolation

                // Chargement des autres informations
                txtName.Text = row.Cells[2].Value.ToString();
                txtAddress.Text = row.Cells[5].Value.ToString();
                dtpBirth.Value = DateTime.Parse(row.Cells[3].Value.ToString());

                string departmentInfo = row.Cells[1].Value.ToString();
                cbDepartment.Text = departmentInfo;

                rbMale.Checked = row.Cells[4].Value.ToString() == "Homme";
                rbFemale.Checked = !rbMale.Checked;
            }
        }




        #endregion
        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadTeachers();
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadTeachers();
            }
        }


        private void ExportToExcel()
        {
            try
            {
                // Create a new Excel application instance
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);

                // Add column headers to the Excel file
                for (int col = 0; col < dgvTeachers.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvTeachers.Columns[col].HeaderText;
                }

                // Fetch all data for the export, not just the current page
                List<DataRow> allRows = new List<DataRow>();

                // Loop through all pages and collect all data
                int totalRecords = GetTotalRecordCount(); // Get total record count from DB
                int totalPages = (totalRecords + pageSize - 1) / pageSize; // Calculate number of pages

                for (int page = 1; page <= totalPages; page++)
                {
                    currFrom = page; // Update current page number
                    LoadTeachers();  // This loads students for the current page

                    // Collect rows from the DataGridView
                    foreach (DataGridViewRow row in dgvTeachers.Rows)
                    {
                        if (row.IsNewRow) continue; // Skip the new row placeholder

                        DataRow dataRow = ((DataTable)dgvTeachers.DataSource).NewRow();

                        // Copy row values to DataRow
                        for (int col = 0; col < dgvTeachers.Columns.Count; col++)
                        {
                            dataRow[col] = row.Cells[col].Value.ToString();
                        }

                        allRows.Add(dataRow); // Add the row to the list
                    }
                }

                // Populate Excel worksheet with all rows collected
                int rowIndex = 2; // Start from row 2 (because row 1 is the header)
                foreach (var row in allRows)
                {
                    for (int col = 0; col < dgvTeachers.Columns.Count; col++)
                    {
                        worksheet.Cells[rowIndex, col + 1] = row[col].ToString();
                    }
                    rowIndex++;
                }

                MessageBox.Show("Exported to Excel successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message);
            }
            LoadTeachers();
        }
        private int GetTotalRecordCount()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT COUNT(teacher_id) FROM SYSTEM.teacher";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                // Afficher un message d'erreur spécifique pour MySQL  
                MessageBox.Show("MySQL Error: " + mysqlEx.Message);
                return 0;
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur général  
                MessageBox.Show("Error: " + ex.Message);
                return 0; // Return 0 in case of an error  
            }
        }
        private void TeacherManager_Load(object sender, EventArgs e)
        {

        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pbTeachers_Click(object sender, EventArgs e)
        {
        
            action = 0;

            pbTeachers.Visible = false;
            lbAddTeacher.Visible = false;
            pbEdit.Visible = false;
            lbEditTeacher.Visible = false;
            pbDelete.Visible = false;
            lbDeleteTeacher.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;
            txtID.Text = "";
            txtID.Visible = true;
            lbTeacherID.Visible = true;
            cbDepartment.Text = "";
            cbDepartment.Enabled = true;
            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;

            txtPassword.Text = "";
            txtPassword.Enabled = true;

            

            dtpBirth.Enabled = true;

            rbMale.Checked = false;
            rbMale.Enabled = true;
            rbFemale.Checked = false;
            rbFemale.Enabled = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT TEACHER_ID, DEP_ID AS `Dep`, FULL_NAME AS `Name`, DATE_OF_BIRTH AS `BIRTH`, " +
                                   "GENDER AS `GENDER`, ADRESS AS `Adress` " +
                                   "FROM SYSTEM.teacher WHERE FULL_NAME LIKE @search OR teacher_ID LIKE @search " +
                                   "OR dep_ID LIKE @search OR GENDER LIKE @search OR ADRESS LIKE @search";

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

        private void pbReload_Click(object sender, EventArgs e)
        {
            currFrom = 1;
            LoadTeachers();
            showAction();
            txtSearch.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            cbDepartment.Text = "";
            dtpBirth.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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
        private void dgvTeachers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

