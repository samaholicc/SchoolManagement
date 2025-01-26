using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access
using System.IO;

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

        public StudentsInClass(string classSectionID)
        {
            InitializeComponent();
            ClassManager.ClassSectionID = classSectionID;  // Set ClassSectionID in the static ClassManager
            LoadStudents();
        }

        private void LoadStudents()
        {
            if (string.IsNullOrEmpty(ClassManager.ClassSectionID))
            {
                MessageBox.Show("ClassSectionID is not set. Please select a class section.");
                return;
            }

            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT A.student_id AS `Student ID`, A.full_name AS `Name` " +
                                   "FROM STUDENTSTABLE A " +
                                   "WHERE A.CLASS_ID = @CLASS_ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CLASS_ID", ClassManager.ClassSectionID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvStudents.DataSource = dataTable;

                            count.Text = dgvStudents.RowCount.ToString() + "/" + ClassManager.limited;
                            if (dgvStudents.RowCount == ClassManager.limited)
                            {
                                pbStudents.Visible = false;
                                lbStudents.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            // Since ClassSectionID is stored in ClassManager, you don't need a separate variable
            StudentClassList studentList = new StudentClassList(ClassManager.ClassSectionID);  // Use ClassManager.ClassSectionID directly
            studentList.ShowDialog();
            LoadStudents();
        }

        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                lbName.Text = row.Cells[1].Value.ToString();
                lbMSSV.Text = row.Cells[0].Value.ToString();
                isSelected = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Export data to Excel logic
            try
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

                // Dynamically get the user's Desktop path
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Data.xls");

                workbook.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                 Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing,
                                 Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data to Excel: " + ex.Message);
            }
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SP_CLASSSTUDENT_DELETE", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("p_STUDENT_ID", lbMSSV.Text);  // Correct the parameter to the student ID
                            cmd.Parameters.AddWithValue("p_CLASS_ID", ClassManager.ClassSectionID);  // Use the class section ID here
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Deleted successfully");
                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting student: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete!");
            }
        }

        private void StudensInClass_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here.
        }
    }
}
