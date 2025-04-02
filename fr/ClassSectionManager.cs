using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
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
                            cbTeacher.Items.Add($"{teacherId} - {teacherName}");
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
                            cbSubject.Items.Add($"{subjectId} - {SubjectName}");
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
                MessageBox.Show(es.Message);
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
                dtpStart.Value = Convert.ToDateTime(row.Cells[3].Value);
                dtpFinish.Value = Convert.ToDateTime(row.Cells[4].Value);
                txtSchedule.Text = row.Cells[5].Value.ToString();
                txtNOS.Text = row.Cells[6].Value.ToString();

                // Correcting the class section and subject IDs correctly  
                ClassSectionID = txtID.Text;
                SubjectID = cbSubject.Text;

                // If 'limited' is meant to be an integer, parse it appropriately  
                if (int.TryParse(txtNOS.Text, out int parsedValue))
                {
                    limited = parsedValue;
                }
                else
                {
                    MessageBox.Show("Invalid number of students format!");
                }

                // Handle ComboBox selections  
                cbSubject.SelectedItem = cbSubject.Items.Cast<string>().FirstOrDefault(i => i.StartsWith(row.Cells[1].Value.ToString().Split(' ')[0]));
                cbTeacher.SelectedItem = cbTeacher.Items.Cast<string>().FirstOrDefault(i => i.StartsWith(row.Cells[2].Value.ToString().Split(' ')[0]));

                
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

            // Reset control states
            cbSubject.Enabled = false;
            cbTeacher.Enabled = false;
            dtpStart.Enabled = false;
            dtpFinish.Enabled = false;
            txtSchedule.Enabled = false;
            txtNOS.Enabled = false;

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

            // Adding parameters for the stored procedure
            cmd.Parameters.AddWithValue("p_SUB_ID", subId);
            cmd.Parameters.AddWithValue("p_TEACHER_ID", teacherId);
            cmd.Parameters.AddWithValue("p_START_DATE", dtpStart.Value);
            cmd.Parameters.AddWithValue("p_FINISH_DATE", dtpFinish.Value);
            cmd.Parameters.AddWithValue("p_SCHEDULE", txtSchedule.Text);
            cmd.Parameters.AddWithValue("p_NB_S", nbS);
            cmd.Parameters.AddWithValue("p_CLASS_ID", txtID.Text);

            // Execute the stored procedure
            cmd.ExecuteNonQuery();
            MessageBox.Show(action == 0 ? "Class added successfully!" : "Class updated successfully!");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error saving class: " + ex.Message);
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            txtSchedule.CustomFormat = "dddd HH:mm";
            txtSchedule.Format = DateTimePickerFormat.Custom;
        }

        private void kryptonPalette1_PalettePaint(object sender, PaletteLayoutEventArgs e)
        {

        }
    }
}
