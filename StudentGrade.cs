using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access

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
                    string query = "SELECT rownum AS no, A.CLASS_ID AS `Class section ID`, D.SUB_ID AS `Subject ID`, D.SUB_NAME AS `Subject name`, D.credits AS `Subject credits`, A.Mid_Term AS `Mid term`, A.Final_Term AS `Final term`, A.Average AS `Average` " +
                                   "FROM KETQUA A " +
                                   "JOIN class B ON A.CLASS_ID = B.CLASS_ID " +
                                   "JOIN Subject D ON B.SUB_ID = D.SUB_ID " +
                                   "WHERE A.STUDENT_ID = @ID AND (" +
                                   "A.CLASS_ID LIKE @search OR D.SUB_ID LIKE @search OR D.SUB_NAME LIKE @search OR D.credits LIKE @search OR A.GIUAKI LIKE @search OR A.CUOIKI LIKE @search OR A.DTB LIKE @search)";

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
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Data";

            // Export data to Excel
            for (int i = 1; i < dgvStudents.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dgvStudents.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dgvStudents.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvStudents.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dgvStudents.Rows[i].Cells[j].Value.ToString();
                }
            }
            workbook.SaveAs("Desktop\\Data.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                             Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing,
                             Type.Missing, Type.Missing, Type.Missing);
            app.Quit();
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