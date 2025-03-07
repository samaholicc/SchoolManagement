using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace SchoolManagement
{
    public partial class ClassSectionManager : KryptonForm
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
        public enum Language
        {
            English,
            French
        }

        public static Language CurrentLanguage = Language.French;

        #region Properties

        public static string ClassSectionID { get; set; }
        public static string SubjectID;
        public static int limited;

        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        #endregion

        #region Constructor

        public ClassSectionManager()
        {
            InitializeComponent();
            LoadClasses();
            LoadSubjects();
            LoadTeachers();
            ConfigureDateTimePickers();
            txtEndTime.Enabled = false;
            txtStartTime.Enabled = false;
        }

        #endregion

        #region Load Data Methods

        private void LoadTeachers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT TEACHER_ID, FULL_NAME FROM SYSTEM.TEACHER", conn);
                    cbTeacher.Items.Clear(); // Clear existing items
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string teacherId = reader.GetString(0);
                            string teacherName = reader.GetString(1);
                            // Use string concatenation instead of string interpolation
                            cbTeacher.Items.Add(teacherId + " - " + teacherName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void LoadSubjects()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT SUB_ID, SUB_NAME FROM SYSTEM.SUBJECT", conn);
                    cbSubject.Items.Clear();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int subjectId = reader.GetInt32(0);
                            string SubjectName = reader.GetString(1);
                            cbSubject.Items.Add(subjectId + " - " + SubjectName);
                        }
                    }
                }
            }
            catch (Exception es)
            {
                ShowMessage("An error occurred: " + es.Message, "Une erreur est survenue : " + es.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void LoadClasses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                        a.CLASS_ID AS 'CLASS SECTION ID',
                        CONCAT(a.Sub_ID, ' - ', s.SUB_NAME) AS `Subject`,
                        CONCAT(a.TEACHER_ID, ' - ', t.Full_name) AS `Teacher`,
                        a.START_DATE AS 'START',
                        a.FINISH_DATE AS 'FINISH',
                        a.SCHEDULE AS 'SCHEDULE',
                        a.NB_S AS 'N.O.S'
                    FROM SYSTEM.CLASS a
                    JOIN SYSTEM.SUBJECT s ON a.Sub_ID = s.SUB_ID
                    JOIN SYSTEM.TEACHER t ON a.TEACHER_ID = t.TEACHER_ID
                    ORDER BY a.CLASS_ID ASC
                    LIMIT @PageSize OFFSET @Offset;", conn);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Offset", (currFrom - 1) * pageSize);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        System.Data.DataTable dataTable = new System.Data.DataTable();
                        dataTable.Load(reader);
                        dgvClass.DataSource = dataTable;
                    }
                }
            }
            catch (Exception es)
            {
                ShowMessage("An error occurred: " + es.Message, "Une erreur est survenue : " + es.Message, "Error", MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];

                // Handle ID retrieval with conversion
                txtID.Text = row.Cells[0].Value.ToString();
                cbSubject.Text = row.Cells[1].Value.ToString();
                cbTeacher.Text = row.Cells[2].Value.ToString();

                // Handle date conversions (ensure the cells are not null)
                if (row.Cells[3].Value != null)
                    dtpStart.Value = Convert.ToDateTime(row.Cells[3].Value);

                if (row.Cells[4].Value != null)
                    dtpFinish.Value = Convert.ToDateTime(row.Cells[4].Value);

                // Retrieve the full schedule from the database
                string fullSchedule = row.Cells[5].Value != null ? row.Cells[5].Value.ToString() : string.Empty;
                txtSchedule.Text = fullSchedule;

                // Parse the schedule string to extract date, start time, and end time
                ParseSchedule(fullSchedule);

                // Handle the number of students
                txtNOS.Text = row.Cells[6].Value != null ? row.Cells[6].Value.ToString() : "0";

                // Correcting the class section and subject IDs correctly
                ClassSectionID = txtID.Text;
                SubjectID = cbSubject.Text;

                // If 'limited' is meant to be an integer, parse it appropriately
                int parsedValue;
                int nbS;
                if (int.TryParse(txtNOS.Text, out parsedValue))
                {
                    limited = parsedValue;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtNOS.Text) || !Int32.TryParse(txtNOS.Text, out nbS))
                    {
                        // Vérification de la culture locale et affichage du message correspondant
                        if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                        {
                            MessageBox.Show("Format du nombre d'élèves invalide !");
                        }
                        else // Culture par défaut pour l'anglais
                        {
                            MessageBox.Show("Invalid number of students format!");
                        }
                        return;
                    }
                }

                // Handle ComboBox selections
                if (row.Cells[1].Value != null)
                    cbSubject.SelectedItem = cbSubject.Items.Cast<string>()
                        .FirstOrDefault(i => i.StartsWith(row.Cells[1].Value.ToString().Split(' ')[0]));
                if (row.Cells[2].Value != null)
                    cbTeacher.SelectedItem = cbTeacher.Items.Cast<string>()
                        .FirstOrDefault(i => i.StartsWith(row.Cells[2].Value.ToString().Split(' ')[0]));
            }
        }

        #endregion

        #region Schedule Parsing Method

        private void ParseSchedule(string fullSchedule)
        {
            try
            {
                // Trim and validate input
                fullSchedule = fullSchedule != null ? fullSchedule.Trim() : null;
                if (string.IsNullOrWhiteSpace(fullSchedule))
                {
                    // Vérification de la culture locale et affichage du message correspondant
                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                    {
                        MessageBox.Show("Erreur : La chaîne de planning est vide.");
                    }
                    else // Culture par défaut pour l'anglais
                    {
                        MessageBox.Show("Error: Schedule string is empty.");
                    }
                }

                // Split into date-time and end time parts
                string[] scheduleParts = fullSchedule.Split(new string[] { " - " }, StringSplitOptions.None);
                if (scheduleParts.Length != 2)
                {
                    // Vérification de la culture locale et affichage du message correspondant
                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
                    {
                        MessageBox.Show("Erreur : format de planning invalide. Attendu 'Dimanche, 16 Mars 2025'.");
                    }
                    else // Culture par défaut pour l'anglais
                    {
                        MessageBox.Show("Error: Invalid schedule format. Expected 'Sunday, 16 March 2025'.");
                    }
                    return;
                }

                // Declare baseDate at a higher scope
                DateTime baseDate = DateTime.Today; // Use today as a base date for all time assignments

                // Extract date and start time from the first part
                string dateTimePart = scheduleParts[0].Trim(); // "Sunday, 16 March 2025 09:00"
                string endTimePart = scheduleParts[1].Trim();  // "10:00"
                MessageBox.Show("end :" + endTimePart);

                // Split dateTimePart into date and start time (last space separates them)
                int lastSpaceIndex = dateTimePart.LastIndexOf(' ');
                string datePart = dateTimePart.Substring(0, lastSpaceIndex).Trim(); // "Sunday, 16 March 2025"
                string startTimePart = dateTimePart.Substring(lastSpaceIndex + 1).Trim(); // "09:00"

                // Parse the date
                DateTime parsedDate;
                if (DateTime.TryParseExact(datePart, "dddd, dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    txtSchedule.Text = parsedDate.ToString("dddd, dd MMMM yyyy");
                    txtSchedule.Value = parsedDate;
                }

                // Parse the start time
                DateTime startDateTime;
                if (DateTime.TryParseExact(startTimePart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
                {
                    txtStartTime.Value = baseDate.Date + startDateTime.TimeOfDay; // Set start time (e.g., "09:00")
                }

                // Parse the end time
                DateTime endDateTime;
                if (DateTime.TryParseExact(endTimePart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
                {
                    txtEndTime.Value = baseDate.Date + endDateTime.TimeOfDay; // Set end time (e.g., "10:00")
                }

                // Ensure controls are enabled and refreshed
                txtStartTime.Enabled = false;
                txtEndTime.Enabled = false;
                txtStartTime.Refresh();
                txtEndTime.Refresh();
            }
            catch (Exception ex)
            {
                ShowMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Methods

       

        #endregion


       #region Event Handlers
private void pbStudents_Click(object sender, EventArgs e)
{
    action = 0; // Setting the action for adding
    ToggleUIForEditing(true); // Show the student addition UI
}

private void pbEdit_Click(object sender, EventArgs e)
{
    if (!isSelected)
    {
        string message = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr" 
            ? "Veuillez choisir une classe à modifier !" 
            : "Please choose class to edit!";
        
        MessageBox.Show(message);
        return;
    }

    action = 1;
    ToggleUIForEditing(true);
}

private void pbSave_Click(object sender, EventArgs e)
{
    try
    {
        ValidateDateTime(); // Validate the date and time before saving

        using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
        {
            conn.Open();
            MySqlCommand cmd;

            cmd = action == 0 ? new MySqlCommand("SP_CLASS_ADD", conn) : new MySqlCommand("SP_CLASS_UPDATE", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            string subId, teacherId;
            int nbS;

            // Validation subject
            if (string.IsNullOrWhiteSpace(cbSubject.Text) || !TryExtractId(cbSubject.Text, out subId))
            {
                ShowValidationMessage("subject", "Sélection invalide de matière. Veuillez choisir une matière valide.", "Invalid subject selection. Please choose a valid subject.");
                return;
            }

            // Validation teacher
            if (string.IsNullOrWhiteSpace(cbTeacher.Text) || !TryExtractId(cbTeacher.Text, out teacherId))
            {
                ShowValidationMessage("teacher", "Sélection invalide de l'enseignant. Veuillez choisir un enseignant valide.", "Invalid teacher selection. Please choose a valid teacher.");
                return;
            }

            // Validation number of students
            if (!Int32.TryParse(txtNOS.Text, out nbS))
            {
                ShowValidationMessage("students", "Nombre d'élèves invalide. Veuillez entrer un nombre valide.", "Invalid number of students. Please enter a valid number.");
                return;
            }

            // Validation start date
            if (dtpStart.Value == DateTime.MinValue)
            {
                ShowValidationMessage("start date", "Veuillez entrer une date de début valide.", "Please enter a valid start date.");
                return;
            }

            // Concatenate schedule details
            string scheduleDetails = $"{txtSchedule.Value:dddd, dd MMMM yyyy} {txtStartTime.Value:HH:mm} - {txtEndTime.Value:HH:mm}";

            cmd.Parameters.AddWithValue("p_SUB_ID", subId);
            cmd.Parameters.AddWithValue("p_TEACHER_ID", teacherId);
            cmd.Parameters.AddWithValue("p_START_DATE", dtpStart.Value);
            cmd.Parameters.AddWithValue("p_FINISH_DATE", dtpFinish.Value);
            cmd.Parameters.AddWithValue("p_SCHEDULE", scheduleDetails);
            cmd.Parameters.AddWithValue("p_NB_S", nbS);
            cmd.Parameters.AddWithValue("p_CLASS_ID", txtID.Text);

            cmd.ExecuteNonQuery();

            string successMessage = action == 0 ? "Classe ajoutée avec succès !" : "Classe mise à jour avec succès !";
            string successMessageEnglish = action == 0 ? "Class added successfully!" : "Class updated successfully!";
            MessageBox.Show(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr" ? successMessage : successMessageEnglish);
        }
    }
    catch (Exception ex)
    {
        ShowMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message, "Error", MessageBoxIcon.Error);
    }

    RefreshClassList();
}

private void lbDelete_Click(object sender, EventArgs e)
{
    if (!isSelected)
    {
        string message = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr" 
            ? "Veuillez choisir une classe à supprimer !" 
            : "Please choose a class to delete!";

        MessageBox.Show(message);
        return;
    }

    DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

    if (dialogResult == DialogResult.Yes)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM SYSTEM.CLASS WHERE CLASS_ID=@ClassID", conn);
                cmd.Parameters.AddWithValue("@ClassID", txtID.Text); // Use the textual value
                cmd.ExecuteNonQuery();
            }
            Refesh();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}

private void pbPrev_Click(object sender, EventArgs e)
{
    if (currFrom > 1)
    {
        currFrom--;
        LoadClasses();
    }
}
#endregion

#region Helper Methods
private bool TryExtractId(string text, out string id)
{
    id = string.Empty;
    if (text.Contains("-"))
    {
        id = text.Split('-')[0].Trim();
        return true;
    }
    return false;
}

private void ShowValidationMessage(string type, string messageFr, string messageEn)
{
    string message = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr" ? messageFr : messageEn;
    MessageBox.Show(message);
}

private void ShowMessage(string messageEn, string messageFr, string title, MessageBoxIcon icon)
{
    if (CurrentLanguage == Language.English)
    {
        MessageBox.Show(messageEn, title, MessageBoxButtons.OK, icon);
    }
    else
    {
        MessageBox.Show(messageFr, title, MessageBoxButtons.OK, icon);
    }
}
#endregion

#region Excel Export
private void ExportToExcel()
{
    try
    {
        Excel.Application excelApp = new Excel.Application();
        excelApp.Visible = true;
        Excel.Workbook workbook = excelApp.Workbooks.Add();
        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);

        // Add column headers to the Excel file
        for (int col = 0; col < dgvClass.Columns.Count; col++)
        {
            worksheet.Cells[1, col + 1] = dgvClass.Columns[col].HeaderText;
        }

        List<DataRow> allRows = new List<DataRow>();
        int totalRecords = GetTotalRecordCount();
        int totalPages = (totalRecords + pageSize - 1) / pageSize;

        for (int page = 1; page <= totalPages; page++)
        {
            currFrom = page;
            LoadClasses();

            foreach (DataGridViewRow row in dgvClass.Rows)
            {
                if (row.IsNewRow) continue;

                DataRow dataRow = ((DataTable)dgvClass.DataSource).NewRow();
                for (int col = 0; col < dgvClass.Columns.Count; col++)
                {
                    dataRow[col] = row.Cells[col].Value.ToString();
                }

                allRows.Add(dataRow);
            }
        }

        int rowIndex = 2;
        foreach (var row in allRows)
        {
            for (int col = 0; col < dgvClass.Columns.Count; col++)
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

    LoadClasses();
}
#endregion


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
    

private void pbDetail_Click(object sender, EventArgs e)
    {
        if (!isSelected)
        {
            MessageBox.Show(GetLocalizedMessage("PleaseChooseClass", "Veuillez choisir une classe à afficher!"));
            return;
        }

        StudentsInClassSection studentsInClassSection = new StudentsInClassSection(ClassSectionID);
        studentsInClassSection.ShowDialog();
    }

    private void ValidateDateTime()
    {
        // Vérification de la date dans txtSchedule  
        if (txtSchedule.Value.Date < DateTime.Now.Date)
        {
            MessageBox.Show(GetLocalizedMessage(
                "InvalidDate",
                "La date sélectionnée est invalide. La valeur sera réinitialisée."));
            txtSchedule.Value = DateTime.Now; // Réinitialiser à la date actuelle si la valeur est invalide  
        }

        // Vérification de l'heure de début : ne pas permettre un début avant 8:00 AM  
        if (txtStartTime.Value.TimeOfDay < TimeSpan.FromHours(8))
        {
            MessageBox.Show(GetLocalizedMessage(
                "InvalidStartTime",
                "L'heure de début ne peut pas être avant 8:00 AM. Elle sera réinitialisée à 8:00 AM."));

            // Réinitialiser la valeur de txtStartTime à 8:00 AM  
            DateTime validStartTime = txtStartTime.Value.Date.Add(TimeSpan.FromHours(8));

            txtStartTime.Value = validStartTime;

            // Log de débogage pour vérifier la valeur mise à jour  
            Console.WriteLine($"Updated txtStartTime.Value to: {txtStartTime.Value:HH:mm}");
        }

        // Vérification de l'heure de fin : ne pas permettre un horaire après 18:00  
        if (txtEndTime.Value.TimeOfDay > TimeSpan.FromHours(18))
        {
            MessageBox.Show(GetLocalizedMessage(
                "InvalidEndTime",
                "L'heure de fin ne peut pas être après 18:00. Elle sera réinitialisée à 18:00."));

            DateTime validEndTime = txtEndTime.Value.Date.Add(TimeSpan.FromHours(18)); // Combine date and 6:00 PM  
            txtEndTime.Value = validEndTime;
        }

        // Vérification de l'heure de fin : l'heure de fin ne doit pas être avant l'heure de début  
        if (txtEndTime.Value <= txtStartTime.Value)
        {
            MessageBox.Show(GetLocalizedMessage(
                "EndTimeBeforeStartTime",
                "L'heure de fin ne peut pas être avant l'heure de début. Veuillez ajuster l'heure de fin."));

            // Ajustez l'heure de fin en ajoutant des minutes  
            DateTime newEndTime = txtStartTime.Value.AddMinutes(60); // Ajoutez 60 minutes à l'heure de début

            // Assurez-vous que la nouvelle heure de fin est dans une plage valide (avant 18:00)
            if (newEndTime.TimeOfDay > TimeSpan.FromHours(18))
            {
                newEndTime = txtSchedule.Value.Date.Add(TimeSpan.FromHours(18)); // Réinitialiser à 18:00  
            }

            txtEndTime.Value = newEndTime;  // Appliquer la nouvelle heure de fin valide  
        }
    }

    // Helper function to return the localized message
    private string GetLocalizedMessage(string englishMessage, string frenchMessage)
    {
        if (CultureInfo.CurrentCulture.Name == "fr-FR")
        {
            return frenchMessage;  // Return the French message
        }
        else
        {
            return englishMessage;  // Return the English message
        }
    }


    // Méthode pour récupérer les horaires de début et de fin
    private void GetScheduledTimes()
        {
            DateTime selectedDate = txtSchedule.Value.Date; // Récupère seulement la date
            TimeSpan startTimeSpan = txtStartTime.Value.TimeOfDay; // Heure de début
            TimeSpan endTimeSpan = txtEndTime.Value.TimeOfDay; // Heure de fin

            // Combinez la date avec les heures
            DateTime finalStart = selectedDate.Add(startTimeSpan);
            DateTime finalEnd = selectedDate.Add(endTimeSpan);

           
        }
        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadClasses();
        }

        private void label7_Click(object sender, EventArgs e)
        {

            ExportToExcel();

        }
        private void pbReload_Click(object sender, EventArgs e)
        {
            Refesh();
        }
        private void Refesh()
        {
            LoadClasses();

            // Reset UI elements
            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.Text = "";
            cbTeacher.Text = "";
            txtSchedule.Text = "";
            txtNOS.Text = "";
            txtEndTime.Text = "";
            txtStartTime.Text = "";

            // Reset control states
            cbSubject.Enabled = false;
            cbTeacher.Enabled = false;
            dtpStart.Enabled = false;
            dtpFinish.Enabled = false;
            txtSchedule.Enabled = false;
            txtNOS.Enabled = false;
            txtEndTime.Enabled = false;
            txtStartTime.Enabled = false;

            // Show relevant buttons and labels
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

            isSelected = false; // Reset selection status
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

            // Clear input fields
            txtSearch.Text = "";


            // Enable or disable controls based on mode
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
            // Configuration for txtSchedule (Date Picker)
            txtSchedule.CustomFormat = "dddd, dd MMMM yyyy";  // Full Day, Day, Month, Year
            txtSchedule.Format = DateTimePickerFormat.Custom;
            txtSchedule.MinDate = DateTime.Today;  // Prevent selecting past dates

            // Configuration for txtStartTime (Start Time Picker)
            txtStartTime.CustomFormat = "HH:mm";  // Format for start time
            txtStartTime.Format = DateTimePickerFormat.Custom;
            txtStartTime.ShowUpDown = true; // Show up/down arrows for time selection
            dtpStart.MinDate = DateTime.Today;  // Prevent selecting past dates (today)
            dtpFinish.MinDate = DateTime.Today;
            // Configuration for txtEndTime (End Time Picker)
            txtEndTime.CustomFormat = "HH:mm";  // Format for end time
            txtEndTime.Format = DateTimePickerFormat.Custom;
            txtEndTime.ShowUpDown = true; // Show up/down arrows for time selection

        }

        private void RefreshClassList()
        {
            // Logic to reload or refresh the list/grid showing classes
            LoadClasses();
            ClearInputs();
            ToggleUIForEditing(false);
        }

        private void ClearInputs()
        {
            // Clear all input fields after saving
            txtID.Clear();
            txtNOS.Clear();
            cbSubject.SelectedIndex = -1;
            cbTeacher.SelectedIndex = -1;
            dtpStart.Value = DateTime.Now;
            dtpFinish.Value = DateTime.Now;
        }

        private void ClassSectionManager_Load(object sender, EventArgs e)
        {

        }
    }
}
