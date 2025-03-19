using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class ClassSectionManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private readonly ClassService _classService;
        private int _currentPage = 1;
        private const int PageSize = 10;
        private bool _isSelected = false;
        private int _action = 0; // 0 = Add, 1 = Edit
        public static string ClassSectionID { get; private set; } // Public static property for selected class ID
        public static int StudentLimit { get; private set; } // Public static property for student limit

        public ClassSectionManager()
        {
            InitializeComponent();
            _classService = new ClassService(connectionString);
            InitializeUI();
            LoadInitialDataAsync();
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

        #region Initialization
        private void InitializeUI()
        {
            ConfigureDateTimePickers();
            ToggleUIForEditing(false);
            UpdatePaginationButtons();

            // Ensure the CellMouseClick event is wired up
            dgvClass.CellMouseClick += DgvClass_CellMouseClick; // Verify in designer if already set
        }

        private async void LoadInitialDataAsync()
        {
            try
            {
                // Load data independently to ensure all are attempted even if one fails
                await Task.WhenAll(
                    LoadClassesAsync().ContinueWith(new Action<Task>(t => LogError(t.Exception, "Error loading classes"))),
                    LoadSubjectsAsync().ContinueWith(new Action<Task>(t => LogError(t.Exception, "Error loading subjects"))),
                    LoadTeachersAsync().ContinueWith(new Action<Task>(t => LogError(t.Exception, "Error loading teachers")))
                );
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorLoadingInitialData"));
            }
        }

        private void LogError(Exception ex, string context)
        {
            if (ex != null)
            {
                ErrorHandler.ShowError(ex, context);
            }
        }
        #endregion

        #region Data Loading
        private async Task LoadTeachersAsync()
        {
            try
            {
                cbTeacher.Items.Clear();
                var teachers = await _classService.GetTeachersAsync();
                foreach (var teacher in teachers)
                {
                    cbTeacher.Items.Add(teacher.Id + " - " + teacher.FullName);
                }
                if (cbTeacher.Items.Count == 0)
                {
                    MessageBox.Show(GetLocalizedMessage("NoTeachersFound"));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorLoadingTeachers"));
            }
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                cbSubject.Items.Clear();
                var subjects = await _classService.GetSubjectsAsync();
                foreach (var subject in subjects)
                {
                    cbSubject.Items.Add(subject.Id + " - " + subject.Name);
                }
                if (cbSubject.Items.Count == 0)
                {
                    MessageBox.Show(GetLocalizedMessage("NoSubjectsFound"));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorLoadingSubjects"));
            }
        }

        private async Task LoadClassesAsync()
        {
            try
            {
                var classes = await _classService.GetClassesAsync(_currentPage, PageSize);
                dgvClass.DataSource = null; // Clear existing data
                dgvClass.DataSource = classes;
                if (classes.Rows.Count == 0)
                {
                    MessageBox.Show(GetLocalizedMessage("NoClassesFound"));
                }
                UpdatePaginationButtons();
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorLoadingClasses"));
            }
        }
        #endregion

        #region Event Handlers
        private void DgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                // Explicitly declare studentLimit for C# 6.0 compatibility
                int studentLimit;
                if (row.Cells["N.O.S"].Value != null && int.TryParse(row.Cells["N.O.S"].Value.ToString(), out studentLimit))
                {
                    StudentLimit = studentLimit;
                }
                else
                {
                    throw new Exception("Invalid student limit value in the selected row.");
                }

                ClassSectionID = row.Cells["CLASS SECTION ID"].Value != null ? row.Cells["CLASS SECTION ID"].Value.ToString() : null;
                if (string.IsNullOrEmpty(ClassSectionID))
                {
                    throw new Exception("Class Section ID is not set in the selected row.");
                }

                _isSelected = true;
                PopulateFormFromRow(row);
                showAction();
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorSelectingClass"));
                _isSelected = false;
                ClassSectionID = null;
                StudentLimit = 0;
            }
        }

        private async void PbSave_Click(object sender, EventArgs e)
        {
            await SaveClassAsync();
        }

        private void PbEdit_Click(object sender, EventArgs e)
        {
            if (!_isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToEdit"));
                return;
            }
            _action = 1;
            ToggleUIForEditing(true);
        }

        private void PbStudents_Click(object sender, EventArgs e)
        {
            _action = 0;
            ClearInputs();
            ToggleUIForEditing(true);
        }

        private async void PbNext_Click(object sender, EventArgs e)
        {
            _currentPage++;
            await LoadClassesAsync();
        }

        private async void PbPrev_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                await LoadClassesAsync();
            }
        }

        private async void PbDelete_Click(object sender, EventArgs e)
        {
            if (!_isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToDelete"));
                return;
            }

            if (MessageBox.Show(GetLocalizedMessage("ConfirmDelete"), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await _classService.DeleteClassAsync(txtID.Text);
                    MessageBox.Show(GetLocalizedMessage("DeleteSuccess"));
                    RefreshClassList();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451) // Foreign key constraint violation
                    {
                        MessageBox.Show(GetLocalizedMessage("CannotDeleteInUse"));
                    }
                    else
                    {
                        ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorDeleting"));
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorDeleting"));
                }
            }
        }

        private async void Label7_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }

        private void PbDetail_Click(object sender, EventArgs e)
        {
            if (!_isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToShow"));
                return;
            }

            if (string.IsNullOrEmpty(ClassSectionID))
            {
                MessageBox.Show(GetLocalizedMessage("NoRecordToShow"));
                return;
            }

            using (var studentsForm = new StudentsInClass(ClassSectionID))
            {
                studentsForm.ShowDialog();
            }
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show(GetLocalizedMessage("NoSearchTerm"));
                return;
            }

            try
            {
                var filteredClasses = await _classService.GetClassesBySearchTermAsync(searchTerm);
                dgvClass.DataSource = null; // Clear existing data
                dgvClass.DataSource = filteredClasses;
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorSearching"));
            }
        }

        private void ClassSectionManager_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }
        #endregion

        #region Business Logic
        private async Task SaveClassAsync()
        {
            try
            {
                ValidateDateTime();
                ClassSection classSection = BuildClassSectionFromForm();
                if (classSection == null)
                {
                    return; // Validation failed, error message already shown
                }

                bool success = _action == 0
                    ? await _classService.AddClassAsync(classSection)
                    : await _classService.UpdateClassAsync(classSection);

                if (success)
                {
                    MessageBox.Show(GetLocalizedMessage(_action == 0 ? "AddSuccess" : "EditSuccess"));
                    RefreshClassList();
                }
                else
                {
                    MessageBox.Show(GetLocalizedMessage("SaveFailed"));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorSaving"));
            }
        }

        private ClassSection BuildClassSectionFromForm()
        {
            // Explicitly declare variables for C# 6.0 compatibility
            string subId = string.Empty;
            if (!TryExtractId(cbSubject.Text, out subId))
            {
                MessageBox.Show(GetLocalizedMessage("InvalidSubject"));
                return null;
            }

            string teacherId = string.Empty;
            if (!TryExtractId(cbTeacher.Text, out teacherId))
            {
                MessageBox.Show(GetLocalizedMessage("InvalidTeacher"));
                return null;
            }

            int numberOfStudents;
            if (!int.TryParse(txtNOS.Text, out numberOfStudents) || numberOfStudents < 0)
            {
                MessageBox.Show(GetLocalizedMessage("InvalidNOS"));
                return null;
            }

            // Replace string interpolation with string.Format for C# 6.0 compatibility
            string schedule = string.Format("{0:dddd, dd MMMM yyyy} {1:HH:mm} - {2:HH:mm}", txtSchedule.Value, txtStartTime.Value, txtEndTime.Value);

            return new ClassSection
            {
                ClassId = txtID.Text,
                SubjectId = subId,
                TeacherId = teacherId,
                StartDate = dtpStart.Value,
                FinishDate = dtpFinish.Value,
                Schedule = schedule,
                NumberOfStudents = numberOfStudents
            };
        }

        private void PopulateFormFromRow(DataGridViewRow row)
        {
            try
            {
                txtID.Text = row.Cells["CLASS SECTION ID"].Value != null ? row.Cells["CLASS SECTION ID"].Value.ToString() : "";
                cbSubject.Text = row.Cells["Subject"].Value != null ? row.Cells["Subject"].Value.ToString() : "";
                cbTeacher.Text = row.Cells["Teacher"].Value != null ? row.Cells["Teacher"].Value.ToString() : "";

                DateTime startDate;
                if (row.Cells["START"].Value != null && DateTime.TryParse(row.Cells["START"].Value.ToString(), out startDate))
                    dtpStart.Value = startDate;
                else
                    dtpStart.Value = DateTime.Now;

                DateTime finishDate;
                if (row.Cells["FINISH"].Value != null && DateTime.TryParse(row.Cells["FINISH"].Value.ToString(), out finishDate))
                    dtpFinish.Value = finishDate;
                else
                    dtpFinish.Value = DateTime.Now;

                txtNOS.Text = row.Cells["N.O.S"].Value != null ? row.Cells["N.O.S"].Value.ToString() : "0";
                ParseSchedule(row.Cells["SCHEDULE"].Value != null ? row.Cells["SCHEDULE"].Value.ToString() : "");
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorParsingRow"));
            }
        }

        private void ParseSchedule(string fullSchedule)
        {
            if (string.IsNullOrWhiteSpace(fullSchedule))
            {
                txtSchedule.Value = DateTime.Now;
                txtStartTime.Value = DateTime.Now;
                txtEndTime.Value = DateTime.Now;
                return;
            }

            var parts = fullSchedule.Split(new[] { " - " }, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                txtSchedule.Value = DateTime.Now;
                txtStartTime.Value = DateTime.Now;
                txtEndTime.Value = DateTime.Now;
                return;
            }

            string dateTimePart = parts[0].Trim();
            string endTimePart = parts[1].Trim();

            int lastSpaceIndex = dateTimePart.LastIndexOf(' ');
            if (lastSpaceIndex == -1)
            {
                txtSchedule.Value = DateTime.Now;
                txtStartTime.Value = DateTime.Now;
                txtEndTime.Value = DateTime.Now;
                return;
            }

            string datePart = dateTimePart.Substring(0, lastSpaceIndex).Trim();
            string startTimePart = dateTimePart.Substring(lastSpaceIndex + 1).Trim();

            DateTime parsedDate;
            if (DateTime.TryParseExact(datePart, "dddd, dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                txtSchedule.Value = parsedDate;
            else
                txtSchedule.Value = DateTime.Now;

            DateTime startTime;
            if (DateTime.TryParseExact(startTimePart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                txtStartTime.Value = DateTime.Today.Add(startTime.TimeOfDay);
            else
                txtStartTime.Value = DateTime.Now;

            DateTime endTime;
            if (DateTime.TryParseExact(endTimePart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                txtEndTime.Value = DateTime.Today.Add(endTime.TimeOfDay);
            else
                txtEndTime.Value = DateTime.Now;
        }

        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Class Sections as CSV";
                saveFileDialog.FileName = "ClassSections_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await _classService.GetAllClassesAsync();

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();

                        // Écrire les en-têtes (replace LINQ with loop for C# 6.0 compatibility)
                        string[] columnNames = new string[dataTable.Columns.Count];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            columnNames[i] = "\"" + dataTable.Columns[i].ColumnName + "\"";
                        }
                        csvContent.AppendLine(string.Join(",", columnNames));

                        // Écrire les données (replace LINQ with loop for C# 6.0 compatibility)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string[] fields = new string[row.ItemArray.Length];
                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                fields[i] = "\"" + (row[i] != null ? row[i].ToString().Replace("\"", "\"\"") : "") + "\"";
                            }
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
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorExporting"));
            }
        }
        #endregion

        #region Helper Methods
        private void showAction()
        {
            pbStudents.Visible = true;
            lbAddClass.Visible = true;
            pbEdit.Visible = true;
            lbEditClass.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
            pbDetail.Visible = true;
            lbShowStudents.Visible = true;
        }

        private void ValidateDateTime()
        {
            if (txtSchedule.Value.Date < DateTime.Now.Date)
                txtSchedule.Value = DateTime.Now;

            if (txtStartTime.Value.TimeOfDay < TimeSpan.FromHours(8))
                txtStartTime.Value = txtStartTime.Value.Date.AddHours(8);

            if (txtEndTime.Value.TimeOfDay > TimeSpan.FromHours(18))
                txtEndTime.Value = txtEndTime.Value.Date.AddHours(18);

            if (txtEndTime.Value <= txtStartTime.Value)
                txtEndTime.Value = txtStartTime.Value.AddHours(1);
        }

        private bool TryExtractId(string text, out string id)
        {
            id = string.Empty;
            if (string.IsNullOrWhiteSpace(text) || !text.Contains("-")) return false;
            id = text.Split('-')[0].Trim();
            return true;
        }

        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Veuillez choisir une classe à modifier !" },
                    { "NoRecordToDelete", "Veuillez choisir une classe à supprimer !" },
                    { "NoRecordToShow", "Veuillez choisir une classe à afficher !" },
                    { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                    { "AddSuccess", "Classe ajoutée avec succès !" },
                    { "EditSuccess", "Classe mise à jour avec succès !" },
                    { "DeleteSuccess", "Suppression réussie." },
                    { "CannotDeleteInUse", "Impossible de supprimer cette classe car elle est en cours d'utilisation." },
                    { "SaveFailed", "Échec de l'enregistrement de la classe." },
                    { "InvalidSubject", "Sélection invalide de matière." },
                    { "InvalidTeacher", "Sélection invalide de l'enseignant." },
                    { "InvalidNOS", "Nombre d'élèves invalide." },
                    { "NoSearchTerm", "Veuillez entrer un terme de recherche !" },
                    { "ErrorLoadingInitialData", "Erreur lors du chargement des données initiales." },
                    { "ErrorLoadingTeachers", "Erreur lors du chargement des enseignants." },
                    { "ErrorLoadingSubjects", "Erreur lors du chargement des matières." },
                    { "ErrorLoadingClasses", "Erreur lors du chargement des classes." },
                    { "ErrorSaving", "Erreur lors de l'enregistrement de la classe." },
                    { "ErrorDeleting", "Erreur lors de la suppression de la classe." },
                    { "ErrorSearching", "Erreur lors de la recherche des classes." },
                    { "ErrorExporting", "Erreur lors de l'exportation vers CSV." },
                    { "ExportSuccess", "Exporté vers CSV avec succès." },
                    { "ErrorSelectingClass", "Erreur lors de la sélection de la classe." },
                    { "ErrorParsingRow", "Erreur lors de l'analyse des données de la ligne." },
                    { "NoClassesFound", "Aucune classe trouvée dans la base de données." },
                    { "NoTeachersFound", "Aucun enseignant trouvé dans la base de données." },
                    { "NoSubjectsFound", "Aucune matière trouvée dans la base de données." }
                }
                : new Dictionary<string, string>
                {
                    { "NoRecordToEdit", "Please choose a class to edit!" },
                    { "NoRecordToDelete", "Please choose a class to delete!" },
                    { "NoRecordToShow", "Please choose a class to show!" },
                    { "ConfirmDelete", "Are you sure you want to delete?" },
                    { "AddSuccess", "Class added successfully!" },
                    { "EditSuccess", "Class updated successfully!" },
                    { "DeleteSuccess", "Delete successful." },
                    { "CannotDeleteInUse", "Cannot delete this class because it is in use." },
                    { "SaveFailed", "Failed to save the class." },
                    { "InvalidSubject", "Invalid subject selection." },
                    { "InvalidTeacher", "Invalid teacher selection." },
                    { "InvalidNOS", "Invalid number of students." },
                    { "NoSearchTerm", "Please enter a search term!" },
                    { "ErrorLoadingInitialData", "Error loading initial data." },
                    { "ErrorLoadingTeachers", "Error loading teachers." },
                    { "ErrorLoadingSubjects", "Error loading subjects." },
                    { "ErrorLoadingClasses", "Error loading classes." },
                    { "ErrorSaving", "Error saving class." },
                    { "ErrorDeleting", "Error deleting class." },
                    { "ErrorSearching", "Error searching classes." },
                    { "ErrorExporting", "Error exporting to CSV." },
                    { "ExportSuccess", "Exported to CSV successfully." },
                    { "ErrorSelectingClass", "Error selecting class." },
                    { "ErrorParsingRow", "Error parsing row data." },
                    { "NoClassesFound", "No classes found in the database." },
                    { "NoTeachersFound", "No teachers found in the database." },
                    { "NoSubjectsFound", "No subjects found in the database." }
                };

            string message = string.Empty;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }

        private void RefreshClassList()
        {
            LoadClassesAsync();
            ClearInputs();
            ToggleUIForEditing(false);
            _isSelected = false;
        }

        private void ClearInputs()
        {
            txtID.Clear();
            txtNOS.Clear();
            cbSubject.SelectedIndex = -1;
            cbTeacher.SelectedIndex = -1;
            dtpStart.Value = DateTime.Now;
            dtpFinish.Value = DateTime.Now;
            txtSchedule.Value = DateTime.Now;
            txtStartTime.Value = DateTime.Now;
            txtEndTime.Value = DateTime.Now;
        }

        private void ToggleUIForEditing(bool isEditingMode)
        {
            pbStudents.Visible = !isEditingMode;
            lbAddClass.Visible = !isEditingMode;
            pbEdit.Visible = !isEditingMode;
            lbEditClass.Visible = !isEditingMode;
            pbDelete.Visible = !isEditingMode;
            lbDelete.Visible = !isEditingMode;
            pbSave.Visible = isEditingMode;
            lbSave.Visible = isEditingMode;
            pbDetail.Visible = !isEditingMode;
            lbShowStudents.Visible = !isEditingMode;

            txtID.Enabled = false; // Always read-only (auto-generated)
            cbSubject.Enabled = isEditingMode;
            cbTeacher.Enabled = isEditingMode;
            dtpStart.Enabled = isEditingMode;
            dtpFinish.Enabled = isEditingMode;
            txtSchedule.Enabled = isEditingMode;
            txtNOS.Enabled = isEditingMode;
            txtEndTime.Enabled = isEditingMode;
            txtStartTime.Enabled = isEditingMode;
        }

        private void ConfigureDateTimePickers()
        {
            txtSchedule.CustomFormat = "dddd, dd MMMM yyyy";
            txtSchedule.Format = DateTimePickerFormat.Custom;
            txtSchedule.MinDate = DateTime.Today;

            txtStartTime.CustomFormat = "HH:mm";
            txtStartTime.Format = DateTimePickerFormat.Custom;
            txtStartTime.ShowUpDown = true;
            txtStartTime.Enabled = false; // Ensure consistency with ToggleUIForEditing

            txtEndTime.CustomFormat = "HH:mm";
            txtEndTime.Format = DateTimePickerFormat.Custom;
            txtEndTime.ShowUpDown = true;
            txtEndTime.Enabled = false; // Ensure consistency with ToggleUIForEditing

            dtpStart.MinDate = DateTime.Today;
            dtpFinish.MinDate = DateTime.Today;
        }

        private void UpdatePaginationButtons()
        {
            pbPrev.Enabled = _currentPage > 1;
            pbNext.Enabled = dgvClass.Rows.Count == PageSize; // Assumes full page means more data exists
        }
        #endregion

        #region Service Layer
        public class ClassService
        {
            private readonly string _connectionString;

            public ClassService(string connectionString)
            {
                _connectionString = connectionString;
            }

            public async Task<List<Teacher>> GetTeachersAsync()
            {
                var teachers = new List<Teacher>();
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand("SELECT TEACHER_ID, FULL_NAME FROM SYSTEM.teacher ORDER BY FULL_NAME", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            teachers.Add(new Teacher { Id = reader.GetString(0), FullName = reader.GetString(1) });
                        }
                    }
                }
                return teachers;
            }

            public async Task<List<Subject>> GetSubjectsAsync()
            {
                var subjects = new List<Subject>();
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand("SELECT SUB_ID, SUB_NAME FROM SYSTEM.subject ORDER BY SUB_NAME", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            subjects.Add(new Subject { Id = reader.GetInt32(0).ToString(), Name = reader.GetString(1) });
                        }
                    }
                }
                return subjects;
            }

            public async Task<DataTable> GetClassesAsync(int page, int pageSize)
            {
                var dataTable = new DataTable();
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand(
                        "SELECT " +
                            "a.CLASS_ID AS 'CLASS SECTION ID', " +
                            "CONCAT(a.SUB_ID, ' - ', s.SUB_NAME) AS 'Subject', " +
                            "CONCAT(a.TEACHER_ID, ' - ', t.FULL_NAME) AS 'Teacher', " +
                            "a.START_DATE AS 'START', " +
                            "a.FINISH_DATE AS 'FINISH', " +
                            "a.SCHEDULE AS 'SCHEDULE', " +
                            "a.NB_S AS 'N.O.S' " +
                        "FROM SYSTEM.class a " +
                        "LEFT JOIN SYSTEM.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN SYSTEM.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "ORDER BY a.CLASS_ID ASC " +
                        "LIMIT @PageSize OFFSET @Offset", conn);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
                return dataTable;
            }

            public async Task<DataTable> GetAllClassesAsync()
            {
                var dataTable = new DataTable();
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand(
                        "SELECT " +
                            "a.CLASS_ID AS 'CLASS SECTION ID', " +
                            "CONCAT(a.SUB_ID, ' - ', s.SUB_NAME) AS 'Subject', " +
                            "CONCAT(a.TEACHER_ID, ' - ', t.FULL_NAME) AS 'Teacher', " +
                            "a.START_DATE AS 'START', " +
                            "a.FINISH_DATE AS 'FINISH', " +
                            "a.SCHEDULE AS 'SCHEDULE', " +
                            "a.NB_S AS 'N.O.S' " +
                        "FROM SYSTEM.class a " +
                        "LEFT JOIN SYSTEM.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN SYSTEM.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "ORDER BY a.CLASS_ID ASC", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
                return dataTable;
            }

            public async Task<DataTable> GetClassesBySearchTermAsync(string searchTerm)
            {
                var dataTable = new DataTable();
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand(
                        "SELECT " +
                            "a.CLASS_ID AS 'CLASS SECTION ID', " +
                            "CONCAT(a.SUB_ID, ' - ', s.SUB_NAME) AS 'Subject', " +
                            "CONCAT(a.TEACHER_ID, ' - ', t.FULL_NAME) AS 'Teacher', " +
                            "a.START_DATE AS 'START', " +
                            "a.FINISH_DATE AS 'FINISH', " +
                            "a.SCHEDULE AS 'SCHEDULE', " +
                            "a.NB_S AS 'N.O.S' " +
                        "FROM SYSTEM.class a " +
                        "LEFT JOIN SYSTEM.subject s ON a.SUB_ID = s.SUB_ID " +
                        "LEFT JOIN SYSTEM.teacher t ON a.TEACHER_ID = t.TEACHER_ID " +
                        "WHERE a.CLASS_ID LIKE @SearchTerm " +
                           "OR s.SUB_NAME LIKE @SearchTerm " +
                           "OR t.FULL_NAME LIKE @SearchTerm " +
                           "OR a.SCHEDULE LIKE @SearchTerm " +
                        "ORDER BY a.CLASS_ID ASC", conn);
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
                return dataTable;
            }

            public async Task<bool> AddClassAsync(ClassSection classSection)
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand("SP_CLASS_ADD", conn) { CommandType = CommandType.StoredProcedure };
                    AddClassParameters(cmd, classSection);
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }

            public async Task<bool> UpdateClassAsync(ClassSection classSection)
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand("SP_CLASS_UPDATE", conn) { CommandType = CommandType.StoredProcedure };
                    AddClassParameters(cmd, classSection);
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }

            public async Task DeleteClassAsync(string classId)
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand("DELETE FROM SYSTEM.class WHERE CLASS_ID = @ClassID", conn);
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            private void AddClassParameters(MySqlCommand cmd, ClassSection classSection)
            {
                cmd.Parameters.AddWithValue("p_SUB_ID", classSection.SubjectId);
                cmd.Parameters.AddWithValue("p_TEACHER_ID", classSection.TeacherId);
                cmd.Parameters.AddWithValue("p_START_DATE", classSection.StartDate);
                cmd.Parameters.AddWithValue("p_FINISH_DATE", classSection.FinishDate);
                cmd.Parameters.AddWithValue("p_SCHEDULE", classSection.Schedule);
                cmd.Parameters.AddWithValue("p_NB_S", classSection.NumberOfStudents);
                cmd.Parameters.AddWithValue("p_CLASS_ID", classSection.ClassId);
            }
        }
        #endregion

        #region Models
        public class ClassSection
        {
            public string ClassId { get; set; }
            public string SubjectId { get; set; }
            public string TeacherId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime FinishDate { get; set; }
            public string Schedule { get; set; }
            public int NumberOfStudents { get; set; }
        }

        public class Teacher
        {
            public string Id { get; set; }
            public string FullName { get; set; }
        }

        public class Subject
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        #endregion

        #region Error Handling
        public static class ErrorHandler
        {
            public static void ShowError(Exception ex, string context)
            {
                MessageBox.Show("An error occurred " + context + ": " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}