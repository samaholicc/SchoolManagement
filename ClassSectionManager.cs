using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
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
        private readonly ClassSectionService _classSectionService;
        private int _currentPage = 1;
        private const int PageSize = 10;
        private bool _isSelected = false;
        private int _action = 0; // 0 = Add, 1 = Edit
        public static string ClassSectionID { get; private set; }
        public static int StudentLimit { get; private set; }

        public ClassSectionManager(ClassSectionService classSectionService)
        {
            InitializeComponent();
            _classSectionService = classSectionService;
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
            dgvClass.CellMouseClick += DgvClass_CellMouseClick;
            ConfigureDataGridViewColumns();
        }

        private void ConfigureDataGridViewColumns()
        {
            dgvClass.AutoGenerateColumns = false;
            dgvClass.Columns.Clear();

            // Add columns manually
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClassId",
                DataPropertyName = "ClassId",
                HeaderText = GetLocalizedMessage("ClassIdHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subject",
                DataPropertyName = "SubjectDisplay",
                HeaderText = GetLocalizedMessage("SubjectHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Teacher",
                DataPropertyName = "TeacherDisplay",
                HeaderText = GetLocalizedMessage("TeacherHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartDate",
                DataPropertyName = "StartDate",
                HeaderText = GetLocalizedMessage("StartDateHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FinishDate",
                DataPropertyName = "FinishDate",
                HeaderText = GetLocalizedMessage("FinishDateHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Schedule",
                DataPropertyName = "Schedule",
                HeaderText = GetLocalizedMessage("ScheduleHeader")
            });
            dgvClass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NumberOfStudents",
                DataPropertyName = "NumberOfStudents",
                HeaderText = GetLocalizedMessage("NumberOfStudentsHeader")
            });
        }

        private async void LoadInitialDataAsync()
        {
            try
            {
                await Task.WhenAll(
                    LoadClassesAsync(),
                    LoadSubjectsAsync(),
                    LoadTeachersAsync()
                );
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorLoadingInitialData"));
            }
        }
        #endregion

        #region Data Loading
        private async Task LoadClassesAsync()
        {
            try
            {
                var classes = await _classSectionService.LoadClassesAsync(_currentPage, PageSize);
                dgvClass.DataSource = null;
                dgvClass.DataSource = classes;
                if (classes.Count == 0)
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

        private async Task LoadTeachersAsync()
        {
            try
            {
                cbTeacher.Items.Clear();
                var teachers = await _classSectionService.LoadTeachersAsync();
                foreach (var teacher in teachers)
                {
                    cbTeacher.Items.Add($"{teacher.Id} - {teacher.FullName}");
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
                var subjects = await _classSectionService.LoadSubjectsAsync();
                foreach (var subject in subjects)
                {
                    cbSubject.Items.Add($"{subject.Id} - {subject.Name}");
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
        #endregion

        #region Event Handlers
        private void DgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                int studentLimit;
                if (row.Cells["NumberOfStudents"].Value != null && int.TryParse(row.Cells["NumberOfStudents"].Value.ToString(), out studentLimit))
                {
                    StudentLimit = studentLimit;
                }
                else
                {
                    throw new Exception("Invalid student limit value in the selected row.");
                }

                ClassSectionID = row.Cells["ClassId"].Value?.ToString();
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
                    await _classSectionService.DeleteClassAsync(txtID.Text);
                    MessageBox.Show(GetLocalizedMessage("DeleteSuccess"));
                    RefreshClassList();
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
            if (!_isSelected || string.IsNullOrEmpty(ClassSectionID))
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
                var filteredClasses = await _classSectionService.SearchClassesAsync(searchTerm);
                dgvClass.DataSource = null;
                dgvClass.DataSource = filteredClasses;
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(ex, GetLocalizedMessage("ErrorSearching"));
            }
        }

        private void ClassSectionManager_Load(object sender, EventArgs e) { }
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
                    return;
                }

                bool success = await _classSectionService.SaveClassAsync(classSection, _action == 0);
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
            int subId;
            if (!TryExtractId(cbSubject.Text, out subId))
            {
                MessageBox.Show(GetLocalizedMessage("InvalidSubject"));
                return null;
            }

            string teacherId;
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
                txtID.Text = row.Cells["ClassId"].Value?.ToString() ?? "";
                var classSection = row.DataBoundItem as ClassSection;
                if (classSection != null)
                {
                    cbSubject.Text = classSection.SubjectDisplay;
                    cbTeacher.Text = classSection.TeacherDisplay;
                }

                DateTime startDate;
                if (row.Cells["StartDate"].Value != null && DateTime.TryParse(row.Cells["StartDate"].Value.ToString(), out startDate))
                    dtpStart.Value = startDate;
                else
                    dtpStart.Value = DateTime.Now;

                DateTime finishDate;
                if (row.Cells["FinishDate"].Value != null && DateTime.TryParse(row.Cells["FinishDate"].Value.ToString(), out finishDate))
                    dtpFinish.Value = finishDate;
                else
                    dtpFinish.Value = DateTime.Now;

                txtNOS.Text = row.Cells["NumberOfStudents"].Value?.ToString() ?? "0";
                ParseSchedule(row.Cells["Schedule"].Value?.ToString() ?? "");
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
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    Title = "Save Class Sections as CSV",
                    FileName = $"ClassSections_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var classes = await _classSectionService.GetAllClassesAsync();
                    if (classes != null && classes.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();
                        csvContent.AppendLine("\"ClassId\",\"Subject\",\"Teacher\",\"StartDate\",\"FinishDate\",\"Schedule\",\"NumberOfStudents\"");

                        foreach (var cls in classes)
                        {
                            string[] fields = new[]
                            {
                                $"\"{cls.ClassId}\"",
                                $"\"{cls.SubjectDisplay}\"",
                                $"\"{cls.TeacherDisplay}\"",
                                $"\"{cls.StartDate:yyyy-MM-dd}\"",
                                $"\"{cls.FinishDate:yyyy-MM-dd}\"",
                                $"\"{cls.Schedule}\"",
                                $"\"{cls.NumberOfStudents}\""
                            };
                            csvContent.AppendLine(string.Join(",", fields));
                        }

                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                        MessageBox.Show(GetLocalizedMessage("ExportSuccess"));
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
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

        private bool TryExtractId(string text, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(text) || !text.Contains("-")) return false;
            string idStr = text.Split('-')[0].Trim();
            return int.TryParse(idStr, out id);
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
                    { "NoSubjectsFound", "Aucune matière trouvée dans la base de données." },
                    // Add header messages for DataGridView columns
                    { "ClassIdHeader", "Identifiant de la classe" },
                    { "SubjectHeader", "Matière" },
                    { "TeacherHeader", "Professeur" },
                    { "StartDateHeader", "Date de début" },
                    { "FinishDateHeader", "Date de fin" },
                    { "ScheduleHeader", "Horaire" },
                    { "NumberOfStudentsHeader", "Nombre d'étudiants" }
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
                    { "NoSubjectsFound", "No subjects found in the database." },
                    // Add header messages for DataGridView columns
                    { "ClassIdHeader", "Class ID" },
                    { "SubjectHeader", "Subject" },
                    { "TeacherHeader", "Teacher" },
                    { "StartDateHeader", "Start Date" },
                    { "FinishDateHeader", "Finish Date" },
                    { "ScheduleHeader", "Schedule" },
                    { "NumberOfStudentsHeader", "Number of Students" }
                };

            string message;
            return messages.TryGetValue(messageKey, out message) ? message : "Unknown error";
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

            txtID.Enabled = false;
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
            txtStartTime.Enabled = false;

            txtEndTime.CustomFormat = "HH:mm";
            txtEndTime.Format = DateTimePickerFormat.Custom;
            txtEndTime.ShowUpDown = true;
            txtEndTime.Enabled = false;

            dtpStart.MinDate = DateTime.Today;
            dtpFinish.MinDate = DateTime.Today;
        }

        private void UpdatePaginationButtons()
        {
            pbPrev.Enabled = _currentPage > 1;
            pbNext.Enabled = dgvClass.Rows.Count == PageSize;
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