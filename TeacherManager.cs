using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;

namespace SchoolManagement
{
    public partial class TeacherManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private int action; // 0 - add, 1 - edit  
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        // Initialize the form  
        public TeacherManager()
        {
            InitializeComponent();
            LoadTeachers();
            LoadComboBoxDepartment();
        }

        // Customize form appearance  
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        #region Load Data Methods

        private void LoadComboBoxDepartment()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT DEP_ID, DEP_NAME FROM DEP", conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string departmentId = reader.GetString(0);
                                string departmentName = reader.GetString(1);
                                cbDepartment.Items.Add($"{departmentId} - {departmentName}");  // Optional formatting for display  
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Handle exceptions  
            }
        }

        private void LoadTeachers()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                            string query = @"
                    SELECT 
                        a.TEACHER_ID AS `Teacher ID`, 
                        CONCAT(a.DEP_ID, ' - ', d.DEP_NAME) AS `Department`,  -- Combine ID and name  
                        a.FULL_NAME AS `Name`,  
                        a.DATE_OF_BIRTH AS `Birth`, 
                        a.GENDER AS `Gender`, 
                        a.ADRESS AS `Address`
                    FROM SYSTEM.TEACHER a  
                    JOIN SYSTEM.DEP d ON a.DEP_ID = d.DEP_ID"; 

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvTeachers.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Button Click Events

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadTeachers();
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            currFrom++;
            LoadTeachers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTeachers();
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            currFrom = 1;
            LoadTeachers();
            showAction();
            ClearInputs();
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a teacher to edit!");
                return;
            }

            action = 1;
            SetEditMode();
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose a teacher to delete!");
                return;
            }

            DeleteTeacher();
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            SaveTeacher();
        }

        #endregion

        #region Search and Actions

        private void SearchTeachers()
        {
            // Similar to LoadTeachers but with search functionality  
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        a.TEACHER_ID AS `Teacher ID`, 
                        a.DEP_ID AS `Department`, 
                        a.FULL_NAME AS `Name`, 
                        a.DATE_OF_BIRTH AS `Birth`, 
                        a.GENDER AS `Gender`, 
                        a.ADRESS AS `Address` 
                    FROM SYSTEM.TEACHER a 
                    WHERE 
                        a.FULL_NAME LIKE @search OR 
                        a.TEACHER_ID LIKE @search OR 
                        a.DEP_ID LIKE @search OR 
                        a.GENDER LIKE @search OR 
                        a.ADRESS LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvTeachers.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteTeacher()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                    using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                    {
                        MySqlCommand cmd = new MySqlCommand("SP_TEACHER_DELETE", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("p_TEACHER_ID", txtID.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Delete success");
                    isSelected = false;
                    LoadTeachers();
                    showAction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveTeacher()
        {
            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    MySqlCommand cmd;


                    if (string.IsNullOrWhiteSpace(txtName.Text) || cbDepartment.SelectedItem == null)
                    {
                        MessageBox.Show("Please fill in all required fields and select a department.");
                        return;
                    }


                    
                            

                            if (action == 0) // Add new teacher  
                            {
                                string query = "INSERT INTO SYSTEM.TEACHER (FULL_NAME, ADRESS, GENDER, DATE_OF_BIRTH, PASSWORD, DEP_ID) " +
                                               "VALUES (@fullname, @adress, @gender, @dateofbirth, @password, @depId);";

                                cmd = new MySqlCommand(query, conn);
                                cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                                cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                                cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                                cmd.Parameters.AddWithValue("@password", Encrypt.HashString(txtPassword.Text));

                                // Attempt to safely parse CLASS_ID  
                                string departmentInfo = cbDepartment.SelectedItem.ToString();
                                if (departmentInfo.Split('-').Length > 0 && int.TryParse(departmentInfo.Split('-')[0].Trim(), out int depId))
                                {
                                    cmd.Parameters.AddWithValue("@depId", depId);
                                }
                                else
                                {
                                    MessageBox.Show("Selected department ID is not valid.");
                                    return;
                                }

                                cmd.ExecuteNonQuery(); // Execute the insertion  
                                     int newTeacherId = Convert.ToInt32(cmd.ExecuteScalar());

                        // Create an account for the new student  
                        InsertData insertData = new InsertData();
                        string result = insertData.AddAccount(txtName.Text, txtPassword.Text, newTeacherId, "Teacher", 0);
                            }
                            else // Edit existing teacher  
                            {
                                string query = "UPDATE SYSTEM.TEACHER SET FULL_NAME = @fullname, ADRESS = @adress, " +
                                               "GENDER = @gender, DATE_OF_BIRTH = @dateofbirth, PASSWORD = @password WHERE TEACHER_ID = @id";

                                cmd = new MySqlCommand(query, conn);
                                cmd.Parameters.AddWithValue("@fullname", txtName.Text);
                                cmd.Parameters.AddWithValue("@adress", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Homme" : "Femme");
                                cmd.Parameters.AddWithValue("@dateofbirth", dtpBirth.Value);
                                cmd.Parameters.AddWithValue("@password", Encrypt.HashString(txtPassword.Text));
                                cmd.Parameters.AddWithValue("@id", txtID.Text); // Ensure the Teacher ID is included

                        // Attempt to safely parse CLASS_ID  
                        string departmentInfo = cbDepartment.SelectedItem.ToString();

                        // Vérifiez que la chaîne commence par "dep" 
                        if (departmentInfo.StartsWith("DEP"))
                        {
                            // Obtenez l'index de l'espace qui sépare l'ID du nom  
                            int spaceIndex = departmentInfo.IndexOf(' ');

                            // Extrayez la sous-chaîne qui contient l'ID du département  
                            // Cela suppose que l'ID est toujours "dep" + 3 chiffres  
                            string depIdString = departmentInfo.Substring(0, spaceIndex); // "dep007"

                            // Vérifiez si l'ID commence par "dep" et contient 3 chiffres après  
                            if (depIdString.Length == 6 && depIdString.StartsWith("DEP") && int.TryParse(depIdString.Substring(3), out int depId))
                            {
                                // Utilisez depId comme vous le souhaitez  
                                cmd.Parameters.AddWithValue("@depId", depId);
                            }
                            else
                            {
                                MessageBox.Show("Selected department ID is not valid.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Selected department format is not valid.");
                            return;
                        }

                        cmd.ExecuteNonQuery(); // Execute the update  
                        InsertData insertData = new InsertData();
                        string result = insertData.UpdatePassword(txtName.Text, txtPassword.Text);
                        MessageBox.Show("Edit success");
                    }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message); // Handle any errors  
                    }
            ClearInputs();
            LoadTeachers();
        }
            
            
                    
              
            
        #endregion

                    #region Helper Methods

        private void ClearInputs()
        {
            // Clear input fields  
            txtID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPassword.Text = "";
            cbDepartment.SelectedIndex = -1; // Clears selection  
            dtpBirth.Value = DateTime.Now; // Reset to current date  
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void showAction()
        {

            pbTeachers.Visible = true;      // Afficher le bouton "Add"
            lbStudents.Visible = true;      // Afficher le label pour les étudiants  
            pbEdit.Visible = true;          // Afficher le bouton "Edit"
            lbEdit.Visible = true;
            pbDelete.Visible = true;        // Afficher le bouton "Delete"
            lbDelete.Visible = true;

            // Masquer le bouton "Save" initialement ou lorsqu’il ne doit pas être visible  
            pbSave.Visible = false;
            lbSave.Visible = false;
        }
        

        private void SetEditMode()
        {
            pbTeachers.Visible = false; // Cacher le bouton "Add"
            lbStudents.Visible = false; // Cacher le label pour les étudiants  
            pbEdit.Visible = false;     // Cacher le bouton "Edit"
            lbEdit.Visible = false;
            pbDelete.Visible = false;   // Cacher le bouton "Delete"
            lbDelete.Visible = false;

            pbSave.Visible = true;      // Afficher le bouton "Save"
            lbSave.Visible = true;

            txtID.Enabled = false; // Disable ID field during editing  
            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbDepartment.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void dgvTeachers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                showAction();

                // Masquer le bouton "Save" dès que vous sélectionnez un enseignant  
                pbSave.Visible = false;
                lbSave.Visible = false;// Masquer le bouton "Save" ici

                DataGridViewRow row = dgvTeachers.Rows[e.RowIndex];

                // Mettez à jour les champs selon l'index  
                txtID.Text = row.Cells[0].Value.ToString(); // Teacher ID  
                txtName.Text = row.Cells[2].Value.ToString(); // Name  
                txtAddress.Text = row.Cells[5].Value.ToString(); // Address  
                dtpBirth.Value = DateTime.Parse(row.Cells[3].Value.ToString()); // Birth  

                // Récupérer le département à partir de la cellule  
                string departmentInfo = row.Cells[1].Value.ToString(); // Obtient "ID - Name"
                cbDepartment.Text = departmentInfo;  // Assigner l'info du département au ComboBox

                // Vérifiez le genre  
                rbMale.Checked = row.Cells[4].Value.ToString() == "Homme";
                rbFemale.Checked = !rbMale.Checked;

                // Si vous souhaitez extraire uniquement l'ID pour un usage futur  
                string selectedDepartmentId = departmentInfo.Split('-')[0].Trim(); // Extraire l'ID  
                cbDepartment.SelectedIndex = -1; // Désélectionner la sélection actuelle d'abord

                // Pour trouver et sélectionner le département dans le ComboBox  
                for (int i = 0; i < cbDepartment.Items.Count; i++)
                {
                    if (cbDepartment.Items[i].ToString().StartsWith(selectedDepartmentId))
                    {
                        cbDepartment.SelectedIndex = i; // Sélectionner le département correspondant  
                        break;
                    }
                }
            }
        }


        private void TeacherManager_Load(object sender, EventArgs e)
        {
            pbSave.Visible = false;
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            // TODO: Add functionality for students  
        }
        

        private void pbTeachers_Click(object sender, EventArgs e)
        {
            action = 0;
            pbTeachers.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            SetInputFieldsForAdding();
        }

        private void SetInputFieldsForAdding()
        {
            // Clear and reset input fields  
            txtID.Visible = false; // Assuming ID is hidden during add  
            label10.Visible = false;
            ClearInputs();

            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbDepartment.Enabled = true;
            dtpBirth.Enabled = true;

            rbMale.Checked = false;
            rbMale.Enabled = true;
            rbFemale.Checked = false;
            rbFemale.Enabled = true;
        }

        #endregion

        private void dgvTeachers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}