using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; 
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


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
        private int action;
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public StudentManager()
        {
            InitializeComponent();
            LoadStudents();
            LoadListBox1Class();
          

        }

        private void LoadStudents()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                // Create a connection using the connection string
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // Create a command object
                    string query = @"
                SELECT 
                    s.STUDENT_ID, 
                    s.FULL_NAME AS Name, 
                    s.DATE_OF_BIRTH AS Birth, 
                    s.GENDER AS Gender, 
                    s.ADRESS AS Adress,
                    GROUP_CONCAT(sc.CLASS_ID ORDER BY sc.CLASS_ID) AS CLASS_ID
                FROM SYSTEM.STUDENTSTABLE s
                JOIN SYSTEM.STUDENT_CLASSES sc ON s.STUDENT_ID = sc.STUDENT_ID
                GROUP BY s.STUDENT_ID, s.FULL_NAME, s.DATE_OF_BIRTH, s.GENDER, s.ADRESS
                ORDER BY s.STUDENT_ID";


                    // Create the MySqlCommand object
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Execute the query and load the result into a DataTable
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Bind the DataTable to the DataGridView
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Show error message if there's an exception
                MessageBox.Show(ex.Message);
            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string mySqlDb = "Server =localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                                String query = "SELECT " +
                          "S.STUDENT_ID, " +
                          "_Lingobit_S.FULL_NAME AS `Name`, " +
                          "S.DATE_OF_BIRTH AS `BIRTH`, " +
                          "S.GENDER AS `GENDER`, " +
                          "S.ADRESS AS `Adress`, " +
                          "GROUP_CONCAT(C.CLASS_ID ORDER BY C.CLASS_ID ASC) AS `Class_ID` " +
                          "FROM SYSTEM.STUDENTSTABLE S " +
                          "JOIN SYSTEM.STUDENT_CLASSES SC ON S.STUDENT_ID = SC.STUDENT_ID " +
                          "JOIN SYSTEM.CLASS C ON SC.CLASS_ID = C.CLASS_ID " +
                          "WHERE S.FULL_NAME LIKE @search " +
                          "OR S.STUDENT_ID LIKE @search " +
                          "_Lingobit_OR C.CLASS_ID LIKE @search " +
                          "OR S.GENDER LIKE @search " +
                          "OR S.ADRESS LIKE @search " +
                          "GROUP BY S.STUDENT_ID, S.FULL_NAME, S.DATE_OF_BIRTH, S.GENDER, S.ADRESS";



                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvStudents.DataSource = dataTable;
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

                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];

                txtID.Text = row.Cells[0].Value.ToString(); // STUDENT_ID
                txtName.Text = row.Cells[1].Value.ToString(); // FULL_NAME

                // Correctly set the date of birth  
                if (!DateTime.TryParse(row.Cells[2].Value.ToString(), out DateTime dateOfBirth))
                {
                    MessageBox.Show("Invalid date format: " + row.Cells[2].Value.ToString());
                }

                txtAddress.Text = row.Cells[4].Value?.ToString() ?? string.Empty; // ADRESS

                
                string classId = row.Cells[5].Value.ToString(); // CLASS_ID  
                
                
                    
                    txtClass.Text = classId; //
                

                // Set gender based on cell value  
                if (row.Cells[3].Value.ToString() == "_Lingobit_Homme")
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
            lbAddStudents.Visible = false;
            pbEdit.Visible = false;
            lbEditStudent.Visible = false;
            pbDelete.Visible = false;
            lbDeleteStudent.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;
            txtID.Text = "";

            txtID.Visible = true;
            lbStudentID.Visible = true;

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;

            txtPassword.Text = "";
            txtPassword.Enabled = true;

            txtClass.Enabled = true;

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
            txtID.Visible = true;
            lbStudentID.Visible = true;
            pbStudents.Visible = false;
            lbAddStudents.Visible = false;
            pbEdit.Visible = false;
            lbEditStudent.Visible = false;
            pbDelete.Visible = false;
            lbDeleteStudent.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Enabled = false;
            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            txtClass.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void showAction()
        {
            
            pbStudents.Visible = true;
            lbAddStudents.Visible = true;
            pbEdit.Visible = true;
            lbEditStudent.Visible = true;
            pbDelete.Visible = true;
            lbDeleteStudent.Visible = true;
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
                    MySqlCommand cmd = new MySqlCommand(); // Initialization of cmd
                    cmd.Connection = conn; // Associate the connection

                    if (action == 0) // Add a new student
                    {
                        if (string.IsNullOrEmpty(txtClass.Text))
                        {
                            MessageBox.Show("Please select a class.");
                            return;
                        }

                        string classId = txtClass.Text.Split('-')[0].Trim(); // Retrieve CLASS_ID as a string

                        // Check if a student with the same name already exists
                        string checkStudentQuery = "SELECT COUNT(*) FROM SYSTEM.STUDENTSTABLE WHERE FULL_NAME = @prenomnom";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkStudentQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                            int studentExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (studentExists > 0)
                            {
                                // Optionally append a unique identifier to the name (e.g., append "_1" or "_timestamp")
                                string uniqueName = txtName.Text + "_" + DateTime.Now.Ticks; // Append unique timestamp to the name
                                MessageBox.Show("A student with this name already exists. The new student will be registered as: " + uniqueName);

                                // Now use the unique name for insertion instead of the original name
                                txtName.Text = uniqueName;
                            }
                        }

                        // Insert a new student into the STUDENTSTABLE
                        string query = "INSERT INTO SYSTEM.STUDENTSTABLE (FULL_NAME, DATE_OF_BIRTH, GENDER, ADRESS) " +
                                       "VALUES (@prenomnom, @dateofbirth, @gender, @address);";
                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@prenomnom", txtName.Text); // Use the unique name here
                        cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                        cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);

                        // Execute the command to insert the student
                        cmd.ExecuteNonQuery();

                        // Retrieve the newly generated STUDENT_ID from the database
                        string newStudentIdQuery = "_Lingobit_SELECT STUDENT_ID FROM SYSTEM.STUDENTSTABLE WHERE FULL_NAME = @prenomnom ORDER BY STUDENT_ID DESC LIMIT 1";
                        using (MySqlCommand getIdCmd = new MySqlCommand(newStudentIdQuery, conn))
                        {
                            getIdCmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                            string newStudentId = getIdCmd.ExecuteScalar()?.ToString();
                            Console.WriteLine($"New Student ID: {newStudentId}"); // Debug Output

                            if (!string.IsNullOrEmpty(newStudentId))
                            {
                                // Now insert the student's class into the STUDENT_CLASSES table
                                string insertClassQuery = "INSERT INTO SYSTEM.STUDENT_CLASSES (STUDENT_ID, CLASS_ID) VALUES (@studentId, @classId)";
                                using (MySqlCommand classCmd = new MySqlCommand(insertClassQuery, conn))
                                {
                                    classCmd.Parameters.AddWithValue("@studentId", newStudentId);
                                    classCmd.Parameters.AddWithValue("@classId", classId);

                                    try
                                    {
                                        // Execute the insert for the student's class
                                        classCmd.ExecuteNonQuery();
                                        MessageBox.Show("Student class added successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        // Show error message if insertion fails
                                        MessageBox.Show("Error inserting student class: " + ex.Message);
                                    }
                                }

                                // Create an account for the newly added student
                                InsertData insertData = new InsertData();
                                string result = insertData.AddAccount(txtName.Text, txtPassword.Text, newStudentId, "Student");
                                insertData.UpdateUserIdBasedOnRole(txtName.Text, "Student");

                                // Notify the user about account creation
                                MessageBox.Show(result != null ? "Add success, and account created." : "_Lingobit_Account creation failed.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to retrieve the student ID after insertion.");
                            }
                        }

                        // Refresh the student list after adding the student
                        LoadStudents();
                        showAction();
                    }
          



                    else // Edit existing student
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(txtClass.Text))
                            {
                                MessageBox.Show("Please select at least one class.");
                                return;
                            }

                            string[] classIds = txtClass.Text.Split(','); // Split the selected class IDs
                            string studentId = txtID.Text;
                            string newPassword = txtPassword.Text.Trim(); // Assuming txtPassword is where the new password is entered

                            // Fetch the current password from the database before updating
                            string currentPassword = GetCurrentPassword(studentId, conn); // Pass connection to the method

                            // Update the student information
                            string updateQuery = @"UPDATE SYSTEM.STUDENTSTABLE 
                       SET FULL_NAME = @prenomnom, 
                           DATE_OF_BIRTH = @dateofbirth, 
                           GENDER = @gender, 
                           ADRESS = @address  
                       WHERE STUDENT_ID = @id";

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.Clear();
                                updateCmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                                updateCmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                                updateCmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                                updateCmd.Parameters.AddWithValue("@address", txtAddress.Text);
                                updateCmd.Parameters.AddWithValue("@id", studentId);

                                updateCmd.ExecuteNonQuery();
                            }

                            // Check if password was changed
                            if (!string.IsNullOrEmpty(newPassword) && newPassword != currentPassword)
                            {
                                // Hash the new password before saving it
                                string hashedPassword = Encrypt.HashString(newPassword);  // Hash the password

                                // Only update password if it was changed
                                InsertData insertData = new InsertData();
                                string updateResult = insertData.UpdatePassword(studentId, hashedPassword); // Pass the hashed password

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


                            // Delete existing class associations for the student before adding new ones
                            string deleteClassQuery = "DELETE FROM SYSTEM.STUDENT_CLASSES WHERE STUDENT_ID = @id";
                            using (MySqlCommand deleteCmd = new MySqlCommand(deleteClassQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@id", studentId);
                                deleteCmd.ExecuteNonQuery();
                            }

                            // Insert new class associations
                            foreach (var classId in classIds)
                            {
                                string cleanClassId = classId.Trim(); 
                                string insertClassQuery = "INSERT INTO SYSTEM.STUDENT_CLASSES (STUDENT_ID, CLASS_ID) VALUES (@id, @classId)";
                                using (MySqlCommand classCmd = new MySqlCommand(insertClassQuery, conn))
                                {
                                    classCmd.Parameters.AddWithValue("@id", studentId);
                                    classCmd.Parameters.AddWithValue("@classId", cleanClassId); 
                                    classCmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("_Lingobit_Edit success");

                            // Clear inputs after save
                            ClearInputs();
                            LoadStudents();
                            showAction();
                            kryptonListBox1.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public string GetCurrentPassword(string studentId, MySqlConnection conn)
        {
            string password = string.Empty;
            string query = "SELECT PASSWORD FROM SYSTEM.ACCOUNT WHERE ID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", studentId);
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


        private void ClearInputs()
        {
            // Clear input fields  
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
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

                        // Delete from STUDENTS_CLASSES table
                        string deleteClassQuery = "_Lingobit_DELETE FROM SYSTEM.STUDENT_CLASSES WHERE STUDENT_ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteClassQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from STUDENTSTABLE table
                        string deleteQuery = "DELETE FROM SYSTEM.STUDENTSTABLE WHERE STUDENT_ID=@id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                        // Suppression du compte associé à l'étudiant dans la table SYSTEM.ACCOUNTTABLE  
                        string deleteAccountQuery = "DELETE FROM SYSTEM.ACCOUNT WHERE ID=@id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteAccountQuery, conn))
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
            txtClass.Text = "";
            dtpBirth.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }


        // Logic for handling the "Next" and "Previous" buttons
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

        private void UpdatePaginationButtons()
        {
            pbPrev.Enabled = currFrom > 1; 
            pbNext.Enabled = currFrom * pageSize < GetTotalRecordCount(); 
        }

        private int GetTotalRecordCount()
        {
            try
            {
                string mySqlDb = "_Lingobit_Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT COUNT(DISTINCT STUDENT_ID) FROM SYSTEM.STUDENTSTABLE";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0; // Return 0 in case of an error
            }

        }

        private void LoadListBox1Class()
        {
            try
                    {    
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT CLASS_ID FROM SYSTEM.CLASS", conn); 

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string classId = reader.GetString(0);
                            kryptonListBox1.Items.Add(classId);
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        


        private void picturebox1_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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
                for (int col = 0; col < dgvStudents.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvStudents.Columns[col].HeaderText;
                }

                // Fetch all data for the export, not just the current page
                List<DataRow> allRows = new List<DataRow>();

                // Loop through all pages and collect all data
                int totalRecords = GetTotalRecordCount(); // Get total record count from DB
                int totalPages = (totalRecords + pageSize - 1) / pageSize; // Calculate number of pages

                for (int page = 1; page <= totalPages; page++)
                {
                    currFrom = page; // Update current page number
                    LoadStudents();  // This loads students for the current page

                    // Collect rows from the DataGridView
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        if (row.IsNewRow) continue; // Skip the new row placeholder

                        DataRow dataRow = ((DataTable)dgvStudents.DataSource).NewRow();

                        // Copy row values to DataRow
                        for (int col = 0; col < dgvStudents.Columns.Count; col++)
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
                    for (int col = 0; col < dgvStudents.Columns.Count; col++)
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
            LoadStudents();
        }



        private void txtClass_Click(object sender, EventArgs e)
        {
            kryptonListBox1.Visible = true;
            kryptonListBox1.ClearSelected();

        }

        private void kryptonListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> selectedClasses = new List<string>();
            foreach (var item in kryptonListBox1.SelectedItems)
            {
                selectedClasses.Add(item.ToString());
            }
            txtClass.Text = string.Join(", ", selectedClasses);
        }

        private void txtClass_Leave(object sender, EventArgs e)
        {
            kryptonListBox1.Visible = false; 
        }

        private void StudentManager_Click(object sender, EventArgs e)
        {
            if (!txtClass.Focused && !kryptonListBox1.Focused)
            {
                kryptonListBox1.Visible = false;
            }
        }

        private void StudentManager_Load(object sender, EventArgs e)
        {
            kryptonListBox1.Visible = false;

        }

       
    }
    }
    



