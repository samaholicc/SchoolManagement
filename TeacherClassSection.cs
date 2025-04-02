using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public static string ClassID { get; set; }
        public static int SubjectID { get; set; }
        public static int StudentLimit { get; set; }

        public TeacherClassSection()
        {
            InitializeComponent();
            LoadClasses();
        }

        #region Load Data Methods

        private void LoadClasses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT class_id AS `Class ID`, sub_id AS `Subject ID`, teacher_id AS `Teacher ID`, 
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

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
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

        #endregion

        #region Button Click Events

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT class_id AS `Class ID`, sub_id AS `Subject ID`, teacher_id AS `Teacher ID`, 
                               start_date AS `Start Date`, finish_date AS `End Date`, schedule AS `Schedule`, 
                               nb_s AS `Student Limit`
                        FROM class 
                        WHERE teacher_id = @teacher_id 
                        AND (class_id LIKE @search OR sub_id LIKE @search OR start_date LIKE @search 
                             OR finish_date LIKE @search OR schedule LIKE @search OR nb_s LIKE @search) 
                        ORDER BY class_id ASC 
                        LIMIT @limit OFFSET @offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@teacher_id", Login.ID);
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        cmd.Parameters.AddWithValue("@limit", pageSize);
                        cmd.Parameters.AddWithValue("@offset", (currFrom - 1) * pageSize);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
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

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await ExportToCsvAsync();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedErrorMessage("NoRecord"));
                return;
            }
            StudentsInClassSection studentsInClass = new StudentsInClassSection(ClassID);
            studentsInClass.ShowDialog();
        }

        #endregion

        #region Helper Methods

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                ClassID = row.Cells[0].Value.ToString(); // VARCHAR
                SubjectID = Convert.ToInt32(row.Cells[1].Value); // INT
                StudentLimit = Convert.ToInt32(row.Cells[6].Value); // INT
            }
        }

        private int GetTotalRecordCount()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(class_id) FROM class WHERE teacher_id = @teacher_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@teacher_id", Login.ID);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
                return 0;
            }
        }

        private async Task ExportToCsvAsync()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Teacher Class Sections as CSV";
                saveFileDialog.FileName = $"TeacherClasses_{Login.ID}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchClassesDataAsync();

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        StringBuilder csvContent = new StringBuilder();

                        // Écrire les en-têtes
                        string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                            .Select(column => $"\"{column.ColumnName}\"")
                            .ToArray();
                        csvContent.AppendLine(string.Join(",", columnNames));

                        // Écrire les données
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string[] fields = row.ItemArray.Select(field =>
                                $"\"{(field != null ? field.ToString().Replace("\"", "\"\"") : "")}\"")
                                .ToArray();
                            csvContent.AppendLine(string.Join(",", fields));
                        }

                        // Écrire dans le fichier
                        File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                        MessageBox.Show(GetLocalizedErrorMessage("Exports"));
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
                MessageBox.Show(GetLocalizedErrorMessage("Error") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchClassesDataAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"
                    SELECT class_id AS `Class ID`, sub_id AS `Subject ID`, teacher_id AS `Teacher ID`, 
                           start_date AS `Start Date`, finish_date AS `End Date`, schedule AS `Schedule`, 
                           nb_s AS `Student Limit`
                    FROM class 
                    WHERE teacher_id = @teacher_id 
                    ORDER BY class_id ASC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@teacher_id", Login.ID);
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    try
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
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

        private string GetLocalizedErrorMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "NoRecord", "Aucune classe sélectionnée à voir." },
                    { "Error", "Erreur : " },
                    { "Exports", "Export réussi vers CSV." }
                }
                : new Dictionary<string, string>
                {
                    { "NoRecord", "No class has been selected." },
                    { "Error", "Error: " },
                    { "Exports", "Exported successfully to CSV." }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }

        #endregion

        private void TeacherClassSection_Load(object sender, EventArgs e)
        {

        }
    }
}