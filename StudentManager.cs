using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
              "S.FULL_NAME AS `Name`, " +
              "S.DATE_OF_BIRTH AS `BIRTH`, " +
              "S.GENDER AS `GENDER`, " +
              "S.ADRESS AS `Adress`, " +
              "GROUP_CONCAT(C.CLASS_ID ORDER BY C.CLASS_ID ASC) AS `Class_ID` " +
              "FROM SYSTEM.STUDENTSTABLE S " +
              "JOIN SYSTEM.STUDENT_CLASSES SC ON S.STUDENT_ID = SC.STUDENT_ID " +
              "JOIN SYSTEM.CLASS C ON SC.CLASS_ID = C.CLASS_ID " +
              "WHERE S.FULL_NAME LIKE @search " +
              "OR S.STUDENT_ID LIKE @search " +
              "OR C.CLASS_ID LIKE @search " +
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

                // STUDENT_ID
                txtID.Text = row.Cells[0].Value.ToString();

                // FULL_NAME
                txtName.Text = row.Cells[1].Value.ToString();


                DateTime dateOfBirth;
                if (!DateTime.TryParse(row.Cells[2].Value.ToString(), out dateOfBirth))
                {
                    MessageBox.Show($"Invalid date format: {row.Cells[2].Value.ToString()}");
                }

                // ADRESS (null-conditional operator used)
                txtAddress.Text = row.Cells[4].Value?.ToString() ?? string.Empty;

                // CLASS_ID  
                string classId = row.Cells[5].Value.ToString();
                txtClass.Text = classId;

                // Set gender based on cell value  
                if (row.Cells[3].Value.ToString() == "Homme")
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
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Veuillez choisir un étudiant à modifier !");
                }
                else 
                {
                    MessageBox.Show("Please choose a student to edit!");
                }
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
                            // Check the current language and show the message accordingly
                            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                            {
                                MessageBox.Show("Veuillez sélectionner une classe.");
                            }
                            else // Default to English
                            {
                                MessageBox.Show("Please select a class.");
                            }
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

                                // Check the current culture and show the message accordingly
                                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                                {
                                    // French message
                                    MessageBox.Show("Un étudiant portant ce nom existe déjà. Le nouvel étudiant sera enregistré sous le nom : " + uniqueName);
                                }
                                else
                                {
                                    // Default to English message
                                    MessageBox.Show("A student with this name already exists. The new student will be registered as: " + uniqueName);
                                }

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
                        string newStudentIdQuery = "SELECT STUDENT_ID FROM SYSTEM.STUDENTSTABLE WHERE FULL_NAME = @prenomnom ORDER BY STUDENT_ID DESC LIMIT 1";
                        using (MySqlCommand getIdCmd = new MySqlCommand(newStudentIdQuery, conn))
                        {
                            getIdCmd.Parameters.AddWithValue("@prenomnom", txtName.Text);
                            string newStudentId = getIdCmd.ExecuteScalar()?.ToString();

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

                                        // Check the current culture and show the success message accordingly
                                        if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                                        {
                                            // French success message
                                            MessageBox.Show("L'étudiant est ajoutée avec succès.");
                                        }
                                        else
                                        {
                                            // Default to English success message
                                            MessageBox.Show("Student was added successfully.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                       
                                            // Failure case
                                            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                                            {
                                                // French failure message
                                                MessageBox.Show("Échec de la création du compte.");
                                            }
                                            else
                                            {
                                                // English failure message
                                                MessageBox.Show("Account creation failed.");
                                            }
                                        

                                    }

                                }

                                // Create an account for the newly added student
                                InsertData insertData = new InsertData();
                                string result = insertData.AddAccount(txtName.Text, txtPassword.Text, newStudentId, "Student");
                                insertData.UpdateUserIdBasedOnRole(txtName.Text, "Student");
                               
                            }
                            else
                            {
                                try
                                {
                                    // Your code for insertion or data retrieval...

                                    // If something goes wrong (for example, student ID retrieval fails)
                                    MessageBox.Show("Failed to retrieve the student ID after insertion.");
                                }
                                catch 
                                {
                                    // Checking culture and displaying the appropriate message in case of failure
                                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                                    {
                                        // French failure message
                                        MessageBox.Show("Échec de la récupération de l'ID de l'étudiant après l'insertion.");
                                    }
                                    else
                                    {
                                        // English failure message
                                        MessageBox.Show("Failed to retrieve the student ID after insertion.");
                                    }
                                }

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
                            if (!string.IsNullOrEmpty(newPassword) && newPassword != currentPassword && IsPasswordValid(newPassword))
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
                            string updateAccountQuery = @"UPDATE SYSTEM.ACCOUNT 
                                  SET FULL_NAME = @prenomnom 
                                  WHERE ID = @id";

                            using (MySqlCommand updateAccountCmd = new MySqlCommand(updateAccountQuery, conn))
                            {
                                updateAccountCmd.Parameters.Clear();
                                updateAccountCmd.Parameters.AddWithValue("@prenomnom", txtName.Text);  // Full name from the input
                                updateAccountCmd.Parameters.AddWithValue("@id", studentId);

                                updateAccountCmd.ExecuteNonQuery();  // Execute the update query for ACCOUNT table
                            }




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
                // Show culture-specific message
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Veuillez sélectionner un étudiant à supprimer !");
                }
                else
                {
                    MessageBox.Show("Please choose a student to delete!");
                }
                return;
            }


            DialogResult dialogResult;
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
            {
                // Custom message box for French
                dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ?", "Confirmer", MessageBoxButtons.YesNo);
            }
            else
            {
                // Default English MessageBox
                dialogResult = MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo);
            }

            // Handle the result based on the user's selection
            if (dialogResult == DialogResult.Yes)
            {
                // Code to perform the delete action
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Supprimé avec succès");
                }
                else
                {
                    MessageBox.Show("Deleted successfully");
                }
            }
            else
            {
                // Code to handle if the user clicks 'No'
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Opération de suppression annulée");
                }
                else
                {
                    MessageBox.Show("Delete operation canceled");
                }
            }



            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        conn.Open();

                        // Delete from STUDENTS_CLASSES table
                        string deleteClassQuery = "DELETE FROM SYSTEM.STUDENT_CLASSES WHERE STUDENT_ID = @id";
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

                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                    {
                        MessageBox.Show("Suppression réussie");
                    }
                    else
                    {
                        MessageBox.Show("Deleted successfully");
                    }

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
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
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

                // Show message after export depending on the language
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Exporté vers Excel avec succès.");
                }
                else
                {
                    MessageBox.Show("Exported to Excel successfully.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors during the export process
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Erreur lors de l'exportation vers Excel: " + ex.Message);
                }
                else
                {
                    MessageBox.Show("Error exporting to Excel: " + ex.Message);
                }
            }

            LoadStudents(); // Reload students data after export
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
        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères !");
                }
                else
                {
                    MessageBox.Show("The password must contain at least 8 characters!");
                }
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    MessageBox.Show("Le mot de passe doit contenir au moins un caractère spécial !");
                }
                else
                {
                    MessageBox.Show("The password must contain at least one special character!");
                }
                return false;
            }

            return true;
        }

    }
}
    



