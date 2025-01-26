using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class ClassSectionManager : KryptonForm
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
        private int pageSize = 10;

        // Instance variable for ClassSectionID
        private string classSectionID;

        public string ClassSectionID
        {
            get { return classSectionID; }
            set { classSectionID = value; }
        }

        public static string SubjectID;
        public static int limited;

        public ClassSectionManager()
        {
            InitializeComponent();
            LoadClasses();
            LoadSubjects();
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT TEACHER_ID, FULL_NAME FROM SYSTEM.TEACHER", conn);
                    cbTeacher.Items.Clear(); // Clear existing items

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int teacherId = reader.GetInt32(0);
                            string teacherName = reader.GetString(1);
                            cbTeacher.Items.Add($"{teacherId} - {teacherName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LoadSubjects()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT SUB_ID, SUB_NAME FROM SYSTEM.SUBJECT", conn);
                    cbSubject.Items.Clear();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int subjectId = reader.GetInt32(0);
                            string SubjectName = reader.GetString(1);
                            cbSubject.Items.Add($"{subjectId} - {SubjectName}");
                        }
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("Error loading subjects: " + es.Message);
            }
        }


        private void LoadClasses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                            SELECT 
                                a.CLASS_ID AS 'CLASS SECTION ID', 
                                CONCAT(a.Sub_ID, ' - ', s.SUB_NAME) AS `Subject`, 
                                CONCAT(a.TEACHER_ID, ' - ', t.Full_name) AS `Teacher`, 
                                a.START_DATE AS 'START', 
                                a.FINISH_DATE AS 'FINISH', 
                                a.SCHEDULE AS 'SCHEDULE', 
                                a.NB_S AS 'N.O.S' 
                            FROM 
                                SYSTEM.CLASS a 
                            JOIN 
                                SYSTEM.SUBJECT s ON a.Sub_ID = s.SUB_ID 
                            JOIN 
                                SYSTEM.TEACHER t ON a.TEACHER_ID = t.TEACHER_ID 
                            ORDER BY 
                                a.CLASS_ID ASC 
                            LIMIT @PageSize OFFSET @Offset;", conn);

                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Offset", (currFrom - 1) * pageSize);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Fully qualifying DataTable
                        System.Data.DataTable dataTable = new System.Data.DataTable();  // Fully qualified reference
                        dataTable.Load(reader);
                        dgvClass.DataSource = dataTable;
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];

                // Handle ID retrieval with conversion
                txtID.Text = row.Cells[0].Value.ToString(); // Ensure this is a string
                cbSubject.Text = row.Cells[1].Value.ToString();   // Convert to string
                cbTeacher.Text = row.Cells[2].Value.ToString();   // Convert to string
                dtpStart.Value = Convert.ToDateTime(row.Cells[3].Value);
                dtpFinish.Value = Convert.ToDateTime(row.Cells[4].Value);
                txtSchedule.Text = row.Cells[5].Value.ToString();
                txtNOS.Text = row.Cells[6].Value.ToString();

                // Setting class section and subject IDs correctly
                ClassSectionID = txtID.Text; // This will now use the instance variable
                SubjectID = cbSubject.Text;
                limited = int.Parse(txtNOS.Text);

                string classId = row.Cells[1].Value.ToString(); // CLASS_ID  
                if (cbSubject.Items.Contains(classId))
                {
                    cbSubject.SelectedItem = classId; // Set the selected class in ComboBox  
                }
                else
                {
                    cbSubject.Text = classId; // Set the text if the item is not found  
                }

                string TeacherId = row.Cells[2].Value.ToString();
                if (cbSubject.Items.Contains(TeacherId))
                {
                    cbTeacher.SelectedItem = TeacherId; // Set the selected teacher in ComboBox  
                }
                else
                {
                    cbTeacher.Text = TeacherId; // Set the text if the item is not found  
                }

                Form Update = new StudentsInClass(ClassSectionID);
                Update.Show();
            }
        }

        private void Refesh()
        {
            LoadClasses();

            // Reset UI elements
            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.Text = "";
            cbTeacher.Text = "";
            txtSchedule.Text = "";
            txtNOS.Text = "";

            // Reset control states
            cbSubject.Enabled = false;
            cbTeacher.Enabled = false;
            dtpStart.Enabled = false;
            dtpFinish.Enabled = false;
            txtSchedule.Enabled = false;
            txtNOS.Enabled = false;

            // Show relevant buttons and labels
            pbStudents.Visible = true;
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
            pbDetail.Visible = true;
            lbDetail.Visible = true;

            isSelected = false; // Reset selection status
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            Refesh();
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0; // Setting the action for adding
            // Show the student addition UIµ

            ToggleUIForEditing(true);
        }

        private void ToggleUIForEditing(bool isEditingMode)
        {
            pbStudents.Visible = !isEditingMode;
            lbStudents.Visible = !isEditingMode;
            pbEdit.Visible = !isEditingMode;
            lbEdit.Visible = !isEditingMode;
            pbDelete.Visible = !isEditingMode;
            lbDelete.Visible = !isEditingMode;
            pbSave.Visible = isEditingMode;
            lbSave.Visible = isEditingMode;
            pbDetail.Visible = !isEditingMode;
            lbDetail.Visible = !isEditingMode;

            // Clear input fields
            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.Text = "";
            cbTeacher.Text = "";
            txtSchedule.Text = "";
            txtNOS.Text = "";

            // Enable or disable controls based on mode
            cbSubject.Enabled = isEditingMode;
            cbTeacher.Enabled = isEditingMode;
            dtpStart.Enabled = isEditingMode;
            dtpFinish.Enabled = isEditingMode;
            txtSchedule.Enabled = isEditingMode;
            txtNOS.Enabled = isEditingMode;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose class to edit!");
                return;
            }
            action = 1;
            ToggleUIForEditing(true);
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (action == 0) // Add new class  
                    {
                        cmd = new MySqlCommand("SP_CLASS_ADD", conn);
                    }
                    else // Update existing class  
                    {
                        cmd = new MySqlCommand("SP_CLASS_UPDATE", conn);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Extraction des IDs à partir des ComboBox  
                    int subId, teacherId, nbS;

                    if (string.IsNullOrWhiteSpace(cbSubject.Text) || !TryExtractId(cbSubject.Text, out subId))
                    {
                        MessageBox.Show("Invalid Subject selection. Please select a valid subject.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(cbTeacher.Text) || !TryExtractId(cbTeacher.Text, out teacherId))
                    {
                        MessageBox.Show("Invalid Teacher selection. Please select a valid teacher.");
                        return;
                    }

                    if (!Int32.TryParse(txtNOS.Text, out nbS))
                    {
                        MessageBox.Show("Invalid number of students. Please enter a valid number.");
                        return;
                    }

                    // Ajout des paramètres  
                    cmd.Parameters.AddWithValue("p_SUB_ID", subId);
                    cmd.Parameters.AddWithValue("p_TEACHER_ID", teacherId);
                    cmd.Parameters.AddWithValue("p_START_DATE", dtpStart.Value);
                    cmd.Parameters.AddWithValue("p_FINISH_DATE", dtpFinish.Value);
                    cmd.Parameters.AddWithValue("p_SCHEDULE", txtSchedule.Text);
                    cmd.Parameters.AddWithValue("p_NB_S", nbS);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show(action == 0 ? "Add success" : "Edit success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Refesh(); // Refresh the data grid view after adding or updating  
        }

        private bool TryExtractId(string input, out int id)
        {
            id = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string[] parts = input.Split(new[] { " - " }, StringSplitOptions.None);
            return parts.Length > 0 && Int32.TryParse(parts[0], out id);
        }

        private void lbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose class to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM SYSTEM.CLASS WHERE CLASS_ID=@ClassID", conn);
                        cmd.Parameters.AddWithValue("@ClassID", txtID.Text);
                        cmd.ExecuteNonQuery();
                    }
                    Refesh(); // Refresh the data grid view after deletion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadClasses();
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadClasses();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Export DataGridView to Excel
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Data";

            // Adding headers
            for (int i = 1; i <= dgvClass.Columns.Count; i++)
            {
                worksheet.Cells[1, i] = dgvClass.Columns[i - 1].HeaderText;
            }

            // Adding row data
            for (int i = 0; i < dgvClass.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvClass.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dgvClass.Rows[i].Cells[j].Value?.ToString() ?? ""; // Handle potential nulls
                }
            }

            // Save the workbook
            workbook.SaveAs("Desktop\\Data.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            app.Quit();
        }

        private void ClassSectionManager_Load(object sender, EventArgs e)
        { }


    }
}
