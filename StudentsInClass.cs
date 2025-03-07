using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SchoolManagement
{
    public partial class StudentsInClass : KryptonForm
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
        private string ClassID;
        public StudentsInClass(string ClassID)
        {
            InitializeComponent();
            this.ClassID = ClassID;
            LoadStudents();
            
        }

        private void LoadStudents()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
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
                        cmd.Parameters.AddWithValue("@ClassID", ClassID);  // Use the passed ClassSectionID
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvStudents.DataSource = dataTable;
                        count.Text = dgvStudents.RowCount.ToString() + "/" + ClassSectionManager.limited;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("Error: " + ex.Message, "Erreur : " + ex.Message));
                }
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            // Use TeacherClassSection.ClassID directly
            StudentClassList studentList = new StudentClassList(TeacherClassSection.ClassID);
            studentList.ShowDialog();
            LoadStudents(); // Refresh the list of students
        }

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
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isSelected) return;

            double mid, final, avg;
            if (!double.TryParse(txtMid.Text, out mid) || !double.TryParse(txtFinal.Text, out final))
            {
                MessageBox.Show(GetLocalizedMessage("Invalid input!", "Entrée invalide!"));
                return;
            }

            avg = (mid + final) / 2;
            txtAver.Text = avg.ToString("F2");

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
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
                        cmd.Parameters.AddWithValue("@ClassID", ClassID);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show(GetLocalizedMessage("Save success", "Sauvegarde réussie"));
                        LoadStudents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("Error: " + ex.Message, "Erreur : " + ex.Message));
                }
                LoadStudents();
            }
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

                // Collect rows from the DataGridView for export
                List<DataRow> allRows = new List<DataRow>();

                // Loop through the DataGridView rows and add them to the list for export
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder

                    DataRow dataRow = ((DataTable)dgvStudents.DataSource).NewRow();

                    // Copy the row values to DataRow
                    for (int col = 0; col < dgvStudents.Columns.Count; col++)
                    {
                        dataRow[col] = row.Cells[col].Value.ToString();
                    }

                    allRows.Add(dataRow); // Add the row to the list
                }

                // Populate the Excel worksheet with the collected rows
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

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("Please select a student first.", "Veuillez d'abord sélectionner un étudiant."));
                return;
            }

            // Make sure lbMSSV.Text and ClassSectionManager.ClassSectionID are correct
            string studentId = lbMSSV.Text;
            string classId = ClassID;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
            {
                MessageBox.Show(GetLocalizedMessage("Student ID or Class ID is invalid.", "L'ID de l'étudiant ou l'ID de la classe est invalide."));
                return;
            }

            // Debugging: Check the current values of studentId and classId
            MessageBox.Show($"Student ID: {studentId}, Class ID: {classId}");

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM student_classes WHERE student_id = @StudentID AND class_id = @ClassID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@ClassID", classId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any row was affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(GetLocalizedMessage("Deleted successfully.", "Supprimé avec succès."));
                            LoadStudents(); // Reload the list of students
                        }
                        else
                        {
                            MessageBox.Show(GetLocalizedMessage("No matching record found to delete.", "Aucun enregistrement trouvé à supprimer."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("Error: " + ex.Message, "Erreur : " + ex.Message));
                }
                LoadStudents();
            }
        }

        private string GetLocalizedMessage(string englishMessage, string frenchMessage)
        {
            if (CultureInfo.CurrentCulture.Name == "fr-FR")
            {
                return frenchMessage;  // Return the French message if culture is French
            }
            else
            {
                return englishMessage;  // Return the English message by default
            }
        }

        private void StudensInClass_Load(object sender, EventArgs e)
        {
            // Additional load logic if necessary.
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle any content click events for the DataGridView if necessary.
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
