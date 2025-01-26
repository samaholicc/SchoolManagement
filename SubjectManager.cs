using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Use MySQL data access

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
                LoadStudents();
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadStudents();
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
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0;

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Visible = true;
            label10.Visible = true;
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
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
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
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;

            LoadStudents();
            txtSearch.Text = "";
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

            workbook.SaveAs("Desktop\\Data.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            app.Quit();
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

        private void SubjectManager_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be added here
        }
    }
}