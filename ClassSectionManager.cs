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
        private void ShowMessage(string messageEnglish, string messageFrench, string caption, MessageBoxIcon icon)
        {
            if (CurrentLanguage == Language.English)
            {
                MessageBox.Show(messageEnglish, caption, MessageBoxButtons.OK, icon);
            }
            else if (CurrentLanguage == Language.French)
            {
                MessageBox.Show(messageFrench, caption, MessageBoxButtons.OK, icon);
            }
        }

        public static string ClassSectionID { get; set; }  


        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

  

        

        public static string SubjectID;
        public static int limited;

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
                MessageBox.Show("An error occurred: " + ex.Message);
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
                MessageBox.Show("Error loading subjects: " + es.Message);
            }
        }


        private void LoadClasses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                    SELECT 
                        a.CLASS_ID AS 'CLASS SECTION ID', 
                        CONCAT(a.Sub_ID, ' - ', s.SUB_NAME) AS `Subject`, 
                        CONCAT(a.TEACHER_ID, ' - ', t.Full_name) AS `Teacher`, 
                        a.START_DATE AS 'START', 
                        a.FINISH_DATE AS 'FINISH', 
                        a.SCHEDULE AS 'SCHEDULE', 
                        a.NB_S AS 'N.O.S' 
                    FROM 
                        SYSTEM.CLASS a 
                    JOIN 
                        SYSTEM.SUBJECT s ON a.Sub_ID = s.SUB_ID 
                    JOIN 
                        SYSTEM.TEACHER t ON a.TEACHER_ID = t.TEACHER_ID 
                    ORDER BY 
                        a.CLASS_ID ASC 
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
            string fullSchedule = row.Cells[5].Value.ToString();
            txtSchedule.Text = fullSchedule;
                

                // Parse the schedule string to extract date, start time, and end time
                ParseSchedule(fullSchedule);
                
                // Handle the number of students
                txtNOS.Text = row.Cells[6].Value.ToString();

            // Correcting the class section and subject IDs correctly
            ClassSectionID = txtID.Text;
            SubjectID = cbSubject.Text;

            // If 'limited' is meant to be an integer, parse it appropriately
            int parsedValue;
            if (int.TryParse(txtNOS.Text, out parsedValue))
            {
                limited = parsedValue;
            }
            else
            {
                MessageBox.Show("Invalid number of students format!");
            }

            // Handle ComboBox selections
            // Find the item that starts with the subject ID, using string concatenation
            if (row.Cells[1].Value != null)
                    cbSubject.SelectedItem = cbSubject.Items.Cast<string>()
                        .FirstOrDefault(i => i.StartsWith(row.Cells[1].Value.ToString().Split(' ')[0]));

                if (row.Cells[2].Value != null)
                    cbTeacher.SelectedItem = cbTeacher.Items.Cast<string>()
                        .FirstOrDefault(i => i.StartsWith(row.Cells[2].Value.ToString().Split(' ')[0]));
            }
        }

        private void ParseSchedule(string fullSchedule)
        {
            try
            {
                // Trim the input string to remove any extra spaces
                fullSchedule = fullSchedule.Trim();

                // Split the schedule into date and time parts using " - " as separator
                string[] scheduleParts = fullSchedule.Split(new string[] { " - " }, StringSplitOptions.None);

                if (scheduleParts.Length == 2)
                {
                    string datePart = scheduleParts[0].Trim();
                    string timePart = scheduleParts[1].Trim();

                    // Log the extracted parts for debugging
                    MessageBox.Show("Extracted Date Part (before trimming): " + datePart);
                    MessageBox.Show("Extracted Time Part: " + timePart);

                    // Now, let's ensure we are correctly extracting the date part and removing any time information.
                    // If datePart contains the start time, we will remove it.

                    // Use the last space index to identify where time might start in datePart
                    int lastSpaceIndex = datePart.LastIndexOf(' ');

                    // If there's a space indicating that time is included, trim it
                    if (lastSpaceIndex != -1)
                    {
                        datePart = datePart.Substring(0, lastSpaceIndex); // Keep only the date part, removing time
                    }

                    // Log the extracted date part for debugging
                    MessageBox.Show("Extracted Date Part (after trimming): " + datePart);

                    DateTime parsedDate;

                    // Try to parse the datePart string using the exact format
                    if (DateTime.TryParseExact(datePart, "dddd, dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        // If successful, display the formatted date in txtSchedule
                        txtSchedule.Text = parsedDate.ToString("dddd, dd MMMM yyyy");
                    }
                    else
                    {
                        // If parsing failed, display an error message
                        MessageBox.Show("Error: Failed to parse the date.");
                        return;
                    }

                    // Now, to parse the time part:
                    string[] timeParts = timePart.Split(new string[] { " - " }, StringSplitOptions.None);

                    // Log the time parts for debugging
                    MessageBox.Show("Time Parts Count: " + timeParts.Length);

                    // Ensure we have exactly 2 time parts (start and end time)
                    if (timeParts.Length == 2)
                    {
                        string startTime = timeParts[0].Trim();
                        string endTime = timeParts[1].Trim();

                        // Log both start and end time
                        MessageBox.Show("Start Time: " + startTime);
                        MessageBox.Show("End Time: " + endTime);

                        DateTime startDateTime;
                        DateTime endDateTime;

                        // Try parsing the start time
                        if (DateTime.TryParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
                        {
                            // Add the time of day to today's date for the start time
                            txtStartTime.Value = DateTime.Today.Add(startDateTime.TimeOfDay);
                        }
                        else
                        {
                            MessageBox.Show("Error: Start time format is incorrect. Start Time: " + startTime);
                            return;
                        }

                        // Try parsing the end time
                        if (DateTime.TryParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
                        {
                            // Add the time of day to today's date for the end time
                            txtEndTime.Value = DateTime.Today.Add(endDateTime.TimeOfDay);
                        }
                        else
                        {
                            MessageBox.Show("Error: End time format is incorrect. End Time: " + endTime);
                            return;
                        }
                    }
                    else
                    {
                        // If the time part is not exactly two parts, show error
                        MessageBox.Show("Error: Time range format is incorrect. Time Part: " + timePart);
                    }
                }
                else
                {
                    // If the schedule format does not match the expected parts, show an error
                    MessageBox.Show("Error: Schedule format is incorrect. Full Schedule: " + fullSchedule);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
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

        private void pbReload_Click(object sender, EventArgs e)
        {
            Refesh();
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0; // Setting the action for adding
            // Show the student addition UIµ

            ToggleUIForEditing(true);
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
        // Method to configure the DateTimePickers
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
        private void ValidateTimes()
        {
            // Ensure start time is not earlier than 00:00
            if (txtStartTime.Value < DateTime.Today)
            {
                MessageBox.Show("Start time cannot be earlier than 00:00.");
                txtStartTime.Value = DateTime.Today;  // Reset to 00:00
            }

            // Ensure end time is not earlier than start time
            if (txtEndTime.Value <= txtStartTime.Value)
            {
                MessageBox.Show("End time must be after start time.");
                txtEndTime.Value = txtStartTime.Value.AddMinutes(30);  // Add 30 minutes to start time as default
            }
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose class to edit!");
                return;
            }
            action = 1;
            ToggleUIForEditing(true);
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (action == 0) // Add new class
                    {
                        cmd = new MySqlCommand("SP_CLASS_ADD", conn); // Use stored procedure for adding
                    }
                    else // Update existing class
                    {
                        cmd = new MySqlCommand("SP_CLASS_UPDATE", conn); // Use stored procedure for updating
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Validate inputs
                    string subId, teacherId;
                    int nbS;

                    // Subject validation
                    if (string.IsNullOrWhiteSpace(cbSubject.Text) || !TryExtractId(cbSubject.Text, out subId))
                    {
                        MessageBox.Show("Invalid Subject selection. Please select a valid subject.");
                        return;
                    }

                    // Teacher validation
                    if (string.IsNullOrWhiteSpace(cbTeacher.Text) || !TryExtractId(cbTeacher.Text, out teacherId))
                    {
                        MessageBox.Show("Invalid Teacher selection. Please select a valid teacher.");
                        return;
                    }

                    // Number of students validation
                    if (!Int32.TryParse(txtNOS.Text, out nbS))
                    {
                        MessageBox.Show("Invalid number of students. Please enter a valid number.");
                        return;
                    }

                    // Start date validation
                    if (dtpStart.Value == DateTime.MinValue)
                    {
                        MessageBox.Show("Please enter a valid start date.");
                        return;
                    }

                    // Concatenate the schedule, start time, and end time in the desired format: "dddd HH:mm - HH:mm"
                    string scheduleDetails = $"{txtSchedule.Value:dddd, dd MMMM yyyy} {txtStartTime.Value:HH:mm} - {txtEndTime.Value:HH:mm}";

                    // Adding parameters for the stored procedure
                    cmd.Parameters.AddWithValue("p_SUB_ID", subId);
                    cmd.Parameters.AddWithValue("p_TEACHER_ID", teacherId);
                    cmd.Parameters.AddWithValue("p_START_DATE", dtpStart.Value);
                    cmd.Parameters.AddWithValue("p_FINISH_DATE", dtpFinish.Value);
                    cmd.Parameters.AddWithValue("p_SCHEDULE", scheduleDetails); // Use the concatenated schedule
                    cmd.Parameters.AddWithValue("p_NB_S", nbS);
                    cmd.Parameters.AddWithValue("p_CLASS_ID", txtID.Text);

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(action == 0 ? "Class added successfully!" : "Class updated successfully!");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving class: " + ex.Message, "Erreur lors de l'enregistrement de la classe : " + ex.Message, "Error", MessageBoxIcon.Error);
            }

            // Refresh the UI and clear inputs
            RefreshClassList();
        }

              private bool TryExtractId(string text, out string id)
                {
                    id = string.Empty;
                    // Logic to extract ID from the formatted text (e.g., "ID - Name")
                    if (text.Contains("-"))
                    {
                        id = text.Split('-')[0].Trim();
                        return true;
                    }
                    return false;
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


      

        private void lbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a class to delete!");
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
                        cmd.Parameters.AddWithValue("@ClassID", txtID.Text); // Utiliser la valeur textuelle  
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

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadClasses();
        }

        private void label7_Click(object sender, EventArgs e)
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
                for (int col = 0; col < dgvClass.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvClass.Columns[col].HeaderText;
                }

                // Fetch all data for the export, not just the current page
                List<DataRow> allRows = new List<DataRow>();

                // Loop through all pages and collect all data
                int totalRecords = GetTotalRecordCount(); // Get total record count from DB
                int totalPages = (totalRecords + pageSize - 1) / pageSize; // Calculate number of pages

                for (int page = 1; page <= totalPages; page++)
                {
                    currFrom = page; // Update current page number
                    LoadClasses();  // This loads students for the current page

                    // Collect rows from the DataGridView
                    foreach (DataGridViewRow row in dgvClass.Rows)
                    {
                        if (row.IsNewRow) continue; // Skip the new row placeholder

                        DataRow dataRow = ((DataTable)dgvClass.DataSource).NewRow();

                        // Copy row values to DataRow
                        for (int col = 0; col < dgvClass.Columns.Count; col++)
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
                MessageBox.Show("Please choose class to view!");
                return;
            }
            StudentsInClassSection studentsInClassSection = new StudentsInClassSection(ClassSectionID);
            studentsInClassSection.ShowDialog();
        }
        private void ClassSectionManager_Load(object sender, EventArgs e)
        {
            
        }

        // Fonction pour valider et gérer la valeur de la date et de l'heure avant de les affecter à un contrôle
        private void ValidateDateTime()
        { 
            // Vérification de la date dans txtSchedule
            if (txtSchedule.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("La date sélectionnée est invalide. La valeur sera réinitialisée.");
                txtSchedule.Value = DateTime.Now; // Réinitialiser à la date actuelle si la valeur est invalide
            }

            // Vérification de l'heure de début
            if (txtStartTime.Value < DateTime.MinValue || txtStartTime.Value > DateTime.Now)
            {
                MessageBox.Show("L'heure de début est invalide. Elle sera réinitialisée.");
                txtStartTime.Value = DateTime.Now; // Réinitialiser à l'heure actuelle si la valeur est invalide
            }

            // Vérification de l'heure de fin
            if (txtEndTime.Value < DateTime.MinValue || txtEndTime.Value > DateTime.Now)
            {
                MessageBox.Show("L'heure de fin est invalide. Elle sera réinitialisée.");
                txtEndTime.Value = DateTime.Now; // Réinitialiser à l'heure actuelle si la valeur est invalide
            }

            // Vérifier si l'heure de fin est après l'heure de début
            if (txtEndTime.Value <= txtStartTime.Value)
            {
                MessageBox.Show("L'heure de fin ne peut pas être avant l'heure de début. Veuillez ajuster l'heure de fin.");

                // Adjust the end time by adding minutes, but first ensure it's a valid DateTime
                DateTime newEndTime = txtStartTime.Value.AddMinutes(60);

                // Ensure the new end time is within valid range
                if (newEndTime > DateTime.MinValue && newEndTime < DateTime.MaxValue)
                {
                    txtEndTime.Value = newEndTime;  // Apply the valid new end time
                }
                else
                {
                    // If adding 30 minutes causes an invalid DateTime, we can handle it differently.
                    // For example, we can just set it to 30 minutes from now as a safe fallback
                    txtEndTime.Value = DateTime.Now.AddMinutes(60); // Set to 30 minutes from now
                }
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

            // Vous pouvez maintenant utiliser finalStart et finalEnd
            Console.WriteLine($"Début: {finalStart}, Fin: {finalEnd}");
        }


       
    }
}
