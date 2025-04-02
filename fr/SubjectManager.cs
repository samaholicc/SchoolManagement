using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Use MySQL data access
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;



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

        public SubjectManager()
        {
            InitializeComponent();
            LoadSubjects();
           
        }

        private void LoadSubjects()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT a.SUB_ID AS `Subject ID`,    a.SUB_NAME AS `Name`,    a.CREDITS AS `Credits`FROM  SUBJECT a ORDER BY     a.SUB_ID ASC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", (currFrom - 1) * pageSize + 1);
                        cmd.Parameters.AddWithValue("@end", currFrom * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadSubjects();
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadSubjects();
        }

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

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0;

            pbStudents.Visible = false;
            lbSubjAdd.Visible = false;
            pbEdit.Visible = false;
            lbSubjEdit.Visible = false;
            pbDelete.Visible = false;
            lbSubjDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Visible = true;
            lbSubjID.Visible = true;
            txtID.Enabled = true;

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a subject to edit!");
                return;
            }
            action = 1;

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

        private void pbReload_Click(object sender, EventArgs e)
        {
            Refesh();
        }

        private void Refesh()
        {
            pbStudents.Visible = true;
            lbSubjAdd.Visible = true;
            pbEdit.Visible = true;
            lbSubjEdit.Visible = true;
            pbDelete.Visible = true;
            lbSubjDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;

            LoadSubjects();
            txtSearch.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

                // Fetch all data for the export, not just the current page
                List<DataRow> allRows = new List<DataRow>();

                // Loop through all pages and collect all data

               
                
                    LoadSubjects();  // This loads students for the current page

                    // Collect rows from the DataGridView
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
            LoadSubjects();
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a subject to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        MySqlCommand cmd = new MySqlCommand("SP_SUBJECT_DELETE", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_SUB_ID", txtID.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Delete success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            isSelected = false;
            Refesh();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            if (action == 0) // Add new subject
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        MySqlCommand cmd = new MySqlCommand("SP_SUBJECT_ADD", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_SUB_NAME", txtName.Text); // Ensure this matches your stored procedure
                        cmd.Parameters.AddWithValue("p_CREDITS", Int32.Parse(txtAddress.Text)); // If txtAddress holds credits
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Add success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else // Edit existing subject
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        MySqlCommand cmd = new MySqlCommand("SP_SUBJECT_UPDATE", conn); // Assuming you have a proper procedure set up
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for update operation correctly
                        cmd.Parameters.AddWithValue("p_SUB_ID", txtID.Text); // Add subject ID parameter
                        cmd.Parameters.AddWithValue("p_SUB_NAME", txtName.Text); // Ensure this parameter is correctly referenced
                        cmd.Parameters.AddWithValue("p_CREDITS", Int32.Parse(txtAddress.Text)); // Assuming txtAddress holds credits
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Edit success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            Refesh(); // Call refresh to update the UI
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Database connection string
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                // Create and open a connection
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // SQL query to search subjects by SUB_NAME or SUB_ID
                    string query = "SELECT a.SUB_ID AS `Subject ID`, a.SUB_NAME AS `Name`, a.CREDITS AS `Credits` " +
                                   "FROM SUBJECT a " +
                                   "WHERE a.SUB_NAME LIKE @search OR a.SUB_ID LIKE @search " +
                                   "ORDER BY a.SUB_ID ASC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add the search parameter to match the search text
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        // Execute the query and load the results into a DataTable
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Bind the results to the DataGridView
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SubjectManager_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here
        }
    }
}