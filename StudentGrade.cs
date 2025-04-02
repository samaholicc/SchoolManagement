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
    public partial class StudentGrade : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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

        #region Load Data Methods
        private void LoadStudents()
        {
            try
            {
                if (string.IsNullOrEmpty(Login.ID))
                {
                    MessageBox.Show(GetLocalizedMessage("LoginIDNotSet"));
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            A.CLASS_ID AS `Class ID`, 
                            D.SUB_ID AS `Subject ID`, 
                            D.SUB_NAME AS `Subject Name`, 
                            D.CREDITS AS `Subject Credits`, 
                            A.MID_TERM AS `Mid Term`, 
                            A.FINAL_TERM AS `Final Term`, 
                            A.AVERAGE AS `Average`
                        FROM SYSTEM.results A
                        JOIN SYSTEM.class B ON A.CLASS_ID = B.CLASS_ID
                        JOIN SYSTEM.subject D ON B.SUB_ID = D.SUB_ID
                        WHERE A.STUDENT_ID = @ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorLoading") + " " + ex.Message);
            }
        }
        #endregion

        #region Event Handlers
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Login.ID))
                {
                    MessageBox.Show(GetLocalizedMessage("LoginIDNotSet"));
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            A.CLASS_ID AS `Class ID`, 
                            D.SUB_ID AS `Subject ID`, 
                            D.SUB_NAME AS `Subject Name`, 
                            D.CREDITS AS `Subject Credits`, 
                            A.MID_TERM AS `Mid Term`, 
                            A.FINAL_TERM AS `Final Term`, 
                            A.AVERAGE AS `Average`
                        FROM SYSTEM.results A
                        JOIN SYSTEM.class B ON A.CLASS_ID = B.CLASS_ID
                        JOIN SYSTEM.subject D ON B.SUB_ID = D.SUB_ID
                        WHERE A.STUDENT_ID = @ID 
                          AND (A.CLASS_ID LIKE @search 
                               OR D.SUB_ID LIKE @search 
                               OR D.SUB_NAME LIKE @search 
                               OR D.CREDITS LIKE @search 
                               OR A.MID_TERM LIKE @search 
                               OR A.FINAL_TERM LIKE @search 
                               OR A.AVERAGE LIKE @search)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Login.ID);
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvStudents.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("ErrorSearching") + " " + ex.Message);
            }
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            LoadStudents();
            txtSearch.Text = "";
            MessageBox.Show(GetLocalizedMessage("ReloadSuccess"));
        }

        private async void label6_Click(object sender, EventArgs e)
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
                saveFileDialog.Title = "Save Student Grades as CSV";
                saveFileDialog.FileName = $"StudentGrades_{Login.ID}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = await FetchGradesDataAsync();

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

                        MessageBox.Show(GetLocalizedMessage("ExportSuccess"));
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
                MessageBox.Show(GetLocalizedMessage("ErrorExporting") + " " + ex.Message);
            }
        }

        private async Task<DataTable> FetchGradesDataAsync()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"
                    SELECT 
                        A.CLASS_ID AS `Class ID`, 
                        D.SUB_ID AS `Subject ID`, 
                        D.SUB_NAME AS `Subject Name`, 
                        D.CREDITS AS `Subject Credits`, 
                        A.MID_TERM AS `Mid Term`, 
                        A.FINAL_TERM AS `Final Term`, 
                        A.AVERAGE AS `Average`
                    FROM SYSTEM.results A
                    JOIN SYSTEM.class B ON A.CLASS_ID = B.CLASS_ID
                    JOIN SYSTEM.subject D ON B.SUB_ID = D.SUB_ID
                    WHERE A.STUDENT_ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", Login.ID);
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
        #endregion

        #region Helper Methods
        private string GetLocalizedMessage(string messageKey)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var messages = currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
                ? new Dictionary<string, string>
                {
                    { "LoginIDNotSet", "L'ID de connexion n'est pas défini. Veuillez vous reconnecter." },
                    { "ErrorLoading", "Erreur lors du chargement des étudiants : " },
                    { "ErrorSearching", "Erreur lors de la recherche : " },
                    { "ErrorExporting", "Erreur d'exportation vers CSV : " },
                    { "ReloadSuccess", "Étudiants rechargés avec succès." },
                    { "ExportSuccess", "Exporté avec succès vers CSV." }
                }
                : new Dictionary<string, string>
                {
                    { "LoginIDNotSet", "Login ID is not set. Please log in again." },
                    { "ErrorLoading", "Error loading students: " },
                    { "ErrorSearching", "Error during search: " },
                    { "ErrorExporting", "Error exporting to CSV: " },
                    { "ReloadSuccess", "Students reloaded successfully." },
                    { "ExportSuccess", "Exported to CSV successfully." }
                };

            string message;
            if (messages.TryGetValue(messageKey, out message))
            {
                return message;
            }
            return "Unknown error";
        }
        #endregion

        private void StudentGrade_Load(object sender, EventArgs e)
        {

        }
    }
}