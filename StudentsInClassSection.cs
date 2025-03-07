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
    public partial class StudentsInClassSection : KryptonForm
    {
        #region Constants and Variables
        private const int CS_DropShadow = 0x00020000;
        private string connectionString = "Server=localhost;Database=system;User ID=root;Password=samia;";
        private string classSectionID;
        private bool isSelected = false;
        #endregion

        #region Constructor
        public StudentsInClassSection(string classSectionID)
        {
            InitializeComponent();
            this.classSectionID = classSectionID;
            LoadStudents(); // Load students for the passed ClassSectionID
        }
        #endregion

        #region Form Creation and Params
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }
        #endregion

        #region Load Students
        private void LoadStudents()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT s.Student_id AS `Student ID`, s.Full_name AS `Name`, " +
                                   "IFNULL(r.mid_term, 0) AS `Mid Term`, " +
                                   "IFNULL(r.final_term, 0) AS `Final Term`, " +
                                   "IFNULL(r.average, 0) AS `Average` " +
                                   "FROM studentstable s " +
                                   "LEFT JOIN results r ON s.student_id = r.student_id AND r.class_id = @ClassID " +
                                   "JOIN student_classes sc ON s.student_id = sc.student_id " +
                                   "WHERE sc.class_id = @ClassID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classSectionID); // Use the passed ClassSectionID
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvStudents.DataSource = dataTable;
                        count.Text = dgvStudents.RowCount.ToString() + "/" + ClassSectionManager.limited;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetErrorMessage("Error") + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isSelected) return;

            double mid, final, avg;
            if (!double.TryParse(txtMid.Text, out mid) || !double.TryParse(txtFinal.Text, out final))
            {
                MessageBox.Show(GetErrorMessage("InvalidInput"));
                return;
            }

            avg = (mid + final) / 2;
            txtAver.Text = avg.ToString("F2");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                INSERT INTO results (student_id, class_id, mid_term, final_term, average)
                VALUES (@StudentID, @ClassID, @Mid, @Final, @Avg)
                ON DUPLICATE KEY UPDATE 
                    mid_term = @Mid, 
                    final_term = @Final, 
                    average = @Avg";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Mid", mid);
                        cmd.Parameters.AddWithValue("@Final", final);
                        cmd.Parameters.AddWithValue("@Avg", avg);
                        cmd.Parameters.AddWithValue("@StudentID", lbMSSV.Text);
                        cmd.Parameters.AddWithValue("@ClassID", classSectionID);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show(GetErrorMessage("SaveSuccess"));
                        LoadStudents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetErrorMessage("Error") + ex.Message);
                }
            }
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetErrorMessage("InvalidInput"));
                return;
            }

            string studentId = lbMSSV.Text;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classSectionID))
            {
                MessageBox.Show(GetErrorMessage("NoRecordToDelete"));
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM student_classes WHERE student_id = @StudentID AND class_id = @ClassID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@ClassID", classSectionID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(GetErrorMessage("DeleteSuccess"));
                            LoadStudents(); // Reload the list of students
                        }
                        else
                        {
                            MessageBox.Show(GetErrorMessage("NoRecordToDelete"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetErrorMessage("Error") + ex.Message);
                }
            }
        }

        #endregion

        #region Event Handlers
        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                lbName.Text = row.Cells["Name"].Value.ToString();
                lbMSSV.Text = row.Cells["Student ID"].Value.ToString();
                txtMid.Text = row.Cells["Mid Term"].Value.ToString();
                txtFinal.Text = row.Cells["Final Term"].Value.ToString();
                txtAver.Text = row.Cells["Average"].Value.ToString();
                isSelected = true;
            }
        }

        

        

        private void pbStudents_Click(object sender, EventArgs e)
        {
            StudentClassSectionList studentList = new StudentClassSectionList();
            studentList.ShowDialog();
            LoadStudents();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        #endregion

        #region Export to Excel
        private void ExportToExcel()
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);

                // Add column headers to the Excel file
                for (int col = 0; col < dgvStudents.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvStudents.Columns[col].HeaderText;
                }

                // Fetch and add all data to Excel
                List<DataRow> allRows = new List<DataRow>();
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue;

                    DataRow dataRow = ((DataTable)dgvStudents.DataSource).NewRow();
                    for (int col = 0; col < dgvStudents.Columns.Count; col++)
                    {
                        dataRow[col] = row.Cells[col].Value.ToString();
                    }
                    allRows.Add(dataRow);
                }

                // Populate the Excel worksheet with the collected rows
                int rowIndex = 2;
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
        #endregion
        

    private string GetErrorMessage(string messageKey)
    {
        // Retrieve the current culture
        var currentCulture = CultureInfo.CurrentCulture.Name;

        // Return messages based on culture
        if (currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase))
        {
            // French messages
            switch (messageKey)
            {
                case "InvalidInput":
                    return "Entrée invalide!";
                case "SaveSuccess":
                    return "Enregistrement réussi";
                case "DeleteSuccess":
                    return "Suppression réussie";
                case "NoRecordToDelete":
                    return "Aucun enregistrement trouvé à supprimer";
                case "Error":
                    return "Erreur : ";
                default:
                    return "Erreur inconnue";
            }
        }
        else
        {
            switch (messageKey)
            {
                case "InvalidInput":
                    return "Invalid input!";
                case "SaveSuccess":
                    return "Save success";
                case "DeleteSuccess":
                    return "Deleted successfully.";
                case "NoRecordToDelete":
                    return "No matching record found to delete.";
                case "Error":
                    return "Error: ";
                default:
                    return "Unknown error";
            }
        }
    }

}
}
