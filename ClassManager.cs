using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public partial class ClassManager : KryptonForm
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
        private string classSectionID;

        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public static string ClassSectionID;
        public static string SubjectID;
        public static int limited;

        public ClassManager()
        {
            InitializeComponent();
            LoadClasses();
            LoadSubjects();
        }



        private void LoadClasses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    string query = @"
            SELECT 
                c.CLASS_ID AS `CLASS ID`, 
                c.CLASS_NAME AS `CLASS NAME`, 
                s.SUB_NAME AS `SUBJECT NAME`,
                c.NB_S AS `NUMBERS`
            FROM 
                SYSTEM.CLASS c  
            LEFT JOIN 
                SYSTEM.SUBJECT s ON s.SUB_ID = c.SUB_ID -- Assuming there is a foreign key relation  
            ORDER BY 
                c.CLASS_ID ASC 
            LIMIT @PageSize OFFSET @Offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.AddWithValue("@Offset", (currFrom - 1) * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvClass.DataSource = dataTable; // Set the DataGridView's data source  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading classes: " + ex.Message);
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
                    cbSubject.Items.Clear(); // Clear previous items

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int subjectId = reader.GetInt32(0);
                            string subjectName = reader.GetString(1);
                            cbSubject.Items.Add($"{subjectId} - {subjectName}"); // Optional formatting for display  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }
        public string SelectedClassSectionID
        {
            get
            {
                if (dgvClass.SelectedRows.Count > 0)
                {
                    return dgvClass.SelectedRows[0].Cells[0].Value?.ToString(); // Assuming the classSectionID is in the first column
                }
                return null;
            }
        }
        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if the row index is valid
            {
                isSelected = true; // Mark that a class is selected
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];

                // Ensure there are enough cells before accessing
                if (row.Cells.Count >= 4) // Ensure at least 4 columns
                {
                    // Update the text fields with the selected row's data
                    txtID.Text = row.Cells[0].Value?.ToString();              // Column 1 (ID)
                    cbSubject.Text = row.Cells[2].Value?.ToString();         // Column 3 (Subject)
                    txtName.Text = row.Cells[1].Value?.ToString();           // Column 2 (Name)
                    txtNOS.Text = row.Cells[3].Value?.ToString();            // Column 4 (Number of Students)

                    // Pass the updated ClassSectionID to the next form
                    string ClassSectionID = txtID.Text;

                    StudentsInClass studentsInClassForm = new StudentsInClass(ClassSectionID);

                    studentsInClassForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The selected row does not have enough columns.");
                }


            }
        }
        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a class to edit!");
                return;
            }
            action = 1; // Set action to edit
            EnableInputs(); // Enable input fields
            ToggleUIForEditing(true); // Toggle UI for editing
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0; // Setting the action for adding
            ToggleUIForEditing(true);
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtNOS.Text))
                {
                    MessageBox.Show("Please fill in all the required fields.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (action == 0) // Add new class
                    {
                        cmd = new MySqlCommand("INSERT INTO CLASS (CLASS_ID,CLASS_NAME, NB_S) VALUES (@CLASS_ID,  @CLASS_NAME, @NOS)", conn);
                        cmd.Parameters.AddWithValue("@CLASS_ID", txtID.Text); // Unique identifier for new entries
                    }
                    else // Edit existing class
                    {
                        cmd = new MySqlCommand("UPDATE CLASS SET CLASS_NAME=@CLASS_NAME, NB_S=@NOS WHERE CLASS_ID=@CLASS_ID", conn);
                        cmd.Parameters.AddWithValue("@CLASS_ID", txtID.Text); // Required for update condition
                    }

                    // Common parameters for both add and update
                    cmd.Parameters.AddWithValue("@CLASS_NAME", txtName.Text); // Class name
                    cmd.Parameters.AddWithValue("@NOS", int.Parse(txtNOS.Text)); // Number of students

                    int result = cmd.ExecuteNonQuery(); // Execute the command

                    if (result > 0)
                    {
                        MessageBox.Show(action == 0 ? "Add success" : "Edit success");
                    }
                    else
                    {
                        MessageBox.Show("No records were affected. Please check your input.");
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers for the Number of Students.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            RefreshData(); // Refresh the data grid view after adding or updating
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void ToggleUIForEditing(bool isEditingMode)
        {
            // Show/hide buttons and labels based on editing mode
            pbStudents.Visible = !isEditingMode;
            lbStudents.Visible = !isEditingMode;
            pbEdit.Visible = !isEditingMode;
            lbEdit.Visible = !isEditingMode;
            pbDelete.Visible = !isEditingMode;
            lbDelete.Visible = !isEditingMode;
            pbSave.Visible = isEditingMode; // Show Save button in editing mode
            lbSave.Visible = isEditingMode;
            pbDetail.Visible = !isEditingMode;
            lbDetail.Visible = !isEditingMode;


            // Enable or disable controls based on mode
            cbSubject.Enabled = isEditingMode;
            txtName.Enabled = isEditingMode;
            txtNOS.Enabled = isEditingMode;
        }

        private void RefreshData()
        {
            LoadClasses();
            ClearInputs();
            DisableInputs();

            // Reset control states
            cbSubject.Enabled = false;
            txtName.Enabled = false;
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

            isSelected = false;
        }

        private void ClearInputs()
        {
            // Clear all input fields
            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.SelectedIndex = -1;
            txtName.Text = "";
            txtNOS.Text = "";
        }

        private void EnableInputs()
        {
            txtID.Enabled = true;
            cbSubject.Enabled = true;
            txtName.Enabled = true;
            txtNOS.Enabled = true;
        }

        private void DisableInputs()
        {
            txtID.Enabled = false;
            cbSubject.Enabled = false;
            txtName.Enabled = false;
            txtNOS.Enabled = false;
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a class to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=system;User ID=root;Password=samia;"))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM CLASS WHERE CLASS_ID=@CLASS_ID", conn);
                        cmd.Parameters.AddWithValue("@CLASS_ID", txtID.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Delete success");
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting class: " + ex.Message);
                }
            }
        }

        private void pbDetail_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a class to view!");
                return;
            }
            StudentsInClass studentsInClassSection = new StudentsInClass(ClassSectionID);
            studentsInClassSection.ShowDialog();
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

        private void ClassManager_Load(object sender, EventArgs e)
        {
            // Any additional load logic can be placed here if required
        }

        private void txtNOS_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
 