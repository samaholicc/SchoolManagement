using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;

namespace SchoolManagement
{
    public partial class TeacherManager : KryptonForm
    {
        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT DEP_ID, DEP_NAME FROM SYSTEM.dep", conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbDepartment.Items.Clear();
                        while (reader.Read())
                        {
                            cbDepartment.Items.Add($"{reader.GetString(0)} - {reader.GetString(1)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorLoadingDepartments"), ex.Message));
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
                    FROM SYSTEM.teacher a  
                    JOIN SYSTEM.dep d ON a.DEP_ID = d.DEP_ID
                    LIMIT @PageSize OFFSET @Offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.AddWithValue("@Offset", (currFrom - 1) * pageSize);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvTeachers.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorLoadingTeachers"), ex.Message));
            }
        }

        #endregion

        #region Button Click Events

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("SelectTeacherToEdit"));
                return;
            }
            action = 1;
            SetEditMode();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            SaveTeacher();
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("SelectTeacherToDelete"));
                return;
            }

            if (MessageBox.Show(GetLocalizedMessage("ConfirmDelete"), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteTeacherQuery = "DELETE FROM SYSTEM.teacher WHERE TEACHER_ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteTeacherQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }

                        string deleteAccountQuery = "DELETE FROM SYSTEM.account WHERE ID = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteAccountQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show(GetLocalizedMessage("TeacherDeleted"));
                    LoadTeachers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(GetLocalizedMessage("ErrorDeletingTeacher"), ex.Message));
                }
                finally
                {
                    isSelected = false;
                    showAction();
                }
            }
        }

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

        private void pbTeachers_Click(object sender, EventArgs e)
        {
            action = 0;
            SetAddMode();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SearchTeachers();
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }

        #endregion

        #region Save / Update Teacher
        private void SaveTeacher()
        {
            try
            {
                if (!ValidateInputs()) return;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string depId = cbDepartment.SelectedItem?.ToString().Split('-')[0].Trim();

                    if (action == 0) // Add new teacher
                    {
                        // Generate a unique identifier
                        string uniqueIdentifier = DateTime.Now.Ticks.ToString() + "_" + new Random().Next(1000, 9999).ToString();

                        // Log the unique identifier for debugging
                        MessageBox.Show(string.Format(GetLocalizedMessage("InsertingTeacher"), txtName.Text + " " + uniqueIdentifier));

                        // Insert into teacher table with unique identifier in FULL_NAME
                        string insertTeacherQuery = @"
                        INSERT INTO SYSTEM.teacher (FULL_NAME, ADRESS, GENDER, DATE_OF_BIRTH, DEP_ID) 
                        VALUES (@fullname, @adress, @gender, @dateofbirth, @depId)";

                        using (var cmd = new MySqlCommand(insertTeacherQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text + " " + uniqueIdentifier);
                            cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                            cmd.Parameters.AddWithValue("@depId", depId);
                            cmd.ExecuteNonQuery();
                        }

                        // Retrieve the TEACHER_ID using the unique identifier
                        string newTeacherId;
                        string selectTeacherIdQuery = @"
                        SELECT TEACHER_ID 
                        FROM SYSTEM.teacher 
                        WHERE TRIM(FULL_NAME) = @fullname 
                        ORDER BY TEACHER_ID DESC LIMIT 1";

                        using (var cmd = new MySqlCommand(selectTeacherIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text + " " + uniqueIdentifier);
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                // Fallback: Generate T### in application
                                string fallbackQuery = @"
                                SELECT COALESCE(MAX(CAST(SUBSTRING(TEACHER_ID, 2) AS UNSIGNED)), 0) + 1 
                                FROM SYSTEM.teacher 
                                WHERE TEACHER_ID REGEXP '^T[0-9]{3}$'";
                                using (var fallbackCmd = new MySqlCommand(fallbackQuery, conn))
                                {
                                    object maxIdResult = fallbackCmd.ExecuteScalar();
                                    int nextId = maxIdResult != null ? Convert.ToInt32(maxIdResult) : 1;
                                    newTeacherId = $"T{nextId.ToString("D3")}";

                                    // Update the inserted record with the generated TEACHER_ID
                                    string updateIdQuery = @"
                                    UPDATE SYSTEM.teacher 
                                    SET TEACHER_ID = @teacherId 
                                    WHERE TRIM(FULL_NAME) = @fullname";
                                    using (var updateCmd = new MySqlCommand(updateIdQuery, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@teacherId", newTeacherId);
                                        updateCmd.Parameters.AddWithValue("@fullname", txtName.Text + " " + uniqueIdentifier);
                                        int rowsAffected = updateCmd.ExecuteNonQuery();
                                        if (rowsAffected == 0)
                                        {
                                            throw new Exception(string.Format(GetLocalizedMessage("FailedToUpdateTeacherId"), txtName.Text + " " + uniqueIdentifier));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                newTeacherId = result.ToString();
                            }

                            if (string.IsNullOrEmpty(newTeacherId) || !newTeacherId.StartsWith("T") || newTeacherId.Length != 4)
                            {
                                throw new Exception(string.Format(GetLocalizedMessage("InvalidTeacherId"), newTeacherId ?? "null"));
                            }

                            // Log the retrieved or generated TEACHER_ID for debugging
                            MessageBox.Show(string.Format(GetLocalizedMessage("RetrievedTeacherId"), newTeacherId));
                        }

                        // Update FULL_NAME to remove the unique identifier
                        string updateTeacherQuery = @"
                        UPDATE SYSTEM.teacher 
                        SET FULL_NAME = @fullname 
                        WHERE TEACHER_ID = @teacherId";
                        using (var cmd = new MySqlCommand(updateTeacherQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            cmd.Parameters.AddWithValue("@teacherId", newTeacherId);
                            cmd.ExecuteNonQuery();
                        }

                        // Insert into account table using the retrieved TEACHER_ID
                        string hashedPassword = Encrypt.HashString(txtPassword.Text);
                        string accountQuery = "INSERT INTO SYSTEM.account (ID, FULL_NAME, PASSWORD, ROLE) VALUES (@id, @fullname, @password, @role)";
                        using (var accountCmd = new MySqlCommand(accountQuery, conn))
                        {
                            accountCmd.Parameters.AddWithValue("@id", newTeacherId);
                            accountCmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            accountCmd.Parameters.AddWithValue("@password", hashedPassword);
                            accountCmd.Parameters.AddWithValue("@role", "Teacher");
                            accountCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(GetLocalizedMessage("TeacherAdded"));
                    }
                    else // Update existing teacher
                    {
                        string teacherId = txtID.Text;
                        string query = @"
                        UPDATE SYSTEM.teacher 
                        SET FULL_NAME = @fullname, ADRESS = @adress, GENDER = @gender, 
                            DATE_OF_BIRTH = @dateofbirth, DEP_ID = @depId 
                        WHERE TEACHER_ID = @id";

                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                            cmd.Parameters.AddWithValue("@depId", depId);
                            cmd.Parameters.AddWithValue("@id", teacherId);
                            cmd.ExecuteNonQuery();
                        }

                        // Update password if changed
                        string newPassword = txtPassword.Text.Trim();
                        if (!string.IsNullOrEmpty(newPassword) && IsPasswordValid(newPassword))
                        {
                            string hashedPassword = Encrypt.HashString(newPassword);
                            string updatePasswordQuery = "INSERT INTO SYSTEM.account (ID, FULL_NAME, PASSWORD, ROLE) VALUES (@id, @fullname, @password, @role) ON DUPLICATE KEY UPDATE PASSWORD = @password";
                            using (MySqlCommand updateCmd = new MySqlCommand(updatePasswordQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@id", teacherId);
                                updateCmd.Parameters.AddWithValue("@fullname", txtName.Text);
                                updateCmd.Parameters.AddWithValue("@password", hashedPassword);
                                updateCmd.Parameters.AddWithValue("@role", "Teacher");
                                updateCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show(GetLocalizedMessage("PasswordUpdated"));
                        }

                        // Update FULL_NAME in account table
                        string updateAccountQuery = "UPDATE SYSTEM.account SET FULL_NAME = @FullName WHERE ID = @ID";
                        using (MySqlCommand updateAccountCmd = new MySqlCommand(updateAccountQuery, conn))
                        {
                            updateAccountCmd.Parameters.AddWithValue("@FullName", txtName.Text);
                            updateAccountCmd.Parameters.AddWithValue("@ID", teacherId);
                            updateAccountCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(GetLocalizedMessage("TeacherUpdated"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorSavingTeacher"), ex.Message));
            }
            finally
            {
                ClearInputs();
                showAction();
                LoadTeachers();
            }
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
            pbSave.Visible = true;
            lbSave.Visible = true;
            txtID.Enabled = false;
            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbDepartment.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void SetAddMode()
        {
            pbTeachers.Visible = false;
            lbAddTeacher.Visible = false;
            pbEdit.Visible = false;
            lbEditTeacher.Visible = false;
            pbDelete.Visible = false;
            lbDeleteTeacher.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;
            ClearInputs();
            txtID.Visible = false;
            lbTeacherID.Visible = false;
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
                txtID.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[2].Value.ToString();
                txtAddress.Text = row.Cells[5].Value.ToString();
                dtpBirth.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                cbDepartment.Text = row.Cells[1].Value.ToString();
                rbMale.Checked = row.Cells[4].Value.ToString() == "Homme";
                rbFemale.Checked = !rbMale.Checked;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(GetLocalizedMessage("NameRequired"));
                return false;
            }
            if (cbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show(GetLocalizedMessage("DepartmentRequired"));
                return false;
            }
            if (action == 0 && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(GetLocalizedMessage("PasswordRequired"));
                return false;
            }
            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                MessageBox.Show(GetLocalizedMessage("PasswordLength"));
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                MessageBox.Show(GetLocalizedMessage("PasswordSpecialChar"));
                return false;
            }
            return true;
        }

        private void SearchTeachers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    SELECT TEACHER_ID AS `Teacher ID`, DEP_ID AS `Dep`, FULL_NAME AS `Name`, 
                           DATE_OF_BIRTH AS `Birth`, GENDER AS `Gender`, ADRESS AS `Address`
                    FROM SYSTEM.teacher 
                    WHERE FULL_NAME LIKE @search OR TEACHER_ID LIKE @search 
                          OR DEP_ID LIKE @search OR GENDER LIKE @search OR ADRESS LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{txtSearch.Text}%");
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvTeachers.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorSearchingTeachers"), ex.Message));
            }
        }

        private void ResetForm()
        {
            currFrom = 1;
            LoadTeachers();
            showAction();
            txtSearch.Text = "";
            ClearInputs();
        }

        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = GetLocalizedMessage("SaveTeachersCsv");
                saveFileDialog.FileName = "Teachers_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchTeacherDataAsync();

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();

                        // Write headers
                        string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                            .Select(column => $"\"{column.ColumnName}\"")
                            .ToArray();
                        csvContent.AppendLine(string.Join(",", columnNames));

                        // Write data
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string[] fields = row.ItemArray.Select(field =>
                                $"\"{(field != null ? field.ToString().Replace("\"", "\"\"") : "")}\"")
                                .ToArray();
                            csvContent.AppendLine(string.Join(",", fields));
                        }

                        // Write to file
                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                        MessageBox.Show(GetLocalizedMessage("ExportedCsv"));
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show(GetLocalizedMessage("NoDataToExport"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorExportingCsv"), ex.Message));
            }
        }

        private async Task<DataTable> FetchTeacherDataAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"
                SELECT 
                    a.TEACHER_ID AS `Teacher ID`, 
                    CONCAT(a.DEP_ID, ' - ', d.DEP_NAME) AS `Department`,  
                    a.FULL_NAME AS `Name`,  
                    a.DATE_OF_BIRTH AS `Birth`, 
                    a.GENDER AS `Gender`, 
                    a.ADRESS AS `Address`
                FROM SYSTEM.teacher a  
                JOIN SYSTEM.dep d ON a.DEP_ID = d.DEP_ID";

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

        private int GetTotalRecordCount()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(TEACHER_ID) FROM SYSTEM.teacher";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(GetLocalizedMessage("ErrorGettingRecordCount"), ex.Message));
                return 0;
            }
        }

        #endregion

        #region Localization

        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                { "ErrorLoadingDepartments", "Erreur lors du chargement des départements : {0}" },
                { "ErrorLoadingTeachers", "Erreur lors du chargement des enseignants : {0}" },
                { "SelectTeacherToEdit", "Veuillez sélectionner un enseignant à modifier !" },
                { "SelectTeacherToDelete", "Veuillez sélectionner un enseignant à supprimer !" },
                { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                { "TeacherDeleted", "Enseignant supprimé avec succès." },
                { "ErrorDeletingTeacher", "Erreur lors de la suppression de l'enseignant : {0}" },
                { "NameRequired", "Le nom est requis." },
                { "DepartmentRequired", "Veuillez sélectionner un département." },
                { "PasswordRequired", "Le mot de passe est requis pour les nouveaux enseignants." },
                { "PasswordLength", "Le mot de passe doit contenir au moins 8 caractères !" },
                { "PasswordSpecialChar", "Le mot de passe doit contenir au moins un caractère spécial !" },
                { "ErrorSearchingTeachers", "Erreur lors de la recherche des enseignants : {0}" },
                { "ErrorExportingCsv", "Erreur lors de l'exportation en CSV : {0}" },
                { "NoDataToExport", "Aucune donnée à exporter." },
                { "ExportedCsv", "Exporté en CSV avec succès." },
                { "SaveTeachersCsv", "Enregistrer les enseignants en CSV" },
                { "ErrorGettingRecordCount", "Erreur lors de l'obtention du nombre d'enregistrements : {0}" },
                { "ErrorSavingTeacher", "Erreur lors de l'enregistrement de l'enseignant : {0}" },
                { "TeacherAdded", "Enseignant ajouté avec succès." },
                { "TeacherUpdated", "Enseignant mis à jour avec succès." },
                { "PasswordUpdated", "Mot de passe mis à jour avec succès." },
                { "InsertingTeacher", "Insertion de l'enseignant avec FULL_NAME : {0}" },
                { "RetrievedTeacherId", "TEACHER_ID récupéré : {0}" },
                { "FailedToUpdateTeacherId", "Échec de la mise à jour de TEACHER_ID pour FULL_NAME : {0}" },
                { "InvalidTeacherId", "Format de TEACHER_ID invalide récupéré : {0}" }
                }
                : new Dictionary<string, string>
                {
                { "ErrorLoadingDepartments", "Error loading departments: {0}" },
                { "ErrorLoadingTeachers", "Error loading teachers: {0}" },
                { "SelectTeacherToEdit", "Please select a teacher to edit!" },
                { "SelectTeacherToDelete", "Please select a teacher to delete!" },
                { "ConfirmDelete", "Are you sure you want to delete?" },
                { "TeacherDeleted", "Teacher deleted successfully." },
                { "ErrorDeletingTeacher", "Error deleting teacher: {0}" },
                { "NameRequired", "Name is required." },
                { "DepartmentRequired", "Please select a department." },
                { "PasswordRequired", "Password is required for new teachers." },
                { "PasswordLength", "Password must be at least 8 characters long!" },
                { "PasswordSpecialChar", "Password must contain at least one special character!" },
                { "ErrorSearchingTeachers", "Error searching teachers: {0}" },
                { "ErrorExportingCsv", "Error exporting to CSV: {0}" },
                { "NoDataToExport", "No data to export." },
                { "ExportedCsv", "Exported to CSV successfully." },
                { "SaveTeachersCsv", "Save Teachers as CSV" },
                { "ErrorGettingRecordCount", "Error getting record count: {0}" },
                { "ErrorSavingTeacher", "Error saving teacher: {0}" },
                { "TeacherAdded", "Teacher added successfully." },
                { "TeacherUpdated", "Teacher updated successfully." },
                { "PasswordUpdated", "Password updated successfully." },
                { "InsertingTeacher", "Inserting teacher with FULL_NAME: {0}" },
                { "RetrievedTeacherId", "Retrieved TEACHER_ID: {0}" },
                { "FailedToUpdateTeacherId", "Failed to update TEACHER_ID for FULL_NAME: {0}" },
                { "InvalidTeacherId", "Invalid TEACHER_ID format retrieved: {0}" }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }

        #endregion

        // Inline DataManager class to avoid conflicts
        public void AddAccount(string id, string fullName, string hashedPassword, string role)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO SYSTEM.account (ID, FULL_NAME, PASSWORD, ROLE) VALUES (@id, @fullname, @password, @role)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fullname", fullName);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void TeacherManager_Load(object sender, EventArgs e)
        {
        }
    }

}