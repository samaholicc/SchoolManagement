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
                MessageBox.Show($"Error loading departments: {ex.Message}");
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
                MessageBox.Show($"Error loading teachers: {ex.Message}");
            }
        }

        #endregion

        #region Button Click Events

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please select a teacher to edit!");
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
                MessageBox.Show("Please select a teacher to delete!");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    MessageBox.Show("Teacher deleted successfully.");
                    LoadTeachers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting teacher: {ex.Message}");
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
                        string query = @"
                            INSERT INTO SYSTEM.teacher (FULL_NAME, ADRESS, GENDER, DATE_OF_BIRTH, DEP_ID) 
                            VALUES (@fullname, @adress, @gender, @dateofbirth, @depId); 
                            SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                            cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                            cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                            cmd.Parameters.AddWithValue("@depId", depId);

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                string newTeacherId = result.ToString();
                                DataManager dataManager = new DataManager();
                                dataManager.AddAccount(newTeacherId, Encrypt.HashString(txtPassword.Text));
                                MessageBox.Show("Teacher added successfully.");
                            }
                        }
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

                        string newPassword = txtPassword.Text.Trim();
                        if (!string.IsNullOrEmpty(newPassword) && IsPasswordValid(newPassword))
                        {
                            DataManager dataManager = new DataManager();
                            string hashedPassword = Encrypt.HashString(newPassword);
                            dataManager.UpdatePassword(teacherId, hashedPassword);
                            MessageBox.Show("Password updated successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving teacher: {ex.Message}");
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
                MessageBox.Show("Name is required.");
                return false;
            }
            if (cbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a department.");
                return false;
            }
            if (action == 0 && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new teachers.");
                return false;
            }
            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long!");
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                MessageBox.Show("Password must contain at least one special character!");
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
                MessageBox.Show($"Error searching teachers: {ex.Message}");
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
                saveFileDialog.Title = "Save Teachers as CSV";
                saveFileDialog.FileName = "Teachers_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchTeacherDataAsync();

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

                        MessageBox.Show("Exported to CSV successfully.");
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
                MessageBox.Show($"Error exporting to CSV: {ex.Message}");
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
                MessageBox.Show($"Error getting record count: {ex.Message}");
                return 0;
            }
        }

        #endregion

        // Inline DataManager class to avoid conflicts
        private class DataManager
        {
            private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            public void AddAccount(string teacherId, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO SYSTEM.account (ID, FULL_NAME, PASSWORD, ROLE) " +
                                   "SELECT TEACHER_ID, FULL_NAME, @password, 'Teacher' " +
                                   "FROM SYSTEM.teacher WHERE TEACHER_ID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", teacherId);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public string UpdatePassword(string userId, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE SYSTEM.account SET PASSWORD = @password WHERE ID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Success" : null;
                    }
                }
            }

            public bool VerifyPassword(string userId, string hashedPassword)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PASSWORD FROM SYSTEM.account WHERE ID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        string storedHash = (string)cmd.ExecuteScalar();
                        return storedHash == hashedPassword;
                    }
                }
            }
        }

        private void TeacherManager_Load(object sender, EventArgs e)
        {

        }
    }
}