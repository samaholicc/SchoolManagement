using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Using MySQL data access

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
            LoadDepartments(); // Charger les départements lors de l'initialisation  
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
            lbStudents.Visible = true;      // Afficher le label des étudiants  
            pbEdit.Visible = true;          // Afficher le bouton "Edit"  
            lbEdit.Visible = true;
            pbDelete.Visible = true;        // Afficher le bouton "Delete"  
            lbDelete.Visible = true;
            pbSave.Visible = false;         // Masquer le bouton "Save"  
            lbSave.Visible = false;
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            action = 0; // Définir l'action à ajouter

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true; // Afficher le bouton "Save"
            lbSave.Visible = true;

            // Préparation pour l'ajout  
            txtID.Visible = true;           // Afficher le champ ID pour l'ajout  
            label10.Visible = true;         // Afficher le label pour ID  
            txtID.Enabled = true;

            // Effacer et activer le champ Name  
            txtName.Text = "";
            txtName.Enabled = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected) // Vérifiez si une sélection a été faite  
            {
                MessageBox.Show("Please choose a department to edit!");
                return;
            }
            action = 1; // Définir l'action à éditer

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true; // Afficher le bouton "Save"
            lbSave.Visible = true;

            // Activer le champ Name pour l'édition  
            txtID.Enabled = false;           // Désactiver le champ ID pendant l'édition  
            txtName.Enabled = true;          // Activer le champ Name pour l'édition  
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            // Vérifiez que les champs nécessaires ne sont pas vides  
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text))
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
                        string query = "INSERT INTO DEP (DEP_ID, DEP_NAME) VALUES (@id, @name)";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim()); // ID du département  
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim()); // Nom du département

                        cmd.ExecuteNonQuery(); // Exécutez la requête  
                        MessageBox.Show("Add success"); // Message de succès  
                    }
                    else // Pour modifier un département existant  
                    {
                        string query = "UPDATE DEP SET DEP_NAME = @name WHERE DEP_ID = @id";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim()); // Nom du département  
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim()); // ID du département

                        cmd.ExecuteNonQuery(); // Exécutez la mise à jour  
                        MessageBox.Show("Edit success");
                    }

                    RefreshData(); // Réinitialiser le formulaire et recharger les données  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Gérer les erreurs  
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
                MessageBox.Show("Please choose a department to delete!");
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

                    MessageBox.Show("Delete success");
                    RefreshData(); // Actualisez l'interface après la suppression  
                }
                catch (MySqlException ex)
                {
                    // Check for foreign key constraint violation  
                    if (ex.Number == 1451) // Error code for foreign key constraint violation  
                    {
                        MessageBox.Show("Cannot delete this department because it is in use.");
                    }
                    else
                    {
                        MessageBox.Show("Database error: " + ex.Message); // Handle other MySQL errors  
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message); // Handle generic errors  
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
    }
}