using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class StudentManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public StudentManager()
        {
            InitializeComponent();
            LoadStudents();
            LoadListBox1Class();
            UpdatePaginationButtons();
        }

        #region Load Data Methods
        private void LoadStudents()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            s.STUDENT_ID, 
                            s.FULL_NAME AS `Name`, 
                            s.DATE_OF_BIRTH AS `Birth`, 
                            s.GENDER AS `Gender`, 
                            s.ADRESS AS `Address`,
                            GROUP_CONCAT(sc.CLASS_ID ORDER BY sc.CLASS_ID SEPARATOR ', ') AS `Class ID`
                        FROM SYSTEM.studentstable s
                        LEFT JOIN SYSTEM.student_classes sc ON s.STUDENT_ID = sc.STUDENT_ID
                        GROUP BY s.STUDENT_ID, s.FULL_NAME, s.DATE_OF_BIRTH, s.GENDER, s.ADRESS
                        ORDER BY s.STUDENT_ID
                        LIMIT @limit OFFSET @offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
                UpdatePaginationButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
        }

        private void LoadListBox1Class()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CLASS_ID FROM SYSTEM.class";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            kryptonListBox1.Items.Clear();
                            while (reader.Read())
                            {
                                kryptonListBox1.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
        }
        #endregion

        #region Event Handlers
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            s.STUDENT_ID, 
                            s.FULL_NAME AS `Name`, 
                            s.DATE_OF_BIRTH AS `Birth`, 
                            s.GENDER AS `Gender`, 
                            s.ADRESS AS `Address`, 
                            GROUP_CONCAT(sc.CLASS_ID ORDER BY sc.CLASS_ID SEPARATOR ', ') AS `Class ID`
                        FROM SYSTEM.studentstable s
                        LEFT JOIN SYSTEM.student_classes sc ON s.STUDENT_ID = sc.STUDENT_ID
                        LEFT JOIN SYSTEM.class c ON sc.CLASS_ID = c.CLASS_ID
                        WHERE s.FULL_NAME LIKE @search 
                           OR s.STUDENT_ID LIKE @search 
                           OR c.CLASS_ID LIKE @search 
                           OR s.GENDER LIKE @search 
                           OR s.ADRESS LIKE @search
                        GROUP BY s.STUDENT_ID, s.FULL_NAME, s.DATE_OF_BIRTH, s.GENDER, s.ADRESS
                        ORDER BY s.STUDENT_ID
                        LIMIT @limit OFFSET @offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
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
                DateTime dateOfBirth;
                if (DateTime.TryParse(row.Cells[2].Value.ToString(), out dateOfBirth))
                {
                    dtpBirth.Value = dateOfBirth;
                }
                else
                {
                    dtpBirth.Value = DateTime.Now;
                }
                txtAddress.Text = row.Cells[4].Value?.ToString() ?? string.Empty; // ADRESS
                txtClass.Text = row.Cells[5].Value?.ToString() ?? string.Empty; // CLASS_ID
                rbMale.Checked = row.Cells[3].Value.ToString() == "Homme";
                rbFemale.Checked = !rbMale.Checked;
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0;
            SetAddMode();
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToEdit"));
                return;
            }
            action = 1;
            SetEditMode();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            SaveStudent();
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToDelete"));
                return;
            }

            if (MessageBox.Show(GetLocalizedMessage("ConfirmDelete"), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Delete from STUDENT_CLASSES
                        string deleteClassQuery = "DELETE FROM SYSTEM.student_classes WHERE STUDENT_ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteClassQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from STUDENTSTABLE
                        string deleteStudentQuery = "DELETE FROM SYSTEM.studentstable WHERE STUDENT_ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteStudentQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from ACCOUNT
                        string deleteAccountQuery = "DELETE FROM SYSTEM.account WHERE ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteAccountQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(GetLocalizedMessage("DeleteSuccess"));
                        LoadStudents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
                }
                finally
                {
                    isSelected = false;
                    showAction();
                }
            }
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            ResetForm();
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

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
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
        #endregion

        #region Helper Methods
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

        private void SetAddMode()
        {
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
            txtID.Enabled = false; // ID is auto-generated

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;

            txtPassword.Text = "";
            txtPassword.Enabled = true;

            txtClass.Text = "";
            txtClass.Enabled = true;

            dtpBirth.Value = DateTime.Now;
            dtpBirth.Enabled = true;

            rbMale.Checked = false;
            rbMale.Enabled = true;
            rbFemale.Checked = false;
            rbFemale.Enabled = true;
        }

        private void SetEditMode()
        {
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

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            txtClass.Text = "";
            dtpBirth.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void ResetForm()
        {
            currFrom = 1;
            isSelected = false;
            showAction();
            LoadStudents();
            ClearInputs();
            txtSearch.Text = "";
        }

        private void SaveStudent()
        {
            if (!ValidateInputs()) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (action == 0) // Add new student
                    {
                        // Generate a unique identifier (e.g., timestamp) to help retrieve the record
                        string uniqueIdentifier = DateTime.Now.Ticks.ToString();

                        // Insert into studentstable and let the trigger generate the STUDENT_ID
                        string insertStudentQuery = @"
                            INSERT INTO SYSTEM.studentstable (FULL_NAME, DATE_OF_BIRTH, GENDER, ADRESS)
                            VALUES (@FullName, @DateOfBirth, @Gender, @Address)";

                        using (MySqlCommand cmd = new MySqlCommand(insertStudentQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", txtName.Text + " " + uniqueIdentifier);
                            cmd.Parameters.AddWithValue("@DateOfBirth", dtpBirth.Value.Date);
                            cmd.Parameters.AddWithValue("@Gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.ExecuteNonQuery();
                        }

                        // Retrieve the auto-generated Student_ID using the unique identifier
                        string newStudentId;
                        string selectStudentIdQuery = @"
                            SELECT STUDENT_ID 
                            FROM SYSTEM.studentstable 
                            WHERE FULL_NAME = @FullName
                            ORDER BY STUDENT_ID DESC LIMIT 1";

                        using (MySqlCommand cmd = new MySqlCommand(selectStudentIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", txtName.Text + " " + uniqueIdentifier);
                            newStudentId = cmd.ExecuteScalar()?.ToString();
                        }

                        if (!string.IsNullOrEmpty(newStudentId))
                        {
                            // Update the FULL_NAME to remove the unique identifier
                            string updateStudentQuery = @"
                                UPDATE SYSTEM.studentstable 
                                SET FULL_NAME = @FullName 
                                WHERE STUDENT_ID = @StudentID";
                            using (MySqlCommand cmd = new MySqlCommand(updateStudentQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                                cmd.Parameters.AddWithValue("@StudentID", newStudentId);
                                cmd.ExecuteNonQuery();
                            }

                            // Insert class associations using the auto-generated Student_ID
                            string[] classIds = txtClass.Text.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var classId in classIds)
                            {
                                string insertClassQuery = "INSERT INTO SYSTEM.student_classes (STUDENT_ID, CLASS_ID) VALUES (@StudentID, @ClassID)";
                                using (MySqlCommand classCmd = new MySqlCommand(insertClassQuery, conn))
                                {
                                    classCmd.Parameters.AddWithValue("@StudentID", newStudentId);
                                    classCmd.Parameters.AddWithValue("@ClassID", classId.Trim());
                                    classCmd.ExecuteNonQuery();
                                }
                            }

                            // Create account using the auto-generated Student_ID
                            string hashedPassword = Encrypt.HashString(txtPassword.Text);
                            DataManager dataManager = new DataManager();
                            dataManager.AddAccount(newStudentId, txtName.Text, hashedPassword);

                            MessageBox.Show(GetLocalizedMessage("SaveSuccess"));
                        }
                        else
                        {
                            MessageBox.Show(GetLocalizedMessage("InsertFailed"));
                        }
                    }
                    else // Edit existing student
                    {
                        string studentId = txtID.Text;

                        // Update studentstable
                        string updateStudentQuery = @"
                            UPDATE SYSTEM.studentstable 
                            SET FULL_NAME = @FullName, DATE_OF_BIRTH = @DateOfBirth, GENDER = @Gender, ADRESS = @Address
                            WHERE STUDENT_ID = @ID";

                        using (MySqlCommand cmd = new MySqlCommand(updateStudentQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                            cmd.Parameters.AddWithValue("@DateOfBirth", dtpBirth.Value.Date);
                            cmd.Parameters.AddWithValue("@Gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@ID", studentId);
                            cmd.ExecuteNonQuery();
                        }

                        // Update password if changed
                        if (!string.IsNullOrEmpty(txtPassword.Text) && IsPasswordValid(txtPassword.Text))
                        {
                            string hashedPassword = Encrypt.HashString(txtPassword.Text);
                            DataManager dataManager = new DataManager();
                            dataManager.UpdatePassword(studentId, hashedPassword);
                        }

                        // Update class associations
                        string deleteClassQuery = "DELETE FROM SYSTEM.student_classes WHERE STUDENT_ID = @ID";
                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteClassQuery, conn))
                        {
                            deleteCmd.Parameters.AddWithValue("@ID", studentId);
                            deleteCmd.ExecuteNonQuery();
                        }

                        string[] classIds = txtClass.Text.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var classId in classIds)
                        {
                            string insertClassQuery = "INSERT INTO SYSTEM.student_classes (STUDENT_ID, CLASS_ID) VALUES (@StudentID, @ClassID)";
                            using (MySqlCommand classCmd = new MySqlCommand(insertClassQuery, conn))
                            {
                                classCmd.Parameters.AddWithValue("@StudentID", studentId);
                                classCmd.Parameters.AddWithValue("@ClassID", classId.Trim());
                                classCmd.ExecuteNonQuery();
                            }
                        }

                        // Update ACCOUNT
                        string updateAccountQuery = "UPDATE SYSTEM.account SET FULL_NAME = @FullName WHERE ID = @ID";
                        using (MySqlCommand updateAccountCmd = new MySqlCommand(updateAccountQuery, conn))
                        {
                            updateAccountCmd.Parameters.AddWithValue("@FullName", txtName.Text);
                            updateAccountCmd.Parameters.AddWithValue("@ID", studentId);
                            updateAccountCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(GetLocalizedMessage("SaveSuccess"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
            finally
            {
                ResetForm();
            }
        }

        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Students as CSV";
                saveFileDialog.FileName = "Students_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchStudentDataAsync();

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();

                        // Écrire les en-têtes
                        string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                            .Select(column => $"\"{column.ColumnName}\"")
                            .ToArray();
                        csvContent.AppendLine(string.Join(",", columnNames));

                        // Écrire les données
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string[] fields = row.ItemArray.Select(field =>
                                $"\"{(field != null ? field.ToString().Replace("\"", "\"\"") : "")}\"")
                                .ToArray();
                            csvContent.AppendLine(string.Join(",", fields));
                        }

                        // Écrire dans le fichier
                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                        MessageBox.Show(GetLocalizedMessage("ExportSuccess"));
                        System.Diagnostics.Process.Start(saveFileDialog.FileName); // Ouvre le fichier
                    }
                    else
                    {
                        MessageBox.Show("No data to export.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchStudentDataAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"
                    SELECT 
                        s.STUDENT_ID, 
                        s.FULL_NAME AS `Name`, 
                        s.DATE_OF_BIRTH AS `Birth`, 
                        s.GENDER AS `Gender`, 
                        s.ADRESS AS `Address`,
                        GROUP_CONCAT(sc.CLASS_ID ORDER BY sc.CLASS_ID SEPARATOR ', ') AS `Class ID`
                    FROM SYSTEM.studentstable s
                    LEFT JOIN SYSTEM.student_classes sc ON s.STUDENT_ID = sc.STUDENT_ID
                    GROUP BY s.STUDENT_ID, s.FULL_NAME, s.DATE_OF_BIRTH, s.GENDER, s.ADRESS
                    ORDER BY s.STUDENT_ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    try
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
                        return dataTable;
                    }
                    finally
                    {
                        adapter.Dispose();
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            finally
            {
                conn.Dispose();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(GetLocalizedMessage("NameRequired"));
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtClass.Text))
            {
                MessageBox.Show(GetLocalizedMessage("ClassRequired"));
                return false;
            }
            if (action == 0 && string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(GetLocalizedMessage("PasswordRequired"));
                return false;
            }
            if (!string.IsNullOrEmpty(txtPassword.Text) && !IsPasswordValid(txtPassword.Text))
            {
                return false;
            }
            return true;
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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(DISTINCT STUDENT_ID) FROM SYSTEM.studentstable";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
                return 0;
            }
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                MessageBox.Show(GetLocalizedMessage("PasswordTooShort"));
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                MessageBox.Show(GetLocalizedMessage("PasswordNoSpecialChar"));
                return false;
            }
            return true;
        }

        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Veuillez choisir un étudiant à modifier !" },
                    { "NoRecordToDelete", "Veuillez sélectionner un étudiant à supprimer !" },
                    { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                    { "SaveSuccess", "Étudiant enregistré avec succès." },
                    { "DeleteSuccess", "Suppression réussie." },
                    { "InsertFailed", "Échec de l'ajout de l'étudiant." },
                    { "NameRequired", "Le nom de l'étudiant est requis." },
                    { "ClassRequired", "Veuillez sélectionner au moins une classe." },
                    { "PasswordRequired", "Le mot de passe est requis pour un nouvel étudiant." },
                    { "PasswordTooShort", "Le mot de passe doit contenir au moins 8 caractères !" },
                    { "PasswordNoSpecialChar", "Le mot de passe doit contenir au moins un caractère spécial !" },
                    { "Error", "Erreur : " },
                    { "ExportSuccess", "Exporté avec succès vers CSV." }
                }
                : new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Please choose a student to edit!" },
                    { "NoRecordToDelete", "Please choose a student to delete!" },
                    { "ConfirmDelete", "Are you sure you want to delete?" },
                    { "SaveSuccess", "Student saved successfully." },
                    { "DeleteSuccess", "Deleted successfully." },
                    { "InsertFailed", "Failed to add student." },
                    { "NameRequired", "Student name is required." },
                    { "ClassRequired", "Please select at least one class." },
                    { "PasswordRequired", "Password is required for a new student." },
                    { "PasswordTooShort", "Password must be at least 8 characters long!" },
                    { "PasswordNoSpecialChar", "Password must contain at least one special character!" },
                    { "Error", "Error: " },
                    { "ExportSuccess", "Exported to CSV successfully." }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        #region DataManager Class
        private class DataManager
        {
            private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            public void AddAccount(string studentId, string fullName, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO SYSTEM.account (ID, FULL_NAME, PASSWORD, ROLE) VALUES (@ID, @FullName, @Password, 'Student')";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", studentId);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public string UpdatePassword(string userId, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE SYSTEM.account SET PASSWORD = @Password WHERE ID = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", userId);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Success" : null;
                    }
                }
            }
        }
        #endregion
    }
}