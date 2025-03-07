using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace SchoolManagement
{
    public partial class DepartmentManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        private int action; // 0 - add, 1 - edit  
        private bool isSelected = false; // Variable pour vérifier la sélection

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }

        public DepartmentManager()
        {
            InitializeComponent();
            LoadDepartments();
      


        }


        private void LoadDepartments()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT DEP_ID AS `Department ID`, DEP_NAME AS `Name` FROM DEP";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvDepartments.DataSource = dataTable; // Assurez-vous que ce DataGridView existe  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvDepartments_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Vérifier que l'indice de la ligne est valide  
            {
                isSelected = true; // Marquer comme sélectionné  
                showAction(); // Afficher les actions possibles

                DataGridViewRow row = dgvDepartments.Rows[e.RowIndex]; // Obtenir la ligne sélectionnée

                // Remplir les champs avec les valeurs de la ligne sélectionnée  
                txtID.Text = row.Cells["Department ID"].Value.ToString(); // Assurez-vous que ce nom correspond bien  
                txtName.Text = row.Cells["Name"].Value.ToString(); // Assurez-vous que ce nom correspond bien  
            }
        }

        private void showAction()
        {
            pbStudents.Visible = true;      // Afficher le bouton "Add"  
            lbDepAdd.Visible = true;      // Afficher le label des étudiants  
            pbEdit.Visible = true;          // Afficher le bouton "Edit"  
            lbDepEdit.Visible = true;
            pbDelete.Visible = true;        // Afficher le bouton "Delete"  
            lbDeleteDep.Visible = true;
            pbSave.Visible = false;         // Masquer le bouton "Save"  
            lbSave.Visible = false;
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0; // Définir l'action à ajouter

            pbStudents.Visible = false;
            lbDepAdd.Visible = false;
            pbEdit.Visible = false;
            lbDepEdit.Visible = false;
            pbDelete.Visible = false;
            lbDeleteDep.Visible = false;
            pbSave.Visible = true; // Afficher le bouton "Save"
            lbSave.Visible = true;

            // Préparation pour l'ajout  
            txtID.Visible = true;           // Afficher le champ ID pour l'ajout  
            lbDepID.Visible = true;         // Afficher le label pour ID  
            txtID.Enabled =false;

            // Effacer et activer le champ Name  
            txtName.Text = "";
            txtName.Enabled = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected) // Vérifiez si une sélection a été faite  
            {
                MessageBox.Show(GetLocalizedMessage("Please choose a department to edit!", "Veuillez choisir un département à éditer!"));
                return;
            }
            action = 1; // Définir l'action à éditer

            pbStudents.Visible = false;
            lbDepAdd.Visible = false;
            pbEdit.Visible = false;
            lbDepEdit.Visible = false;
            pbDelete.Visible = false;
            lbDeleteDep.Visible = false;
            pbSave.Visible = true; // Afficher le bouton "Save"
            lbSave.Visible = true;

            // Activer le champ Name pour l'édition  
            txtID.Enabled = false;           // Désactiver le champ ID pendant l'édition  
            txtName.Enabled = true;          // Activer le champ Name pour l'édition  
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            // Vérifiez que les champs nécessaires ne sont pas vides  
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (action == 0) // Pour ajouter un nouveau département  
                    {
                        string query = "INSERT INTO DEP (DEP_NAME) VALUES ( @name)";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim()); // Nom du département

                        cmd.ExecuteNonQuery(); // Exécutez la requête  
                        MessageBox.Show(GetLocalizedMessage("Add success", "Ajout réussi")); // Message de succès  
                    }
                    else // Pour modifier un département existant  
                    {
                        string query = "UPDATE DEP SET DEP_NAME = @name WHERE DEP_ID = @id";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim()); // Nom du département  
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim()); // ID du département

                        cmd.ExecuteNonQuery(); // Exécutez la mise à jour  
                        MessageBox.Show(GetLocalizedMessage("Edit success", "Édition réussie"));
                    }

                    RefreshData(); // Réinitialiser le formulaire et recharger les données  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Gérer les erreurs  
            }
        }
        private string GetLocalizedMessage(string englishMessage, string frenchMessage)
        {
            if (CultureInfo.CurrentCulture.Name == "fr-FR")
            {
                return frenchMessage;  // Return the French message
            }
            else
            {
                return englishMessage;  // Return the English message
            }
        }
        private void RefreshData()
        {
            LoadDepartments(); // Rechargez les départements  
            showAction();      // Affichez les actions appropriées  
            ClearInputs();     // Vide les champs de saisie  
        }

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show(GetLocalizedMessage("Please choose a department to delete!", "Veuillez choisir un département à supprimer!"));
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
                        conn.Open();
                        string query = "DELETE FROM DEP WHERE DEP_ID = @ID";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show(GetLocalizedMessage("Delete success", "Suppression réussie"));
                    RefreshData(); // Actualisez l'interface après la suppression  
                }
                catch (MySqlException ex)
                {
                    // Check for foreign key constraint violation  
                    if (ex.Number == 1451) // Error code for foreign key constraint violation  
                    {
                        MessageBox.Show(GetLocalizedMessage("Cannot delete this department because it is in use.", "Impossible de supprimer ce département car il est en cours d'utilisation."));
                    }
                    else
                    {
                        MessageBox.Show(GetLocalizedMessage("Database error: " + ex.Message, "Erreur de base de données: " + ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetLocalizedMessage("An error occurred: " + ex.Message, "Une erreur est survenue: " + ex.Message));
                }
            }

            isSelected = false; // Reset selection  
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            RefreshData(); // Recharge les départements et réinitialise l'interface  
        }

        private void DepartmentManager_Load(object sender, EventArgs e)
        {
            // Logique additionnelle de chargement, si nécessaire  
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
                for (int col = 0; col < dgvDepartments.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1] = dgvDepartments.Columns[col].HeaderText;
                }

                // Fetch all data for the export, not just the current page
                List<DataRow> allRows = new List<DataRow>();

                // Loop through all pages and collect all data
               

                    LoadDepartments();   // This loads students for the current page

                    // Collect rows from the DataGridView
                    foreach (DataGridViewRow row in dgvDepartments.Rows)
                    {
                        if (row.IsNewRow) continue; // Skip the new row placeholder

                        DataRow dataRow = ((DataTable)dgvDepartments.DataSource).NewRow();

                        // Copy row values to DataRow
                        for (int col = 0; col < dgvDepartments.Columns.Count; col++)
                        {
                            dataRow[col] = row.Cells[col].Value.ToString();
                        }

                        allRows.Add(dataRow); // Add the row to the list
                    }
                

                // Populate Excel worksheet with all rows collected
                int rowIndex = 2; // Start from row 2 (because row 1 is the header)
                foreach (var row in allRows)
                {
                    for (int col = 0; col < dgvDepartments.Columns.Count; col++)
                    {
                        worksheet.Cells[rowIndex, col + 1] = row[col].ToString();
                    }
                    rowIndex++;
                }

                MessageBox.Show(GetLocalizedMessage("Exported to Excel successfully.", "Exportation vers Excel réussie."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error exporting to Excel: " + ex.Message, "Erreur lors de l'exportation vers Excel : " + ex.Message));
            }
            LoadDepartments();
 
        
        
        }
}
    }
