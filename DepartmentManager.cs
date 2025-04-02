using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using SchoolManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class DepartmentManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly IDepartmentRepository _departmentRepository;
        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;

        public DepartmentManager(IDepartmentRepository departmentRepository)
        {
            InitializeComponent();
            _departmentRepository = departmentRepository;
            LoadDepartmentsAsync();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        #region Load Data Methods
        private async Task LoadDepartmentsAsync() 
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync();
                dgvDepartments.DataSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorLoading") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchDepartmentsDataAsync()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = "SELECT DEP_ID AS `Department ID`, DEP_NAME AS `Name` FROM SYSTEM.DEP ORDER BY DEP_ID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
                        return dataTable;
                    }
                }
            }
            finally
            {
                conn.Dispose();
            }
        }
        #endregion

        #region Event Handlers
        private void dgvDepartments_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                showAction();

                DataGridViewRow row = dgvDepartments.Rows[e.RowIndex];
                txtID.Text = row.Cells["Id"].Value.ToString(); // Id is string
                txtName.Text = row.Cells["Name"].Value.ToString();
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

        private async void pbSave_Click(object sender, EventArgs e)
        {
            await SaveDepartmentAsync();
        }

        private async void pbDelete_Click(object sender, EventArgs e)
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
                    string id = txtID.Text.Trim(); // Id is string, no conversion needed
                    await _departmentRepository.DeleteDepartmentAsync(id);
                    MessageBox.Show(GetLocalizedMessage("DeleteSuccess"));
                    await RefreshDataAsync();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451) // Foreign key constraint violation
                    {
                        MessageBox.Show(GetLocalizedMessage("CannotDeleteInUse"));
                    }
                    else
                    {
                        MessageBox.Show(GetLocalizedMessage("ErrorDeleting") + " " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("Error") + " " + ex.Message);
                }
                finally
                {
                    isSelected = false;
                }
            }
        }

        private async void pbReload_Click(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }

        private async void pictureBox2_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }

        private void DepartmentManager_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }
        #endregion

        #region Helper Methods
        private void showAction()
        {
            pbStudents.Visible = true;
            lbDepAdd.Visible = true;
            pbEdit.Visible = true;
            lbDepEdit.Visible = true;
            pbDelete.Visible = true;
            lbDeleteDep.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }

        private void SetAddMode()
        {
            pbStudents.Visible = false;
            lbDepAdd.Visible = false;
            pbEdit.Visible = false;
            lbDepEdit.Visible = false;
            pbDelete.Visible = false;
            lbDeleteDep.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Visible = false; // ID is auto-generated
            lbDepID.Visible = false;
            txtName.Text = "";
            txtName.Enabled = true;
        }

        private void SetEditMode()
        {
            pbStudents.Visible = false;
            lbDepAdd.Visible = false;
            pbEdit.Visible = false;
            lbDepEdit.Visible = false;
            pbDelete.Visible = false;
            lbDeleteDep.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Enabled = false;
            txtName.Enabled = true;
        }

        private async Task RefreshDataAsync()
        {
            await LoadDepartmentsAsync();
            showAction();
            ClearInputs();
            isSelected = false;
        }

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
        }

        private async Task SaveDepartmentAsync()
        {
            if (!ValidateInputs()) return;

            try
            {
                var department = new Department
                {
                    Name = txtName.Text.Trim()
                };

                if (action == 1) // Edit existing department
                {
                    department.Id = txtID.Text.Trim(); // Id is string
                    await _departmentRepository.UpdateDepartmentAsync(department);
                }
                else // Add new department
                {
                    await _departmentRepository.AddDepartmentAsync(department);
                }

                MessageBox.Show(GetLocalizedMessage(action == 0 ? "AddSuccess" : "EditSuccess"));
                await RefreshDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorSaving") + " " + ex.Message);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(GetLocalizedMessage("NameRequired"));
                return false;
            }
            return true;
        }

        private async Task ExportToCsvAsync()
        {
            SaveFileDialog saveFileDialog = null;
            DataTable dataTable = null;
            string filePath = null;

            try
            {
                saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    Title = "Save Departments as CSV",
                    FileName = $"Departments_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                filePath = saveFileDialog.FileName;

                dataTable = await FetchDepartmentsDataAsync();
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    throw new Exception("No data to export.");
                }

                StringBuilder csvContent = new StringBuilder();
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                    .Select(column => $"\"{column.ColumnName}\"")
                    .ToArray();
                csvContent.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field =>
                        $"\"{(field != null ? field.ToString().Replace("\"", "\"\"") : "")}\"")
                        .ToArray();
                    csvContent.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show(GetLocalizedMessage("ExportSuccess"));
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
            finally
            {
                if (dataTable != null)
                {
                    dataTable.Dispose();
                    dataTable = null;
                }
                if (saveFileDialog != null)
                {
                    saveFileDialog.Dispose();
                    saveFileDialog = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Veuillez choisir un département à éditer !" },
                    { "NoRecordToDelete", "Veuillez choisir un département à supprimer !" },
                    { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                    { "AddSuccess", "Ajout réussi." },
                    { "EditSuccess", "Édition réussie." },
                    { "DeleteSuccess", "Suppression réussie." },
                    { "CannotDeleteInUse", "Impossible de supprimer ce département car il est en cours d'utilisation." },
                    { "NameRequired", "Le nom du département est requis." },
                    { "ErrorLoading", "Erreur lors du chargement des départements : " },
                    { "ErrorSaving", "Erreur lors de l'enregistrement du département : " },
                    { "ErrorDeleting", "Erreur lors de la suppression du département : " },
                    { "ErrorExporting", "Erreur lors de l'exportation : " },
                    { "ExportSuccess", "Exportation réussie." }
                }
                : new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Please choose a department to edit!" },
                    { "NoRecordToDelete", "Please choose a department to delete!" },
                    { "ConfirmDelete", "Are you sure you want to delete?" },
                    { "AddSuccess", "Add successful." },
                    { "EditSuccess", "Edit successful." },
                    { "DeleteSuccess", "Delete successful." },
                    { "CannotDeleteInUse", "Cannot delete this department because it is in use." },
                    { "NameRequired", "Department name is required." },
                    { "ErrorLoading", "Error loading departments: " },
                    { "ErrorSaving", "Error saving department: " },
                    { "ErrorDeleting", "Error deleting department: " },
                    { "ErrorExporting", "Error exporting: " },
                    { "ExportSuccess", "Export successful." }
                };

            string message; // Declare variable beforehand
            if (messages.TryGetValue(messageKey, out message)) // Use it here
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        private void DepartmentManager_Load_1(object sender, EventArgs e)
        {
        }
    }
}