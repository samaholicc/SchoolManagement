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
    public partial class StudentsInClassSection : KryptonForm
    {
        #region Constants and Variables
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private readonly string classSectionID;
        private bool isSelected = false;
        #endregion

        #region Constructor
        public StudentsInClassSection(string classSectionID)
        {
            InitializeComponent();
            this.classSectionID = classSectionID;
            LoadStudents();
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
                    string query = @"
                        SELECT s.Student_id AS `Student ID`, s.Full_name AS `Name`, 
                               IFNULL(r.mid_term, 0) AS `Mid Term`, 
                               IFNULL(r.final_term, 0) AS `Final Term`, 
                               IFNULL(r.average, 0) AS `Average`
                        FROM studentstable s
                        LEFT JOIN results r ON s.student_id = r.student_id AND r.class_id = @ClassID
                        JOIN student_classes sc ON s.student_id = sc.student_id
                        WHERE sc.class_id = @ClassID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassID", classSectionID);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvStudents.DataSource = dataTable;
                            // Replace string interpolation with string concatenation
                            count.Text = dgvStudents.RowCount + "/" + TeacherClassSection.StudentLimit;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetErrorMessage("Error") + " " + ex.Message);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetErrorMessage("NoRecordSelected"));
                return;
            }

            double mid, finalTerm, avg;
            if (!double.TryParse(txtMid.Text, out mid) || !double.TryParse(txtFinal.Text, out finalTerm))
            {
                MessageBox.Show(GetErrorMessage("InvalidInput"));
                return;
            }

            // Validate grade range (0 to 20)
            if (mid < 0 || mid > 20 || finalTerm < 0 || finalTerm > 20)
            {
                MessageBox.Show(GetErrorMessage("GradeOutOfRange"));
                return;
            }

            avg = (mid + finalTerm) / 2;
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
                        cmd.Parameters.AddWithValue("@Final", finalTerm);
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
                    MessageBox.Show(GetErrorMessage("Error") + " " + ex.Message);
                }
            }
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetErrorMessage("NoRecordSelected"));
                return;
            }

            string studentId = lbMSSV.Text;
            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show(GetErrorMessage("NoRecordToDelete"));
                return;
            }

            if (MessageBox.Show(GetErrorMessage("ConfirmDelete"), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
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
                                LoadStudents();
                            }
                            else
                            {
                                MessageBox.Show(GetErrorMessage("NoRecordToDelete"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(GetErrorMessage("Error") + " " + ex.Message);
                    }
                }
            }
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

        private void pbStudents_Click(object sender, EventArgs e)
        {
            // Check if the number of students exceeds the limit
            if (dgvStudents.RowCount >= TeacherClassSection.StudentLimit)
            {
                MessageBox.Show(GetErrorMessage("ClassFull"));
                return;
            }

            StudentClassSectionList studentList = new StudentClassSectionList(classSectionID);
            studentList.ShowDialog();
            LoadStudents();
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }
        #endregion

        #region Export to CSV
        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Students in Class Section as CSV";
                // Replace string interpolation with string concatenation
                saveFileDialog.FileName = "StudentsInClass_" + classSectionID + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchStudentsDataAsync();

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

                        MessageBox.Show(GetErrorMessage("ExportSuccess"));
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
                MessageBox.Show(GetErrorMessage("Error") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchStudentsDataAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"
                    SELECT s.Student_id AS `Student ID`, s.Full_name AS `Name`, 
                           IFNULL(r.mid_term, 0) AS `Mid Term`, 
                           IFNULL(r.final_term, 0) AS `Final Term`, 
                           IFNULL(r.average, 0) AS `Average`
                    FROM studentstable s
                    LEFT JOIN results r ON s.student_id = r.student_id AND r.class_id = @ClassID
                    JOIN student_classes sc ON s.student_id = sc.student_id
                    WHERE sc.class_id = @ClassID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClassID", classSectionID);
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    try
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(new Action(() => adapter.Fill(dataTable)));
                        return dataTable;
                    }
                    finally
                    {
                        adapter.Dispose();
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            finally
            {
                conn.Dispose();
            }
        }
        #endregion

        #region Localization
        private string GetErrorMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "InvalidInput", "Entrée invalide ! Les notes doivent être des nombres." },
                    { "GradeOutOfRange", "Les notes doivent être comprises entre 0 et 20 !" },
                    { "NoRecordSelected", "Aucun étudiant sélectionné." },
                    { "SaveSuccess", "Enregistrement réussi." },
                    { "DeleteSuccess", "Suppression réussie." },
                    { "NoRecordToDelete", "Aucun enregistrement trouvé à supprimer." },
                    { "ConfirmDelete", "Êtes-vous sûr de vouloir supprimer ?" },
                    { "Error", "Erreur : " },
                    { "ExportSuccess", "Export réussi vers CSV." },
                    { "ClassFull", "La classe est pleine." } // Added for class full message
                }
                : new Dictionary<string, string>
                {
                    { "InvalidInput", "Invalid input! Grades must be numbers." },
                    { "GradeOutOfRange", "Grades must be between 0 and 20!" },
                    { "NoRecordSelected", "No student selected." },
                    { "SaveSuccess", "Save successful." },
                    { "DeleteSuccess", "Deleted successfully." },
                    { "NoRecordToDelete", "No matching record found to delete." },
                    { "ConfirmDelete", "Are you sure you want to delete?" },
                    { "Error", "Error: " },
                    { "ExportSuccess", "Exported successfully to CSV." },
                    { "ClassFull", "Class is full." } // Added for class full message
                };

            string message = string.Empty; // Explicit declaration for C# 6.0 compatibility
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        private void StudentsInClassSection_Load(object sender, EventArgs e)
        {
        }
    }
}