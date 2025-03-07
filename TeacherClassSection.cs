using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SchoolManagement
{
    public partial class TeacherClassSection : KryptonForm
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

        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public static string ClassID { get; set; }

        public static int SubjectID;          // INT
        public static int StudentLimit;

        public TeacherClassSection()  // Constructor to pass ClassSectionID
        {
            InitializeComponent();
            LoadClasses();
           
        }

        private void LoadClasses()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = @"SELECT class_id AS `Class ID`, sub_id AS `Subject ID`, teacher_id AS `Teacher ID`, 
                                    start_date AS `Start Date`, finish_date AS `End Date`, schedule AS `Schedule`, 
                                    nb_s AS `Student Limit`
                                    FROM class 
                                    WHERE teacher_id = @teacher_id 
                                    ORDER BY class_id ASC 
                                    LIMIT @limit OFFSET @offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@teacher_id", Login.ID);
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvClass.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                                            string query = @"
                            SELECT Class_id AS `Class ID`, 
                                   sub_id AS `Subject ID`, 
                                   teacher_id AS `Teacher ID`, 
                                   start_date AS `Start Date`, 
                                   finish_date AS `End Date`, 
                                   schedule AS `Schedule`,
                                   nb_s AS `Class limit`
                            FROM class 
                            WHERE teacher_id = @teacher_id 
                            AND (Class_id LIKE @search 
                                 OR sub_id LIKE @search 
                                 OR start_date LIKE @search 
                                 OR finish_date LIKE @search 
                                 OR schedule LIKE @search 
                                 OR nb_s LIKE @search) 
                            ORDER BY class_id ASC 
                            LIMIT @limit OFFSET @offset";


                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@teacher_id", Login.ID);
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvClass.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            isSelected = false;
            txtSearch.Text = "";
            LoadClasses();
        }

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;

                DataGridViewRow row = dgvClass.Rows[e.RowIndex];

                ClassID = row.Cells[0].Value.ToString();  // VARCHAR
                SubjectID = Convert.ToInt32(row.Cells[1].Value);  // INT
                StudentLimit = Convert.ToInt32(row.Cells[6].Value);
            }
        }

        private void TeacherClassSection_Load(object sender, EventArgs e)
        {
            // Add any initialization if needed
        }

        private void picturebox1_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private int GetTotalRecordCount()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT COUNT(class_ID) FROM SYSTEM.STUDENT_classes";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
                return 0; // Return 0 in case of an error
            }
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

                MessageBox.Show(GetLocalizedErrorMessage("Exports"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
            LoadClasses();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedErrorMessage("NoRecord"));
            }
            StudentsInClass studentsInClass = new StudentsInClass(ClassID);
            studentsInClass.ShowDialog();
        }

        private void dgvClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string GetLocalizedErrorMessage(string messageKey)
        {
            // Detect the current culture (language setting of the system)
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            // Handle messages based on language
            if (currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)) // French culture
            {
                // French messages
                switch (messageKey)
                {
                    case "NoRecord":
                        return "Aucun classe selectionner à voir.";
                
                  
                    case "Error":
                        return "Erreur : ";
                    case "Exports":
                        return "Export réussi.";

                    default:
                        return "Erreur inconnue";
                }
            }
            else // Default to English culture
            {
                // English messages
                switch (messageKey)
                {
                    
                    
                  
                    case "NoRecord":
                        return "No class has been selected.";
                    case "Error":
                        return "Error: ";
                    case "Exports":
                        return "Exported successfully.";


                    default:
                        return "Unknown error";
                }
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadClasses();
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadClasses();
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void lbExport_Click(object sender, EventArgs e)
        {

        }
    }
                       






    }
