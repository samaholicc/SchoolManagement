using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SchoolManagement
{
    public partial class StudentGrade : KryptonForm
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

        public StudentGrade()
        {
            InitializeComponent();
            LoadStudents();
           
        }

        private void LoadStudents()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT A.CLASS_ID AS `Class ID`, D.SUB_ID AS `Subject ID`, D.SUB_NAME AS `Subject name`, D.credits AS `Subject credits`, A.Mid_TERM AS `Mid term`, A.Final_term AS `Final term`, A.Average AS `Average` " +
                                   "FROM results A " +
                                   "JOIN Class B ON A.Class_ID = B.CLASS_ID " +
                                   "JOIN Subject D ON B.SUB_ID = D.SUB_ID " +
                                   "WHERE A.STUDENT_ID = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Login.ID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT A.CLASS_ID AS `Class section ID`, D.SUB_ID AS `Subject ID`, D.SUB_NAME AS `Subject name`, D.credits AS `Subject credits`, A.Mid_Term AS `Mid term`, A.Final_Term AS `Final term`, A.Average AS `Average` " +
                                   "FROM results A " +
                                   "JOIN class B ON A.CLASS_ID = B.CLASS_ID " +
                                   "JOIN Subject D ON B.SUB_ID = D.SUB_ID " +
                                   "WHERE A.STUDENT_ID = @ID AND (" +
                                   "A.CLASS_ID LIKE @search OR D.SUB_ID LIKE @search OR D.SUB_NAME LIKE @search OR D.credits LIKE @search OR A.mid_term LIKE @search OR A.final_term LIKE @search OR A.average LIKE @search)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            LoadStudents();
            txtSearch.Text = "";
        }

        private void label6_Click(object sender, EventArgs e)
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
                for (int col = 0; col < dgvStudents.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvStudents.Columns[col].HeaderText;
                }

                // Fetch all data for the export (without pagination)
                List<DataRow> allRows = new List<DataRow>();

                // Load all rows from the DataGridView
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder

                    DataRow dataRow = ((DataTable)dgvStudents.DataSource).NewRow();

                    // Copy row values to DataRow
                    for (int col = 0; col < dgvStudents.Columns.Count; col++)
                    {
                        dataRow[col] = row.Cells[col].Value.ToString();
                    }

                    allRows.Add(dataRow); // Add the row to the list
                }

                // Populate Excel worksheet with all rows collected
                int rowIndex = 2; // Start from row 2 (because row 1 is the header)
                foreach (var row in allRows)
                {
                    for (int col = 0; col < dgvStudents.Columns.Count; col++)
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
        }



      

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optionally handle cell content clicks here if needed
        }

        private void StudentGrade_Load(object sender, EventArgs e)
        {

        }
    }
}