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
    public partial class SubjectManager : KryptonForm
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
        private int pageSize = 20;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public SubjectManager()
        {
            InitializeComponent();
            this.Load += async (s, e) => await LoadSubjectsAsync();
        }

        #region Load Data Methods

        private async Task LoadSubjectsAsync()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                try
                {
                    await conn.OpenAsync();
                    string query = @"
                        SELECT SUB_ID AS `Subject ID`, SUB_NAME AS `Name`, CREDITS AS `Credits`
                        FROM SUBJECT
                        ORDER BY SUB_ID ASC
                        LIMIT @limit OFFSET @offset";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        try
                        {
                            DataTable dataTable = new DataTable();
                            await Task.Run(() => adapter.Fill(dataTable));
                            dgvStudents.DataSource = dataTable;
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
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        #endregion

        #region Button Click Events

        private async void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                await LoadSubjectsAsync();
            }
        }

        private async void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            await LoadSubjectsAsync();
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
                MessageBox.Show(GetLocalizedErrorMessage("NoRecordToEdit"));
                return;
            }
            action = 1;
            SetEditMode();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            SaveSubjectAsync();
        }

        private async void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedErrorMessage("NoRecordToDelete"));
                return;
            }

            if (MessageBox.Show(GetLocalizedErrorMessage("ConfirmDelete"), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(connectionString);
                    try
                    {
                        await conn.OpenAsync();
                        MySqlCommand cmd = new MySqlCommand("SP_SUBJECT_DELETE", conn);
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("p_SUB_ID", txtID.Text);
                            await cmd.ExecuteNonQueryAsync();
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
                    MessageBox.Show(GetLocalizedErrorMessage("DeleteSuccess"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
                }
                finally
                {
                    isSelected = false;
                    await RefreshFormAsync();
                }
            }
        }

        private async void pbReload_Click(object sender, EventArgs e)
        {
            await RefreshFormAsync();
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                try
                {
                    await conn.OpenAsync();
                    string query = @"
                        SELECT SUB_ID AS `Subject ID`, SUB_NAME AS `Name`, CREDITS AS `Credits`
                        FROM SUBJECT
                        WHERE SUB_NAME LIKE @search OR SUB_ID LIKE @search
                        ORDER BY SUB_ID ASC
                        LIMIT @limit OFFSET @offset";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        try
                        {
                            DataTable dataTable = new DataTable();
                            await Task.Run(() => adapter.Fill(dataTable));
                            dgvStudents.DataSource = dataTable;
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
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        #endregion

        #region Helper Methods

        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                showAction();

                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                txtID.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                txtAddress.Text = row.Cells[2].Value.ToString();
            }
        }

        private void showAction()
        {
            pbStudents.Visible = true;
            lbSubjAdd.Visible = true;
            pbEdit.Visible = true;
            lbSubjEdit.Visible = true;
            pbDelete.Visible = true;
            lbSubjDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }

        private void SetAddMode()
        {
            pbStudents.Visible = false;
            lbSubjAdd.Visible = false;
            pbEdit.Visible = false;
            lbSubjEdit.Visible = false;
            pbDelete.Visible = false;
            lbSubjDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Text = "";
            txtID.Visible = true;
            lbSubjID.Visible = true;
            txtID.Enabled = true;

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;
        }

        private void SetEditMode()
        {
            pbStudents.Visible = false;
            lbSubjAdd.Visible = false;
            pbEdit.Visible = false;
            lbSubjEdit.Visible = false;
            pbDelete.Visible = false;
            lbSubjDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Enabled = false;
            txtName.Enabled = true;
            txtAddress.Enabled = true;
        }

        private async Task RefreshFormAsync()
        {
            isSelected = false;
            currFrom = 1;
            showAction();
            await LoadSubjectsAsync();
            txtSearch.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
        }

        private async void SaveSubjectAsync()
        {
            if (!ValidateInputs()) return;

            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                try
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd;

                    if (action == 0) // Add new subject
                    {
                        cmd = new MySqlCommand("SP_SUBJECT_ADD", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_SUB_NAME", txtName.Text);
                        cmd.Parameters.AddWithValue("p_CREDITS", int.Parse(txtAddress.Text));
                    }
                    else // Edit existing subject
                    {
                        cmd = new MySqlCommand("SP_SUBJECT_UPDATE", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_SUB_ID", txtID.Text);
                        cmd.Parameters.AddWithValue("p_SUB_NAME", txtName.Text);
                        cmd.Parameters.AddWithValue("p_CREDITS", int.Parse(txtAddress.Text));
                    }

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                    MessageBox.Show(GetLocalizedErrorMessage("SaveSuccess"));
                }
                finally
                {
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
            finally
            {
                await RefreshFormAsync();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(GetLocalizedErrorMessage("NameRequired"));
                return false;
            }
            int credits;
            if (string.IsNullOrWhiteSpace(txtAddress.Text) ||
                !int.TryParse(txtAddress.Text, out credits) ||
                credits < 0)
            {
                MessageBox.Show(GetLocalizedErrorMessage("InvalidCredits"));
                return false;
            }
            return true;
        }

        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Subjects as CSV";
                saveFileDialog.FileName = "Subjects_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchDataFromDatabaseAsync();

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

                        MessageBox.Show(GetLocalizedErrorMessage("Exports"));
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
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchDataFromDatabaseAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();

                const string query = @"
                    SELECT SUB_ID AS `Subject ID`, 
                           SUB_NAME AS `Name`, 
                           CREDITS AS `Credits`
                    FROM SUBJECT 
                    ORDER BY SUB_ID ASC";

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

        private string GetLocalizedErrorMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            Dictionary<string, string> messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Aucune matière trouvée à éditer." },
                    { "NoRecordToDelete", "Aucun enregistrement trouvé à supprimer." },
                    { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                    { "SaveSuccess", "Enregistrement réussi." },
                    { "DeleteSuccess", "Suppression réussie." },
                    { "Error", "Erreur : " },
                    { "Exports", "Export réussi." },
                    { "NameRequired", "Le nom de la matière est requis." },
                    { "InvalidCredits", "Les crédits doivent être un nombre positif." }
                }
                : new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "No selected subject found to edit." },
                    { "NoRecordToDelete", "No matching record found to delete." },
                    { "ConfirmDelete", "Are you sure you want to delete?" },
                    { "SaveSuccess", "Save successful." },
                    { "DeleteSuccess", "Deleted successfully." },
                    { "Error", "Error: " },
                    { "Exports", "Exported successfully." },
                    { "NameRequired", "Subject name is required." },
                    { "InvalidCredits", "Credits must be a positive number." }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }

        #endregion
    }
}