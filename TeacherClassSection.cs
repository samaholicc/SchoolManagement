using System;
using System.Data;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient; // Use MySql.Data for MySQL database connection

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

        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public static string ClassSectionID;
        public static string SubjectID;
        public static int limited;

        public TeacherClassSection()
        {
            InitializeComponent();
            LoadClasses();
        }

        private void LoadClasses()
        {
            try
            {
                string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();
                    string query = "SELECT * FROM ( SELECT a.MALOPHP AS `CLASS SECTION ID`, a.MAMH AS `SUBJECT ID`, a.MAGV AS `TEACHER ID`, a.BATDAU AS `START`, a.KETTHUC AS `FINISH`, a.LICHHOC AS `SCHEDULE`, a.SISO AS `N.O.S`, ROW_NUMBER() OVER(ORDER BY MALOPHP ASC) AS r__ FROM LOPHP a WHERE MAGV = @magv ORDER BY MALOPHP ASC ) AS temp WHERE r__ BETWEEN @start AND @end";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@magv", Login.ID);
                        cmd.Parameters.AddWithValue("@start", (currFrom - 1) * pageSize + 1);
                        cmd.Parameters.AddWithValue("@end", currFrom * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvClass.DataSource = dataTable;
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
                    string query = "SELECT * FROM ( SELECT a.MALOPHP AS `CLASS SECTION ID`, a.MAMH AS `SUBJECT ID`, a.MAGV AS `TEACHER ID`, a.BATDAU AS `START`, a.KETTHUC AS `FINISH`, a.LICHHOC AS `SCHEDULE`, a.SISO AS `N.O.S`, ROW_NUMBER() OVER(ORDER BY MALOPHP ASC) AS r__ FROM LOPHP a WHERE MAGV = @magv AND (MALOPHP LIKE @search OR MAMH LIKE @search OR BATDAU LIKE @search OR KETTHUC LIKE @search OR LICHHOC LIKE @search OR SISO LIKE @search) ORDER BY MALOPHP ASC) AS temp WHERE r__ BETWEEN @start AND @end";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@magv", Login.ID);
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        cmd.Parameters.AddWithValue("@start", (currFrom - 1) * pageSize + 1);
                        cmd.Parameters.AddWithValue("@end", currFrom * pageSize);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvClass.DataSource = dataTable;
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
            isSelected = false;
            txtSearch.Text = "";
            LoadClasses();
        }

       

    

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;

                DataGridViewRow row = dgvClass.Rows[e.RowIndex];

                ClassSectionID = row.Cells[0].Value.ToString();
                SubjectID = row.Cells[1].Value.ToString();
                limited = Int32.Parse(row.Cells[6].Value.ToString());
            }
        }

        private void dgvClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // This method can be used if needed for handling cell content clicks
        }

        private void TeacherClassSection_Load(object sender, EventArgs e)
        {

        }
    }
}

